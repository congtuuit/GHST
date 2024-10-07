using GHSTShipping.Application.DTOs.Shop;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Shops.Queries
{
    public class GetShopDetailRequest : IRequest<BaseResult<ShopViewDetailDto>>
    {
        public Guid? ShopId { get; set; }
    }

    public class GetShopDetailRequestHandler(
       IUnitOfWork unitOfWork,
       IShopRepository shopRepository,
       IAccountServices accountServices
       ) : IRequestHandler<GetShopDetailRequest, BaseResult<ShopViewDetailDto>>
    {
        public async Task<BaseResult<ShopViewDetailDto>> Handle(GetShopDetailRequest request, CancellationToken cancellationToken)
        {
            var shop = await shopRepository.Where(i => i.Id == request.ShopId)
                .Select(i => new ShopViewDetailDto()
                {
                    Id = i.Id,
                    ShopUniqueCode = i.UniqueCode,
                    RegisterDate = i.RegisterDate,
                    ShopName = i.Name,
                    AvgMonthlyCapacity = i.AvgMonthlyYield,
                    IsVerified = i.IsVerified,
                    AccountId = i.AccountId,
                    PhoneNumber = i.PhoneNumber,
                    BankName = i.BankName,
                    BankAccountHolder = i.BankAccountHolder,
                    BankAccountNumber = i.BankAccountNumber,
                    BankAddress = i.BankAddress,
                    AllowPublishOrder = i.AllowPublishOrder,
                })
                .FirstOrDefaultAsync(cancellationToken);

            var account = await accountServices.GetAllUsers()
                .Where(i => i.Id == shop.AccountId)
                .FirstOrDefaultAsync(cancellationToken);

            shop.Email = account.Email;
            shop.FullName = account.Name;

            return BaseResult<ShopViewDetailDto>.Ok(shop);
        }
    }
}
