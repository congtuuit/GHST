using GHSTShipping.Application.Features.Configs.Commands;
using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    public class DeliveryPricePlaneController() : BaseApiController
    {
        [HttpGet]
        public async Task<BaseResult<Guid>> List([FromBody] UpsertDeliveryPricePlaneCommand command)
        {
            var id = await Mediator.Send(command);

            return BaseResult<Guid>.Ok(id);
        }

        [HttpPost]
        public async Task<BaseResult<Guid>> Upsert([FromBody] UpsertDeliveryPricePlaneCommand command)
        {
            var id = await Mediator.Send(command);

            return BaseResult<Guid>.Ok(id);
        }

        [HttpDelete("{id}")]
        public async Task<BaseResult> Delete(Guid id)
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
