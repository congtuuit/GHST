using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.DTOs.User;
using GHSTShipping.Application.Extensions;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Parameters;
using GHSTShipping.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Users.Queries
{
    public class GetShopPagedListRequest : PaginationRequestParameter, IRequest<BaseResult<PaginationResponseDto<ShopDto>>>
    {
    }

    public class GetShopPagedListRequestHandler(
        IUnitOfWork unitOfWork,
        IAuthenticatedUserService authenticatedUser,
        IShopRepository shopRepository,
        IShopPartnerConfigRepository shopPartnerConfigRepository,
        IAccountServices accountServices
        ) : IRequestHandler<GetShopPagedListRequest, BaseResult<PaginationResponseDto<ShopDto>>>
    {
        public async Task<BaseResult<PaginationResponseDto<ShopDto>>> Handle(GetShopPagedListRequest request, CancellationToken cancellationToken)
        {
            var userId = authenticatedUser.UserId;
            Guid uidGuid = Guid.Parse(userId);

            int skipCount = (request.PageNumber - 1) * request.PageSize;
            PaginationResponseDto<ShopDto> pagingResult = await unitOfWork.Shops
                .All()
                .Select(i => new ShopDto
                {
                    Id = i.Id,
                    ShopName = i.Name,
                    ShopUniqueCode = i.UniqueCode,
                    RegisterDate = i.RegisterDate,
                    FullName = authenticatedUser.DisplayName,
                    AvgMonthlyCapacity = i.AvgMonthlyYield,
                    IsVerified = i.IsVerified,
                    AccountId = i.AccountId
                })
                .ToPaginationAsync(request.PageNumber, request.PageSize, cancellationToken);

            var accountIds = pagingResult.Data.Select(i => i.AccountId).ToList();
            var accounts = await accountServices.GetAllUsers()
                .Where(i => accountIds.Contains(i.Id))
                .ToListAsync(cancellationToken);

            var shopIds = pagingResult.Data.Select(i => i.Id);

            var deliveryConfigs = await shopPartnerConfigRepository
                .Where(i => shopIds.Contains(i.ShopId))
                .Select(i => new
                {
                    i.ShopId,
                    i.PartnerConfigId,
                })
                .ToListAsync();

            int index = 0;
            foreach (var item in pagingResult.Data)
            {
                item.No = skipCount + index + 1;
                index++;
                var account = accounts.FirstOrDefault(i => i.Id == item.AccountId);
                if (account != null)
                {
                    item.PhoneNumber = account.PhoneNumber;
                    item.Email = account.Email;
                }

                item.TotalDeliveryConnected = deliveryConfigs.Count(i => i.ShopId == item.Id);
            }

            return BaseResult<PaginationResponseDto<ShopDto>>.Ok(pagingResult);
        }
    }
}
