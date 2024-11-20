using Delivery.GHN;
using Delivery.GHN.Models;
using GHSTShipping.Application.Helpers;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class GHN_JobSyncOrderCommand : IRequest<BaseResult>
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }

    public class GHN_JobSyncOrderCommandHandler(IServiceScopeFactory serviceScopeFactory) : IRequestHandler<GHN_JobSyncOrderCommand, BaseResult>
    {
        public async Task<BaseResult> Handle(GHN_JobSyncOrderCommand request, CancellationToken cancellationToken)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var ghnApiClient = scope.ServiceProvider.GetRequiredService<IGhnApiClient>();

                try
                {
                    var shopConfigs = await (from shop in unitOfWork.Shops.All()
                                             join config in unitOfWork.ShopPartnerConfigs.All()
                                             on shop.Id equals config.ShopId
                                             where shop.IsVerified && shop.ParentId == null &&
                                                   config.PartnerConfig.DeliveryPartner == EnumDeliveryPartner.GHN
                                             select new
                                             {
                                                 shop.Id,
                                                 shop.UniqueCode,
                                                 config.PartnerConfig.ProdEnv,
                                                 config.PartnerConfig.ApiKey,
                                                 config.PartnerShopId
                                             })
                                     .ToListAsync(cancellationToken);

                    if (shopConfigs.Count == 0) return BaseResult.Ok();

                    var apiConfigs = shopConfigs
                        .Select(i => new
                        {
                            i.Id,
                            i.UniqueCode,
                            i.ApiKey,
                            ApiConfig = new ApiConfig(i.ProdEnv, i.ApiKey, i.PartnerShopId)
                        })
                        .ToList();

                    var uniqueConfigs = apiConfigs.GroupBy(i => i.ApiKey).ToList();

                    TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime vietnamNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
                    DateTime startOfDayVietnam = new DateTime(vietnamNow.Year, vietnamNow.Month, vietnamNow.Day, 0, 0, 0, DateTimeKind.Unspecified);

                    startOfDayVietnam = startOfDayVietnam.AddDays(-10);
                    DateTimeOffset startOfDayVietnamOffset = new DateTimeOffset(startOfDayVietnam, vietnamTimeZone.GetUtcOffset(startOfDayVietnam));

                    DateTime endOfDayVietnam = new DateTime(vietnamNow.Year, vietnamNow.Month, vietnamNow.Day, 23, 59, 59, DateTimeKind.Unspecified);
                    DateTimeOffset endOfDayVietnamOffset = new DateTimeOffset(endOfDayVietnam, vietnamTimeZone.GetUtcOffset(endOfDayVietnam));

                    foreach (var uniquerConfig in uniqueConfigs)
                    {
                        var config = uniquerConfig.First();

                        var queryBatch = 100;
                        var offset = 0;
                        var batchOrders = new List<Order>();
                        var batchOrderItems = new List<OrderItem>();
                        var continueSync = true;

                        while (continueSync)
                        {
                            var searchParams = new ShippingOrderSearchRequest
                            {
                                ShopId = int.Parse(config.ApiConfig.ShopId),
                                Limit = queryBatch,
                                Offset = offset, // Sử dụng như số trang
                                FromTime = DateTimeHelper.ConvertToUnixTimestamp(startOfDayVietnam),
                                ToTime = DateTimeHelper.ConvertToUnixTimestamp(endOfDayVietnamOffset),
                            };

                            var ghnOrdersResponse = await ghnApiClient.SearchOrdersAsync(config.ApiConfig, searchParams);

                            // Dừng nếu không có thêm đơn hàng nào
                            if (ghnOrdersResponse.Data == null || ghnOrdersResponse.Data.Count == 0)
                            {
                                continueSync = false;
                                break;
                            }

                            foreach (var shopConfig in uniquerConfig)
                            {
                                var (orders, orderItems) = await GHN_SyncOrderRequestHandler.ListOrderMappingAsync(
                                    ghnOrdersResponse,
                                    shopConfig.Id,
                                    shopConfig.UniqueCode,
                                    shopConfig.ApiConfig,
                                    ghnApiClient);

                                batchOrders.AddRange(orders);
                                batchOrderItems.AddRange(orderItems);
                            }

                            // Tăng offset để lấy trang tiếp theo
                            offset++;

                            // Kiểm tra nếu tất cả các trang đã được xử lý
                            if (offset * queryBatch >= ghnOrdersResponse.Total)
                            {
                                continueSync = false;
                            }
                        }

                        await GHN_SyncOrderRequestHandler.NEW_BatchSaveAsync(unitOfWork, batchOrders, batchOrderItems);
                    }
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error(ex, "An error occurred while executing the cron job.");
                }
            }

            return BaseResult.Ok();
        }
    }
}
