using GHSTShipping.Application.DTOs.Account;
using GHSTShipping.Application.DTOs.Account.Requests;
using GHSTShipping.Application.DTOs.Account.Responses;
using GHSTShipping.Application.Features.Users.Commands;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    public class AccountController(IAccountServices accountServices, IUserSessionService userSessionService, IAuthenticatedUserService authenticatedUserService) : BaseApiController
    {
        [HttpPost]
        public async Task<BaseResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        public async Task<BaseResult> LogoutAsync()
        {
            var sessionId = authenticatedUserService.SessionId;
            if (sessionId.HasValue)
            {
                await userSessionService.LogoutSession(sessionId.Value);
            }

            return await accountServices.SignOutAsync();
        }

        [HttpPost]
        public async Task<BaseResult<Guid>> RegisterAsync([FromBody] CreateShopCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        public async Task<BaseResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
        {
            return await accountServices.HandleSendEmailForgotPasswordAsync(request);
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

        [HttpGet]
        public async Task<BaseResult<List<UserSessionDto>>> GetActiveSessions(Guid userId)
        {
            var sessions = await userSessionService.GetActiveSessions(userId);

            return sessions;
        }

    }
}
