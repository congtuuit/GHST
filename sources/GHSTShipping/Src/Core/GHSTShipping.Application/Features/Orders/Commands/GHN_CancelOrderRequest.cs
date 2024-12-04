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
using static CreateDeliveryOrderRequest;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class GHN_CancelOrderRequest : IRequest<BaseResult>
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
        IUnitOfWork unitOfWork,
        IMediator mediator
        ) : IRequestHandler<GHN_CancelOrderRequest, BaseResult>
    {
        public async Task<BaseResult> Handle(GHN_CancelOrderRequest request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.Where(i => request.OrderIds.Contains(i.Id))
                .Select(i => new Domain.Entities.Order
                {
                    Id = i.Id,
                    ShopId = i.ShopId,
                    CurrentStatus = i.CurrentStatus,
                    private_order_code = i.private_order_code,
                    ProdEnv = i.ProdEnv,
                    ApiKey = i.ApiKey,
                })
                .ToListAsync(cancellationToken);

            if (orders.Count == 0)
            {
                return BaseResult.Ok();
            }

            var orderHistories = new List<OrderStatusHistory>();
            orderRepository.ModifyRange(orders);
            
            try
            {
                // Get API config to send request to GHN
                foreach (var o in orders)
                {
                    // Nếu đơn hàng không phải là đơn bên GHN thì bỏ qua
                    if (string.IsNullOrWhiteSpace(o.private_order_code))
                    {
                        o.Cancel();
                        orderHistories.Add(new OrderStatusHistory
                        {
                            OrderId = o.Id,
                            Status = OrderStatus.CANCEL,
                            ChangedBy = authenticatedUserService.UserId,
                            Notes = "Cancel order"
                        });
                        continue;
                    }

                    var ghnOrderCode = o.private_order_code;
                    var apiConfig = new ApiConfig(o.ProdEnv, o.ApiKey);
                    var apiResult = await ghnApiClient.CancelOrderAsync(apiConfig, new List<string>() { ghnOrderCode });
                    if (apiResult.Code == 200)
                    {
                        o.Cancel();
                        orderHistories.Add(new OrderStatusHistory
                        {
                            OrderId = o.Id,
                            Status = OrderStatus.CANCEL,
                            ChangedBy = authenticatedUserService.UserId,
                            Notes = "Cancel order"
                        });

                        await mediator.Send(new GHN_SyncOrderRequest
                        {
                            ShopId = o.ShopId.Value,
                            PartnerOrderCode = ghnOrderCode,
                        },cancellationToken);
                    }
                }

                if (orderHistories.Count > 0)
                {
                    await orderHistoryRepository.AddRangeAsync(orderHistories);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return BaseResult<CancelOrderResponse>.Ok();
            }
            catch (Exception ex)
            {
            }

            return BaseResult.Ok();
        }
    }
}
