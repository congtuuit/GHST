using Delivery.GHN;
using Delivery.GHN.Models;
using GHSTShipping.Application.DTOs.Shop;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
       IAccountServices accountServices,
       IGhnApiClient _ghnApiClient,
       IPartnerConfigService _partnerConfigService
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
                    GhnShopId = i.GhnShopId,
                })
                .FirstOrDefaultAsync(cancellationToken);

            var account = await accountServices.GetAllUsers()
                .Where(i => i.Id == shop.AccountId)
                .FirstOrDefaultAsync(cancellationToken);

            shop.Email = account.Email;
            shop.FullName = account.Name;

            shop.GhnShopDetails = await GetGhnShopDetailDtos(shop.PhoneNumber);

            return BaseResult<ShopViewDetailDto>.Ok(shop);
        }

        private async Task<IEnumerable<GhnShopDetailDto>> GetGhnShopDetailDtos(string phoneNumber)
        {
            try
            {
                var partnerConfig = await _partnerConfigService.GetPartnerConfigAsync(Domain.Enums.EnumDeliveryPartner.GHN);
                var apiConfig = new ApiConfig(partnerConfig.ProdEnv, partnerConfig.ApiKey);
                var response = await _ghnApiClient.GetAllShopsAsync(apiConfig, new GetAllShopsRequest
                {
                    //client_phone = phoneNumber,
                    limit = 20,
                    offset = 1
                });

                if (response.Code == 200)
                {
                    return response.Data.shops.Select(i => new GhnShopDetailDto
                    {
                        Id = i._id,
                        Name = i.name
                    });
                }
            }
            catch (Exception ex)
            {
            }

            return Enumerable.Empty<GhnShopDetailDto>();
        }
    }
}
