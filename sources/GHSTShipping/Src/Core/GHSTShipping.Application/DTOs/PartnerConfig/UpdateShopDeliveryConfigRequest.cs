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

        public string? Address { get; set; }

        public string? WardName { get; set; }

        public string? DistrictName { get; set; }

        public string? ProviceName { get; set; }
    }
}
