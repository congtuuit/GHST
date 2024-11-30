using Delivery.GHN.Models;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Queries
{
    public class OrderUtils
    {
        public OrderUtils() { }

        public static async Task<List<ApiConfig>> GetApiConfigsByShopIdAsync(
            IUnitOfWork unitOfWork,
            Guid shopId,
            EnumDeliveryPartner partner)
        {
            var partnerConfig = await unitOfWork.ShopPartnerConfigs
                .Where(i => i.ShopId == shopId && i.PartnerConfig.DeliveryPartner == partner)
                .Select(i => new
                {
                    i.PartnerConfigId,
                    i.PartnerConfig.ProdEnv,
                    i.PartnerConfig.ApiKey,
                })
                .FirstOrDefaultAsync();

            if (partnerConfig != null)
            {
                var shopIds = await unitOfWork.DeliveryPricePlanes
                        .Where(i => i.ShopId == shopId && i.PartnerConfigId == partnerConfig.PartnerConfigId)
                        .Select(i => i.PartnerShopId)
                        .Distinct()
                        .ToListAsync();

                var result = shopIds.Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => new ApiConfig(partnerConfig.ProdEnv, partnerConfig.ApiKey, i)).ToList();

                return result;
            }

            return [];
        }
    }
}
