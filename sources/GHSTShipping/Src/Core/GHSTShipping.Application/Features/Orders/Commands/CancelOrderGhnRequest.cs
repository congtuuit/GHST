using Delivery.GHN;
using Delivery.GHN.Models;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class CancelOrderGhnRequest : IRequest<BaseResult>
    {
        public List<Guid> OrderIds { get; set; }
    }

    public class CancelOrderGhnRequestHandler(
        IGhnApiClient ghnApiClient,
        IPartnerConfigService partnerConfigService,
        IAuthenticatedUserService authenticatedUserService,
        IShopRepository shopRepository,
        IOrderRepository orderRepository,
        IOrderHistoryRepository orderHistoryRepository,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<CancelOrderGhnRequest, BaseResult>
    {
        private ApiConfig apiConfig;

        public async Task<BaseResult> Handle(CancelOrderGhnRequest request, CancellationToken cancellationToken)
        {
            var userId = authenticatedUserService.UId;

            var orders = await orderRepository.Where(i => request.OrderIds.Contains(i.Id))
                .Select(i => new Domain.Entities.Order
                {
                    Id = i.Id,
                    CurrentStatus = i.CurrentStatus,
                    private_order_code = i.private_order_code
                })
                .ToListAsync();

            if (orders.Any())
            {
                var orderHistories = new List<OrderStatusHistory>();
                orderRepository.ModifyRange(orders);
                foreach (var item in orders)
                {
                    item.CurrentStatus = OrderStatus.CANCEL;

                    orderHistories.Add(new OrderStatusHistory
                    {
                        OrderId = item.Id,
                        Status = OrderStatus.CANCEL,
                        ChangedBy = userId.ToString(),
                        Notes = "Cancel order"
                    });
                }

                if (orderHistories.Any())
                {
                    await orderHistoryRepository.AddRangeAsync(orderHistories);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);


                // Send request to GHN
                var shop = await shopRepository.Where(i => i.AccountId == userId).Select(i => new
                {
                    ShopId = i.Id,
                    i.UniqueCode,
                    i.AllowPublishOrder,
                })
                .FirstOrDefaultAsync(cancellationToken);

                try
                {
                    // Get API config to send request to GHN
                    var partnerConfig = await partnerConfigService.GetPartnerConfigAsync(Domain.Enums.EnumDeliveryPartner.GHN);
                    var apiConfig = new ApiConfig(partnerConfig.ProdEnv, partnerConfig.ApiKey);

                    var orderCodes = orders.Where(i => !string.IsNullOrWhiteSpace(i.private_order_code)).Select(i => i.private_order_code).ToList();
                    if (orderCodes.Any())
                    {
                        var apiResult = await ghnApiClient.CancelOrderAsync(apiConfig, orderCodes);
                        if (apiResult.Code == 200)
                        {
                            return BaseResult<CancelOrderResponse>.Ok(apiResult.Data);
                        }
                        else
                        {
                            return BaseResult<CancelOrderResponse>.Failure(new Error(ErrorCode.ErrorInIdentity, apiResult.Message));
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return BaseResult.Ok();
        }
    }
}
