using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.DTOs.Shop;
using GHSTShipping.Application.Features.Shops.Commands;
using GHSTShipping.Application.Features.Shops.Queries;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    public class ShopsController() : BaseApiController
    {
        [HttpGet, Authorize]
        [Route("{shopId}")]
        public async Task<BaseResult<ShopViewDetailDto>> Detail([FromRoute] Guid? shopId)
           => await Mediator.Send(new GetShopDetailRequest() { ShopId = shopId });

        [HttpPut, Authorize]
        [Route("{shopId}")]
        public async Task<BaseResult<ShopViewDetailDto>> ChangeOperationConfig([FromRoute] Guid? shopId, [FromBody] ChangeOperationConfigRequest request)
          => await Mediator.Send(request);

        [HttpPut, Authorize]
        [Route("{shopId}")]
        public async Task<BaseResult<ShopViewDetailDto>> GhnShopId([FromRoute] Guid? shopId, [FromBody] UpdateGhnShopIdrRequest request)
          => await Mediator.Send(request);

        [HttpGet, Authorize]
        public async Task<BaseResult<PaginationResponseDto<ShopPricePlanDto>>> Prices([FromQuery] GetShopPriceRequest request)
            => await Mediator.Send(request);

        [HttpPost, Authorize]
        public async Task<BaseResult> Prices([FromBody] CreateShopPriceCommand request)
            => await Mediator.Send(request);

        [HttpPut, Authorize]
        [Route("{Id}")]
        public async Task<BaseResult> Prices([FromRoute] System.Guid Id, CreateShopPriceCommand request)
        {
            request.Id = Id;

            return await Mediator.Send(request);
        }

        [HttpDelete, Authorize]
        [Route("{Id}")]
        public async Task<BaseResult> Prices([FromRoute] System.Guid Id, [FromBody] IEnumerable<Guid> Ids = null)
        {
            return await Mediator.Send(new DeleteShopPriceCommand() { Id = Id, Ids = Ids });
        }
    }
}
