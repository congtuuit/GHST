using GHSTShipping.Application.DTOs.Account.Requests;
using GHSTShipping.Application.DTOs.Account.Responses;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Users.Commands
{
    public class AuthenticationRequestHandler(
        IAccountServices accountServices,
        IShopRepository shopRepository
        ) : IRequestHandler<AuthenticationRequest, BaseResult<AuthenticationResponse>>
    {
        public async Task<BaseResult<AuthenticationResponse>> Handle(AuthenticationRequest request, CancellationToken cancellationToken)
        {
            var authenResult = await accountServices.Authenticate(request);
            if (authenResult.Success)
            {
                if (authenResult.Data.Roles.Any(i => i.Contains(AccountTypeConstants.ADMIN)))
                {
                    authenResult.Data.IsVerified = true;

                    return authenResult;
                }
                else
                {
                    var uid = Guid.Parse(authenResult.Data.Id);
                    var shop = await shopRepository.Where(i => i.AccountId == uid)
                        .Select(i => new
                        {
                            i.Id,
                            i.IsVerified
                        })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (shop != null)
                    {
                        if (shop.IsVerified)
                        {
                            authenResult.Data.IsVerified = true;

                            return authenResult;
                        }
                        else
                        {
                            return authenResult;
                        }
                    }
                }
            }

            return BaseResult<AuthenticationResponse>.Failure(new Error(ErrorCode.NotFound));
        }
    }
}
