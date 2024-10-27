using GHSTShipping.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    /// <summary>
    /// Connection between store and delivery partner
    /// </summary>
    [Table(nameof(ShopPartnerConfig))]
    public class ShopPartnerConfig : AuditableBaseEntity
    {
        public Guid ShopId { get; set; }

        public virtual Shop Shop { get; set; }

        public Guid PartnerConfigId { get; set; }

        public virtual PartnerConfig PartnerConfig { get; set; }

        public string? PartnerShopId { get; set; }

        public string? ClientPhone { get; set; }

        public string? Address { get; set; }

        public string? WardName { get; set; }

        public string? DistrictName { get; set; }

        public string? ProviceName { get; set; }
    }
}
