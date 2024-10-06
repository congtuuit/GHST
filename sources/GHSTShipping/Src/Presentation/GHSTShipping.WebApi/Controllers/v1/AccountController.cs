using GHSTShipping.Application.DTOs.Account.Requests;
using GHSTShipping.Application.DTOs.Account.Responses;
using GHSTShipping.Application.Features.Users.Commands;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    public class AccountController(IAccountServices accountServices) : BaseApiController
    {
        [HttpPost]
        public async Task<BaseResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {

            return await accountServices.Authenticate(request);
        }

        [HttpPost]
        public async Task<BaseResult> LogoutAsync() => await accountServices.SignOutAsync();

        [HttpPost]
        public async Task<BaseResult<Guid>> RegisterAsync([FromBody] CreateShopCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut]
        public async Task<BaseResult> ResetPasswordAsync([FromBody] ResetPasswordCommand command)
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
