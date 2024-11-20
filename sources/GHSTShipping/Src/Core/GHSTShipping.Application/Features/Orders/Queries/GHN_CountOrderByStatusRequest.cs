using Delivery.GHN;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        IOrderRepository orderRepository,
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
            // var apiConfig = await partnerConfigService.GetApiConfigAsync(Domain.Enums.EnumDeliveryPartner.GHN, shop.ShopId);
            // var response = await ghnApiClient.CountOrderByStatusAsync(apiConfig);

            var defaultStatuses = new Dictionary<string, int>
            {
                { "cancel", 0 },
                { "damage", 0 },
                { "delivered", 0 },
                { "delivering", 0 },
                { "delivery_fail", 0 },
                { "draft", 0 },
                { "draft_cancel", 0 },
                { "exception", 0 },
                { "lost", 0 },
                { "money_collect_delivering", 0 },
                { "money_collect_picking", 0 },
                { "picked", 0 },
                { "picking", 0 },
                { "ready_to_pick", 0 },
                { "return", 0 },
                { "return_fail", 0 },
                { "return_sorting", 0 },
                { "return_transporting", 0 },
                { "returned", 0 },
                { "returning", 0 },
                { "sorting", 0 },
                { "storing", 0 },
                { "transporting", 0 },
                { "waiting_to_return", 0 }
            };

            var orderStatusCounts = await orderRepository
                .Where(o => o.ShopId == shop.ShopId)
                .GroupBy(o => o.CurrentStatus)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            foreach (var statusCount in orderStatusCounts)
            {
                if (defaultStatuses.ContainsKey(statusCount.Status))
                {
                    defaultStatuses[statusCount.Status] = statusCount.Count;
                }
            }

            var responseData = new
            {
                code = 200,
                message = "Success",
                data = defaultStatuses
            };

            var data = JsonConvert.SerializeObject(responseData);

            return data;
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
