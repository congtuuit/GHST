using Delivery.GHN.Models;
using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.DTOs.Orders;
using GHSTShipping.Application.Features.Orders.Commands;
using GHSTShipping.Application.Features.Orders.Queries;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrdersController() : BaseApiController
    {
        [HttpGet]
        [Route("list")]
        public async Task<BaseResult<PaginationResponseDto<OrderDto>>> GetOrders([FromQuery] GetOrderPagedListRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpGet]
        [Route("group-by-shops")]
        public async Task<BaseResult<PaginationResponseDto<ShopViewReportDto>>> OrdersGroupByShops([FromQuery] GetOrdersGroupByShopRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpGet]
        [Route("metadata")]
        public async Task<BaseResult<GetOrderMetadataResponse>> MetaData([FromQuery] GetOrderMetadataRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpGet]
        [Route("ghn/detail/{orderId}")]
        public async Task<BaseResult<OrderDetailDto>> GetOrderDetail([FromRoute] Guid orderId)
        {
            return await Mediator.Send(new GetOrderDetailRequest() { OrderId = orderId });
        }

        [HttpPost]
        [Route("ghn/create")]
        public async Task<BaseResult<CreateDeliveryOrderResponse>> CreateGhnAsync([FromBody] GHN_CreateOrderRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        [Route("ghn/update/{Id}")]
        public async Task<BaseResult<CreateDeliveryOrderResponse>> UpdateGhnAsync([FromRoute] Guid? Id, [FromBody] GHN_CreateOrderRequest request)
        {
            if (Id.HasValue)
            {
                request.OrderId = Id;

                return await Mediator.Send(request);
            }

            return BaseResult<CreateDeliveryOrderResponse>.Failure();
        }

        [HttpPut]
        [Route("ghn/cancel")]
        public async Task<BaseResult> CancelGhnAsync([FromBody] GHN_CancelOrderRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut]
        [Route("ghn/confirm/{orderId}")]
        public async Task<BaseResult> ConfirmGhnOrderAsync([FromRoute] Guid orderId)
        {
            return await Mediator.Send(new GHN_ConfirmOrderRequest() { OrderId = orderId });
        }

        [HttpPut]
        [Route("ghn/update")]
        public async Task<BaseResult> UpdateGhnOrderAsync([FromBody] GHN_UpdateOrderWeightRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpGet]
        [Route("ghn/count-order-by-status/{shopId}")]
        public async Task<BaseResult<string>> CountOrderByStatus([FromRoute] Guid shopId)
        {
            var response = await Mediator.Send(new GHN_CountOrderByStatusRequest() { ShopId = shopId });

            return BaseResult<string>.Ok(response);
        }

        [HttpPost("calculate-shipping-cost")]
        public async Task<BaseResult<OrderShippingCostDto>> CalculateShippingCost([FromBody] GHN_OrderShippingCostCalcRequest request)
        {
            var result = await Mediator.Send(request);

            return BaseResult<OrderShippingCostDto>.Ok(result);
        }

    }
}
