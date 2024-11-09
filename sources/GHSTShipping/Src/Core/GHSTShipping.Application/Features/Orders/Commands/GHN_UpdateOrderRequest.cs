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
    }

    public class GHN_UpdateOrderRequestHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository
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

                    Length = i.Length,
                    Width = i.Width,
                    Height = i.Height,
                    Weight = i.Weight,
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

            order.CalcConvertedWeight();
            var convertedWeight = (int)order.ConvertedWeight;
            var price = await GHN_CreateOrderRequestHandler.CalculateDeliveryFeeAsync(unitOfWork, convertedWeight, order.ShopId.Value);
            order.OrrverideDeliveryFee(price);

            await unitOfWork.SaveChangesAsync();

            return BaseResult.Ok();
        }
    }
}
