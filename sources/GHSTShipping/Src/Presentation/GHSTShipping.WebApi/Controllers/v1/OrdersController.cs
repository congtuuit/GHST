using Delivery.GHN.Models;
using GHSTShipping.Application.DTOs;
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
            return await Mediator.Send(new GHN_ConfirmOrderRequest() { OrderId = orderId});
        }

        [HttpGet]
        [Route("ghn/count-order-by-status/{shopId}")]
        public async Task<BaseResult<string>> CountOrderByStatus([FromRoute] Guid shopId)
        {
            var response = await Mediator.Send(new GHN_CountOrderByStatusRequest() { ShopId = shopId });

            return BaseResult<string>.Ok(response);
        }

    }
}
