using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.DTOs.User;
using GHSTShipping.Application.Extensions;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Parameters;
using GHSTShipping.Application.Wrappers;
using MediatR;
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
        IShopRepository shopRepository
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
                })
                .ToPaginationAsync(request.PageNumber, request.PageSize, cancellationToken);

            int index = 0;
            foreach (var item in pagingResult.Data)
            {
                item.No = skipCount + index + 1;
                index++;
            }

            return BaseResult<PaginationResponseDto<ShopDto>>.Ok(pagingResult);
        }
    }
}
