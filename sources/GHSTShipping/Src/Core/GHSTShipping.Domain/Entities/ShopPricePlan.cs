using GHSTShipping.Domain.Common;
using GHSTShipping.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    [Table(nameof(ShopPricePlan))]
    public class ShopPricePlan : AuditableBaseEntity
    {
        public Guid ShopId { get; set; }

        /// <summary>
        /// GHN | Shopee express | J&T | Best | Viettel | GHTK
        /// </summary>
        [MaxLength(200)]
        public string Supplier { get; set; }

        [Precision(18, 2)]
        public decimal PrivatePrice { get; set; }

        [Precision(18, 2)]
        public decimal OfficialPrice { get; set; }

        [Precision(18, 2)]
        public decimal Capacity { get; set; }

        public virtual Shop Shop { get; set; }

        public ShopPricePlan() { }

        public ShopPricePlan(Guid shopId, string supplier, decimal privatePrice, decimal officialPrice, decimal capacity)
        {
            ShopId = shopId;
            Supplier = supplier;
            PrivatePrice = privatePrice;
            OfficialPrice = officialPrice;
            Capacity = capacity;
        }
    }
}
