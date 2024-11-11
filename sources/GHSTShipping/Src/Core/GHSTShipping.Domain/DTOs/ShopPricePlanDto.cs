using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace GHSTShipping.Domain.DTOs
{
    public class ShopPricePlanDto
    {

        public int No { get; set; }

        public Guid Id { get; set; }

        public Guid ShopId { get; set; }

        public string ShopName { get; set; }

        public string ShopUniqueCode { get; set; }

        /// <summary>
        /// GHN | Shopee express | J&T | Best | Viettel | GHTK
        /// </summary>
        [MaxLength(200)]
        public string Supplier { get; set; }

        [Precision(18, 2)]
        public decimal PrivatePrice { get; set; }

        [Precision(18, 2)]
        public decimal OfficialPrice { get; set; }

        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int ConvertedWeight { get; set; }

        private int _convertRate;

        public int ConvertRate
        {
            get
            {
                return _convertRate == 0 ? 1 : _convertRate;
            }
            set
            {
                _convertRate = value;
            }
        }
    }
}
