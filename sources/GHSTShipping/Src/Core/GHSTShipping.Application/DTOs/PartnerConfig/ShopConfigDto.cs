using GHSTShipping.Domain.Enums;
using System;

namespace GHSTShipping.Application.DTOs.PartnerConfig
{
    public class ShopConfigDto
    {
        public Guid ShopId { get; set; }

        public Guid PartnerConfigId { get; set; }

        public EnumDeliveryPartner? DeliveryPartner { get; set; }

        public string PartnerShopId { get; set; }

        public string ClientPhone { get; set; }

        public string? Address { get; set; }

        public string? WardName { get; set; }

        public string? DistrictName { get; set; }

        public string? ProvinceName { get; set; }
    }
}
