using GHSTShipping.Application.Features.Configs.Commands;
using GHSTShipping.Application.Features.Configs.Queries;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    public class DeliveryPricePlaneController() : BaseApiController
    {
        [HttpGet]
        public async Task<BaseResult<List<ShopDeliveryPricePlaneDto>>> List([FromQuery] GetShopDeliveryPricePlanesRequest request)
        {
            var response = await Mediator.Send(request);

            return response;
        }

        [HttpPost]
        public async Task<BaseResult<Guid>> Upsert([FromBody] UpsertDeliveryPricePlaneCommand command)
        {
            var id = await Mediator.Send(command);

            return BaseResult<Guid>.Ok(id);
        }

        [HttpPost]
        public async Task<BaseResult<bool>> Assign([FromBody] AssignDeliveryPricePlanesToShopCommand command)
        {
            var response = await Mediator.Send(command);

            return BaseResult<bool>.Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<BaseResult> Delete([FromRoute] Guid id)
        {
            var result = await Mediator.Send(new DeleteDeliveryPricePlaneCommand(id));

            if (!result)
            {
                return BaseResult.Failure();
            }

            return BaseResult.Ok();
        }
    }
}
