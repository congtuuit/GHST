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
        [Route("ghn/list")]
        public async Task<BaseResult<PaginationResponseDto<OrderDto>>> GetOrders([FromQuery] GetOrderPagedListRequest request)
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
        public async Task<BaseResult<CreateDeliveryOrderResponse>> CreateGhnAsync([FromBody] CreateGhnOrderRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut]
        [Route("ghn/cancel")]
        public async Task<BaseResult<CancelOrderResponse>> CancelGhnAsync([FromBody] CancelOrderGhnRequest request)
        {
            return await Mediator.Send(request);
        }


    }
}
