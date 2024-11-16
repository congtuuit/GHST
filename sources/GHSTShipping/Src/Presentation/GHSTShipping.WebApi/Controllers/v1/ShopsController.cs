using GHSTShipping.Application.DTOs.Shop;
using GHSTShipping.Application.Features.Shops.Commands;
using GHSTShipping.Application.Features.Shops.Queries;
using GHSTShipping.Application.Wrappers;
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

        [HttpGet, Authorize]
        public async Task<BaseResult<List<BasicShopInfoDto>>> List([FromQuery] GetShopByIdQuery request)
           => await Mediator.Send(request);

        [HttpPut, Authorize]
        [Route("{shopId}")]
        public async Task<BaseResult<ShopViewDetailDto>> ChangeOperationConfig([FromRoute] Guid? shopId, [FromBody] ChangeOperationConfigRequest request)
          => await Mediator.Send(request);

        [HttpPut, Authorize]
        [Route("{shopId}")]
        public async Task<BaseResult<ShopViewDetailDto>> GhnShopId([FromRoute] Guid? shopId, [FromBody] UpdateGhnShopIdrRequest request)
          => await Mediator.Send(request);

        [HttpPost, Authorize]
        public async Task<BaseResult<Guid>> Create([FromBody] CreateChildShopCommand request)
           => await Mediator.Send(request);


        
    }
}
