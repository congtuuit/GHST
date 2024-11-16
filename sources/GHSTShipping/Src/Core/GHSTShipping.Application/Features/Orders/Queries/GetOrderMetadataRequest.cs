using GHSTShipping.Application.DTOs.Shop;
using GHSTShipping.Application.Features.Configs.Queries;
using GHSTShipping.Application.Features.Shops.Queries;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Queries
{
    /// <summary>
    /// NOTE: Change this class will be impacted the create order feature
    /// </summary>
    public class GetOrderMetadataRequest : IRequest<BaseResult<GetOrderMetadataResponse>>
    {
    }

    public class GetOrderMetadataResponse
    {
        public List<DeliveryConfigDto> DeliveryConfigs { get; set; }
        public List<ShopDeliveryPricePlaneDto> DeliveryPricePlanes { get; set; }

        public List<BasicShopInfoDto> MyShops { get; set; }

        public class DeliveryConfigDto
        {
            public Guid DeliveryConfigId { get; set; }

            public EnumDeliveryPartner DeliveryPartner { get; set; }

            public string DeliveryPartnerName
            {
                get
                {
                    return DeliveryPartner.GetCode();
                }
            }

            public List<GhnShopDetailDto> Shops { get; set; }
        }
    }

    public class GetOrderMetadataRequestHandler : IRequestHandler<GetOrderMetadataRequest, BaseResult<GetOrderMetadataResponse>>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IShopRepository _shopRepository;
        private readonly IShopPartnerConfigRepository _shopPartnerConfigRepository;
        private readonly IPartnerConfigRepository _partnerConfigRepository;
        private readonly IPartnerConfigService _partnerConfigService;
        private readonly IMediator _mediator;

        public GetOrderMetadataRequestHandler(
            IAuthenticatedUserService authenticatedUser,
            IShopRepository shopRepository,
            IShopPartnerConfigRepository shopPartnerConfigRepository,
            IPartnerConfigRepository partnerConfigRepository,
            IPartnerConfigService partnerConfigService,
            IMediator mediator)
        {
            _authenticatedUserService = authenticatedUser;
            _shopRepository = shopRepository;
            _shopPartnerConfigRepository = shopPartnerConfigRepository;
            _partnerConfigRepository = partnerConfigRepository;
            _partnerConfigService = partnerConfigService;
            _mediator = mediator;
        }

        public async Task<BaseResult<GetOrderMetadataResponse>> Handle(GetOrderMetadataRequest request, CancellationToken cancellationToken)
        {
            Guid userId = _authenticatedUserService.UId;
            var shop = await _shopRepository.Where(i => i.AccountId == userId)
                 .Select(i => new
                 {
                     i.Id,
                 })
                .FirstOrDefaultAsync(cancellationToken);

            // Get shop delivery configs
            var deliveryConfigs = _shopPartnerConfigRepository.Where(i => i.ShopId == shop.Id).Select(i => new
            {
                i.ShopId,
                i.PartnerConfigId,
                i.PartnerShopId,
                i.ClientPhone,
                PartnerConfig = new
                {
                    i.PartnerConfig.ApiKey,
                    i.PartnerConfig.ProdEnv,
                    i.PartnerConfig.DeliveryPartner
                }
            });

            var results = new List<GetOrderMetadataResponse.DeliveryConfigDto>();

            // Fetch ghn shop by configs
            var ghnShopDetails = new List<GhnShopDetailDto>();
            foreach (var deliveryConfig in deliveryConfigs)
            {
                var deliveryConfigDto = new GetOrderMetadataResponse.DeliveryConfigDto()
                {
                    DeliveryConfigId = deliveryConfig.PartnerConfigId,
                    DeliveryPartner = deliveryConfig.PartnerConfig.DeliveryPartner,
                    Shops = new List<GhnShopDetailDto>()
                };

                if (deliveryConfig.PartnerConfig.DeliveryPartner == Domain.Enums.EnumDeliveryPartner.GHN)
                {
                    var shopDetail = await _shopPartnerConfigRepository.Where(i => i.ShopId == deliveryConfig.ShopId && i.PartnerConfigId == deliveryConfig.PartnerConfigId)
                        .Select(i => new GhnShopDetailDto
                        {
                            Id = i.PartnerShopId,
                            Phone = i.ClientPhone,
                            Name = i.ShopName,

                            Address = i.Address,
                            WardId = i.WardCode,
                            WardName = i.WardName,
                            DistrictId = i.DistrictId,
                            DistrictName = i.DistrictName,
                            ProvinceId = i.ProvinceId,
                            ProvinceName = i.ProvinceName,
                        })
                        .FirstOrDefaultAsync();

                    if (shopDetail != null)
                    {
                        deliveryConfigDto.Shops.Add(shopDetail);
                    }
                }

                results.Add(deliveryConfigDto);
            }

            var deliveryPricePlanes = await _mediator.Send(new GetShopDeliveryPricePlanesRequest
            {
                ShopId = shop.Id,
                //IsActivated = true, // TODO allow enable or disable delivery config price
            });

            var myShops = await _mediator.Send(new GetShopByIdQuery { Id = shop.Id }, cancellationToken);

            var response = new GetOrderMetadataResponse()
            {
                DeliveryConfigs = results,
                DeliveryPricePlanes = deliveryPricePlanes.Data,
                MyShops = myShops
            };

            return BaseResult<GetOrderMetadataResponse>.Ok(response);
        }
    }
}
