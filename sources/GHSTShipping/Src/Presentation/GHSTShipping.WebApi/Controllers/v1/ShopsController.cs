using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.Features.Shops.Commands;
using GHSTShipping.Application.Features.Shops.Queries;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    public class ShopsController() : BaseApiController
    {
        [HttpGet, Authorize]
        public async Task<BaseResult<PaginationResponseDto<ShopPricePlanDto>>> Prices([FromQuery] GetShopPriceRequest request)
            => await Mediator.Send(request);

        [HttpPost, Authorize]
        public async Task<BaseResult<System.Guid>> Prices([FromBody] CreateShopPriceCommand request)
            => await Mediator.Send(request);

        [HttpPut, Authorize]
        [Route("{Id}")]
        public async Task<BaseResult<System.Guid>> Prices([FromRoute] System.Guid Id, CreateShopPriceCommand request)
        {
            request.Id = Id;

            return await Mediator.Send(request);
        }

        [HttpDelete, Authorize]
        [Route("{Id}")]
        public async Task<BaseResult> Prices([FromRoute] System.Guid Id)
        {
            return await Mediator.Send(new DeleteShopPriceCommand() { Id = Id});
        }
    }
}
