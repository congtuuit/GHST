using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.DTOs.User;
using GHSTShipping.Application.Features.Users.Commands;
using GHSTShipping.Application.Features.Users.Queries;
using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    public class UsersController() : BaseApiController
    {
        [HttpGet, Authorize]
        public async Task<BaseResult> Notice() => await Mediator.Send(new GetNoticesRequest());

        [HttpGet, Authorize]
        public async Task<BaseResult<PaginationResponseDto<ShopDto>>> Shops([FromQuery] GetShopPagedListRequest request) => await Mediator.Send(request);

        [HttpPut, Authorize]
        [Route("{shopId}")]
        public async Task<BaseResult> ActiveShop([FromRoute] Guid shopId) => await Mediator.Send(new ActiveShopCommand() { ShopId = shopId });
    }
}
