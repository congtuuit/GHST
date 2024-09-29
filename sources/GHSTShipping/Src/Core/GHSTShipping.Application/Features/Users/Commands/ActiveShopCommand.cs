using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Users.Commands
{
    public class ActiveShopCommand : IRequest<BaseResult>
    {
        public Guid ShopId { get; set; }
    }

    public class ActiveShopCommandHandler(
        IAuthenticatedUserService authenticatedUser,
        IShopRepository shopRepository,
        IAccountServices accountServices,
        IUnitOfWork unitOfWork,
        ILogger<ActiveShopCommandHandler> logger
        ) : IRequestHandler<ActiveShopCommand, BaseResult>
    {
        public async Task<BaseResult> Handle(ActiveShopCommand request, CancellationToken cancellationToken)
        {
            var userId = authenticatedUser.UserId;
            Guid uidGuid = Guid.Parse(userId);
            var shop = await shopRepository.Where(i => i.Id == request.ShopId && i.IsVerified == false).Select(i => new Shop
            {
                Id = i.Id,
                IsVerified = i.IsVerified,
            })
            .FirstOrDefaultAsync(cancellationToken);

            if (shop != null)
            {
                using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
                try
                {
                    shop.IsVerified = true;
                    shopRepository.ModifyProperty(shop, i => i.IsVerified);
                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);

                    await transaction.RollbackAsync(cancellationToken);
                }
            }

            return BaseResult.Ok();
        }
    }
}
