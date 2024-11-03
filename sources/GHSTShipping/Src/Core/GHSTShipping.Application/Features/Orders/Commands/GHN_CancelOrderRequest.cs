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
        private ApiConfig apiConfig;

        public async Task<BaseResult> Handle(GHN_CancelOrderRequest request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.Where(i => request.OrderIds.Contains(i.Id))
                .Select(i => new Domain.Entities.Order
                {
                    Id = i.Id,
                    ShopId = i.ShopId,
                    CurrentStatus = i.CurrentStatus,
                    private_order_code = i.private_order_code
                })
                .ToListAsync(cancellationToken);

            if (orders.Count == 0)
            {
                return BaseResult.Ok();
            }

            var orderHistories = new List<OrderStatusHistory>();
            orderRepository.ModifyRange(orders);
            foreach (var item in orders)
            {
                item.Cancel();

                orderHistories.Add(new OrderStatusHistory
                {
                    OrderId = item.Id,
                    Status = OrderStatus.CANCEL,
                    ChangedBy = authenticatedUserService.UserId,
                    Notes = "Cancel order"
                });
            }

            if (orderHistories.Count > 0)
            {
                await orderHistoryRepository.AddRangeAsync(orderHistories);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            try
            {
                // Get API config to send request to GHN
                var shopId = orders.First().ShopId;
                var apiConfig = await partnerConfigService.GetApiConfigAsync(Domain.Enums.EnumDeliveryPartner.GHN, shopId.Value);
                var orderCodes = orders.Where(i => !string.IsNullOrWhiteSpace(i.private_order_code)).Select(i => i.private_order_code).ToList();
                if (orderCodes.Count > 0)
                {
                    var apiResult = await ghnApiClient.CancelOrderAsync(apiConfig, orderCodes);
                    if (apiResult.Code == 200)
                    {
                        foreach (var orderCode in orderCodes)
                        {
                            await mediator.Send(new GHN_SyncOrderRequest
                            {
                                PartnerOrderCode = orderCode
                            },
                            cancellationToken);
                        }

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

            return BaseResult.Ok();
        }
    }
}
