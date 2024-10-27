using System;

namespace GHSTShipping.Application.DTOs.PartnerConfig
{
    public class ShopConfigDto
    {
        public Guid ShopId { get; set; }

        public Guid PartnerConfigId { get; set; }

        public string PartnerShopId { get; set; }

        public string ClientPhone { get; set; }
    }
}
