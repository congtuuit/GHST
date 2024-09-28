﻿using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Shops.Commands
{
    public class ResetPasswordCommand : IRequest<BaseResult>
    {
        public string Token { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordCommandHandler(IAccountServices accountServices) : IRequestHandler<ResetPasswordCommand, BaseResult>
    {
        public async Task<BaseResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                return BaseResult.Failure(new Error(ErrorCode.ModelStateNotValid, "Bad request"));
            }

            if (request.Password != request.ConfirmPassword)
            {
                return BaseResult.Failure(new Error(ErrorCode.FieldDataInvalid, "Password not matching"));
            }

            var resetPassword = await accountServices.SetPasswordViaSecurityStampAsync(request.Token, request.Password);

            return resetPassword;
        }
    }
}