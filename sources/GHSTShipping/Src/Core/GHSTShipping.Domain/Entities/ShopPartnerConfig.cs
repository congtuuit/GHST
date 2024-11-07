using GHSTShipping.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
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

        [MaxLength(100)]
        public string? PartnerShopId { get; set; }

        [MaxLength(100)]
        public string? ClientPhone { get; set; }

        [MaxLength(500)]

        public string? Address { get; set; }

        [MaxLength(100)]
        public string? WardName { get; set; }

        [MaxLength(100)]
        public string? WardCode { get; set; }

        [MaxLength(100)]
        public string? DistrictName { get; set; }

        [MaxLength(100)]
        public string? DistrictId { get; set; }

        [MaxLength(100)]
        public string? ProvinceName { get; set; }

        [MaxLength(100)]
        public string? ProvinceId { get; set; }

        [MaxLength(100)]
        public string ShopName { get; set; }

    }
}
