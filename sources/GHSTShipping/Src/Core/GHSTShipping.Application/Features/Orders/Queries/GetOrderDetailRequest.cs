using AutoMapper;
using AutoMapper.QueryableExtensions;
using Delivery.GHN;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Queries
{
    public class GetOrderDetailRequest : IRequest<BaseResult<OrderDetailDto>>
    {
        public Guid? OrderId { get; set; }

        public Guid? ShopId { get; set; }
    }

    public class GetOrderDetailRequestHandler(
        IOrderRepository orderRepository,
        IShopRepository shopRepository,
        IGhnApiClient ghnApiClient,
        IPartnerConfigService partnerConfigService,
        IAuthenticatedUserService authenticatedUserService,
        MapperConfiguration mapperConfiguration
        ) : IRequestHandler<GetOrderDetailRequest, BaseResult<OrderDetailDto>>
    {
        public async Task<BaseResult<OrderDetailDto>> Handle(GetOrderDetailRequest request, CancellationToken cancellationToken)
        {
            // Early return if OrderId is invalid
            if (!request.OrderId.HasValue || request.OrderId == Guid.Empty)
            {
                return BaseResult<OrderDetailDto>.Failure(new Error(ErrorCode.NotFound));
            }

            // Fetch the order details
            var order = await orderRepository
                .Where(i => i.Id == request.OrderId)
                .Include(i => i.Items)
                .ProjectTo<OrderDetailDto>(mapperConfiguration)
                .FirstOrDefaultAsync(cancellationToken);

            var orderSyncInfo = await orderRepository
                .Where(i => i.Id == request.OrderId)
                .Select(i => new
                {
                    i.Id,
                    i.ApiKey,
                    i.ProdEnv,
                    i.PartnerShopId,
                }).FirstOrDefaultAsync(cancellationToken);

            // Early return if the order is not found
            if (order == null)
            {
                return BaseResult<OrderDetailDto>.Failure(new Error(ErrorCode.NotFound));
            }

            // Hide some fields when
            // Request to get detail as shop or request is not admin
            if (request.ShopId.HasValue || authenticatedUserService.IsAdmin == false)
            {
                order.Height = order.RootHeight;
                order.Length = order.RootLength;
                order.Width = order.RootWidth;
                order.Weight = order.RootWeight;
                order.ConvertedWeight = order.RootConvertedWeight;
                order.CalculateWeight = order.RootCalculateWeight;
            }

            // If the PrivateOrderCode is empty, return the order details
            if (string.IsNullOrWhiteSpace(order.PrivateOrderCode))
            {
                return BaseResult<OrderDetailDto>.Ok(order);
            }

            // Fetch API config for the delivery partner (GHN)
            var apiConfig = new Delivery.GHN.Models.ApiConfig(orderSyncInfo.ProdEnv, orderSyncInfo.ApiKey, orderSyncInfo.PartnerShopId);

            // Fetch the delivery order details from GHN
            var apiResult = await ghnApiClient.DetailDeliveryOrderAsync(apiConfig, order.PrivateOrderCode);

            // Update order status if API call succeeds
            if (apiResult.Code == 200)
            {
                //order.Status = apiResult.Data.status;
            }

            return BaseResult<OrderDetailDto>.Ok(order);
        }
    }
}
