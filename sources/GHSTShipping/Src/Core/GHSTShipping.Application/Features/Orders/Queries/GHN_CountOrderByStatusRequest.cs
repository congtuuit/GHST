using Delivery.GHN;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace GHSTShipping.Application.Features.Orders.Queries
{
    public class GHN_CountOrderByStatusRequest : IRequest<string>
    {
        public Guid? ShopId { get; set; }
    }

    public class GHN_CountOrderByStatusRequestHandler(
        IGhnApiClient ghnApiClient,
        IAuthenticatedUserService authenticatedUserService,
        IPartnerConfigService partnerConfigService,
        IShopRepository shopRepository,
        IShopPartnerConfigRepository shopPartnerConfigRepository
        ) : IRequestHandler<GHN_CountOrderByStatusRequest, string>
    {
        public async Task<string> Handle(GHN_CountOrderByStatusRequest request, CancellationToken cancellationToken)
        {
            var shop = await shopRepository
                .Where(i => i.Id == request.ShopId)
                .Select(i => new
                {
                    ShopId = i.Id,
                })
                .FirstOrDefaultAsync(cancellationToken);

            // Fetch API config for the delivery partner (GHN)
            var apiConfig = await partnerConfigService.GetApiConfigAsync(Domain.Enums.EnumDeliveryPartner.GHN, shop.ShopId);

            var response = await ghnApiClient.CountOrderByStatusAsync(apiConfig);

            return response;
        }

        internal class DeliveryConfigDto
        {
            public Guid ShopId { get; set; }
            public Guid PartnerConfigId { get; set; }
            public string PartnerShopId { get; set; }

            public PartnerConfigDto PartnerConfig { get; set; }
        }

        private async Task<DeliveryConfigDto> GetDeliveryConfigAsync(Guid shopId, CancellationToken cancellationToken)
        {
            var result = await shopPartnerConfigRepository
               .Where(i => i.ShopId == shopId && i.PartnerConfig.IsActivated)
               .Select(i => new DeliveryConfigDto
               {
                   ShopId = i.ShopId,
                   PartnerConfigId = i.PartnerConfigId,
                   PartnerShopId = i.PartnerShopId,
                   PartnerConfig = new PartnerConfigDto
                   {
                       ApiKey = i.PartnerConfig.ApiKey,
                       ProdEnv = i.PartnerConfig.ProdEnv,
                       DeliveryPartner = i.PartnerConfig.DeliveryPartner
                   }
               })
               .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
