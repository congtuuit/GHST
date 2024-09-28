using GHSTShipping.Application.DTOs.Account.Requests;
using GHSTShipping.Application.DTOs.Account.Responses;
using GHSTShipping.Application.Features.Shops.Commands;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    public class AccountController(IAccountServices accountServices) : BaseApiController
    {
        [HttpPost]
        public async Task<BaseResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request)
            => await accountServices.Authenticate(request);

        [HttpPut, Authorize]
        public async Task<BaseResult> ChangeUserNameAsync(ChangeUserNameRequest model)
            => await accountServices.ChangeUserNameAsync(model);

        [HttpPut, Authorize]
        public async Task<BaseResult> ChangePasswordAsync(ChangePasswordRequest model)
            => await accountServices.ChangePasswordAsync(model);

        [HttpPost]
        public async Task<BaseResult<Guid>> RegisterAsync([FromBody] CreateShopCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        [Obsolete]
        public async Task<BaseResult<AuthenticationResponse>> Start()
        {
            var ghostUsername = await accountServices.RegisterGhostAccountAsync();
            return await accountServices.AuthenticateByUserNameAsync(ghostUsername.Data);
        }
    }
}
