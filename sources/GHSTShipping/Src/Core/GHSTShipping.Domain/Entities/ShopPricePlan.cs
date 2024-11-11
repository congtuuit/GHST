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
        public long PrivatePrice { get; set; }

        [Precision(18, 2)]
        public long OfficialPrice { get; set; }

        [Precision(18, 2)]
        public int Weight { get; set; }

        [Precision(18, 2)]
        public int Length { get; set; }

        [Precision(18, 2)]
        public int Width { get; set; }

        [Precision(18, 2)]
        public int Height { get; set; }

        public int ConvertedWeight { get; set; }

        public int ConvertRate { get; set; } = 1;

        public virtual Shop Shop { get; set; }

        public ShopPricePlan() { }

        /// <summary>
        /// Auto calc CalcConvertedWeight
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="supplier"></param>
        /// <param name="privatePrice"></param>
        /// <param name="officialPrice"></param>
        /// <param name="weight"></param>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="convertedRate"></param>
        public ShopPricePlan(
            Guid shopId,
            string supplier,
            long privatePrice,
            long officialPrice,
            int weight,
            int length,
            int width,
            int height,
            int convertedRate)
        {
            ShopId = shopId;
            Supplier = supplier;
            PrivatePrice = privatePrice;
            OfficialPrice = officialPrice;
            Weight = weight;
            Length = length;
            Width = width;
            Height = height;
            this.ConvertRate = convertedRate;

            this.CalcConvertedWeight();
        }

        public ShopPricePlan(
            Guid shopId,
            string supplier,
            long privatePrice,
            long officialPrice,
            int weight,
            int length,
            int width,
            int height,
            int convertedRate,
            int convertedWeight)
        {
            ShopId = shopId;
            Supplier = supplier;
            PrivatePrice = privatePrice;
            OfficialPrice = officialPrice;
            Weight = weight;
            Length = length;
            Width = width;
            Height = height;
            ConvertRate = convertedRate;

            this.ConvertedWeight = convertedWeight;
        }

        public int CalcConvertedWeight()
        {
            this.ConvertedWeight = (this.Length * this.Width * this.Height) / this.ConvertRate;
            return this.ConvertedWeight;
        }

        /// <summary>
        /// Công thức tính, áp dụng cho ghi đè
        /// </summary>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public int CalcConvertedWeight(int length, int width, int height, int rate)
        {
            return (length * width * height) / rate;
        }
    }
}
