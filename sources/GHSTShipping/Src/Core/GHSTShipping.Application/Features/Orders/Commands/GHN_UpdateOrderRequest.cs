using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class GHN_UpdateOrderRequest : IRequest<BaseResult>
    {
        public Guid OrderId { get; set; }

        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ConvertRate { get; set; } = 1;
    }

    public class GHN_UpdateOrderRequestHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IMediator mediator
        ) : IRequestHandler<GHN_UpdateOrderRequest, BaseResult>
    {
        public async Task<BaseResult> Handle(GHN_UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.Where(i => request.OrderId == i.Id)
                .Select(i => new Domain.Entities.Order
                {
                    Id = i.Id,
                    ShopId = i.ShopId,
                    CurrentStatus = i.CurrentStatus,
                    PartnerShopId = i.PartnerShopId,
                    DeliveryPricePlaneId = i.DeliveryPricePlaneId,

                    Length = i.Length,
                    Width = i.Width,
                    Height = i.Height,
                    Weight = i.Weight,
                    ConvertRate = i.ConvertRate
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                return BaseResult.Failure(new Error(ErrorCode.NotFound, "Không tìm thấy đơn hàng"));
            }

            orderRepository.Modify(order);
            if (request.Length > 0) {
                order.Length = request.Length;
            }
            if (request.Width > 0)
            {
                order.Width = request.Width;
            }
            if (request.Height > 0)
            {
                order.Height = request.Height;
            }
            if (request.Weight > 0)
            {
                order.Weight = request.Weight;
            }

            var pricePlan = await mediator.Send(new GHN_OrderShippingCostCalcRequest
            {
                ShopDeliveryPricePlaneId = order.DeliveryPricePlaneId.Value,
                Height = order.Height,
                Length = order.Length,
                Width = order.Width,
                Weight = order.Weight,
            });
            
            order.OrrverideDeliveryFee(pricePlan.ShippingCost);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResult.Ok();
        }
    }
}
