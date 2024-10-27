using System;

namespace GHSTShipping.Application.DTOs.PartnerConfig
{
    public class UpdateShopDeliveryConfigRequest
    {
        public Guid ShopId { get; set; }

        public Guid DeliveryConfigId { get; set; }

        public string? PartnerShopId { get; set; }

        public bool IsConnect { get; set; }

        public string? ClientPhone { get; set; }
    }
}
