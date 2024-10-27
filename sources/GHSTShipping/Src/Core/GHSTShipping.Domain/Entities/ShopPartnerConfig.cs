using GHSTShipping.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    [Table(nameof(ShopPartnerConfig))]
    public class ShopPartnerConfig : AuditableBaseEntity
    {
        public Guid ShopId { get; set; }

        public virtual Shop Shop { get; set; }

        public Guid PartnerConfigId { get; set; }

        public virtual PartnerConfig PartnerConfig { get; set; }

        public string? PartnerShopId { get; set; }

        public string? ClientPhone { get; set; }
    }
}
