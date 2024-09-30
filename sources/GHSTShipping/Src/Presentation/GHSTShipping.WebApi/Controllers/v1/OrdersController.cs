using Delivery.GHN.Models;
using GHSTShipping.Application.Features.Orders.Commands;
using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrdersController() : BaseApiController
    {
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
