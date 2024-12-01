using GHSTShipping.Application.DTOs.Shop;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Enums;
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
                    AllowUsePartnerShopAddress = i.AllowUsePartnerShopAddress,
                })
                .FirstOrDefaultAsync(cancellationToken);

            var account = await accountServices.GetAllUsers()
                .Where(i => i.Id == shop.AccountId)
                .FirstOrDefaultAsync(cancellationToken);

            shop.Email = account.Email;
            shop.FullName = account.Name;

            var parterConfigs = await _partnerConfigService.GetPartnerConfigsAsync(true);
            if (parterConfigs.Any())
            {
                shop.Partners = parterConfigs.Select(i => new DTOs.PartnerConfig.ShopConfigViewDto
                {
                    PartnerConfigId = i.Id,
                    PartnerName = i.DeliveryPartnerName,
                    PartnerAccountName = i.FullName
                })
                .ToList();

                Guid? ghnParterConfigId = null;
                // Fetch ghn shop by configs
                /// Lấy thông tin các shop con theo token của GHN
                var ghnShopDetails = new Dictionary<Guid, IEnumerable<GhnShopDetailDto>>();
                foreach (var parterConfig in parterConfigs)
                {
                    if (parterConfig.DeliveryPartner == Domain.Enums.EnumDeliveryPartner.GHN)
                    {
                        var _ghnShopDetails = await _partnerConfigService.GetGhnShopDetailDtos(new PartnerConfigDto
                        {
                            ProdEnv = parterConfig.ProdEnv,
                            ApiKey = parterConfig.ApiKey,
                        });

                        if (_ghnShopDetails.Any())
                        {
                            // Mapping config and shops
                            ghnShopDetails.Add(parterConfig.Id, _ghnShopDetails);
                        }
                    }
                }

                shop.GhnShopDetails = ghnShopDetails;

                var deliveryConfigs = await _partnerConfigService.GetShopConfigsAsync(shop.Id);
                shop.ShopConfigs = deliveryConfigs.ToList();

                // Get current GHN shop Id has been connected
                /*var ghnConfig = await _partnerConfigService.GetApiConfigAsync(EnumDeliveryPartner.GHN, shop.Id);
                if (ghnConfig != null)
                {
                    shop.CurrentGhnShopId = ghnConfig.ShopId;
                }*/
            }

            return BaseResult<ShopViewDetailDto>.Ok(shop);
        }
    }
}
