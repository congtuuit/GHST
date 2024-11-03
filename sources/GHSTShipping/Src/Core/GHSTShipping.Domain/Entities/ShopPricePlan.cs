using GHSTShipping.Domain.Common;
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
        public int Weight { get; set; }

        [Precision(18, 2)]
        public int Length { get; set; }

        [Precision(18, 2)]
        public int Width { get; set; }

        [Precision(18, 2)]
        public int Height { get; set; }

        public int ConvertedWeight { get; set; }

        public virtual Shop Shop { get; set; }

        public ShopPricePlan() { }

        public ShopPricePlan(Guid shopId, string supplier, decimal privatePrice, decimal officialPrice, int weight, int length, int width, int height)
        {
            ShopId = shopId;
            Supplier = supplier;
            PrivatePrice = privatePrice;
            OfficialPrice = officialPrice;
            Weight = weight;
            Length = length;
            Width = width;
            Height = height;

            this.ConvertedWeight = length * width * height;
        }

        public ShopPricePlan(Guid shopId, string supplier, decimal privatePrice, decimal officialPrice, int weight, int length, int width, int height, int convertedWeight)
        {
            ShopId = shopId;
            Supplier = supplier;
            PrivatePrice = privatePrice;
            OfficialPrice = officialPrice;
            Weight = weight;
            Length = length;
            Width = width;
            Height = height;

            this.ConvertedWeight = convertedWeight;
        }

        public int CalcConvertedWeight(int length, int width, int height)
        {
            return length * width * height;
        }
    }
}
