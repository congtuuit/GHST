using System;

namespace GHSTShipping.Application.DTOs.Shop
{
    public class CreateShopPriceRequest
    {
        public Guid? Id { get; set; }

        public Guid ShopId { get; set; }

        /// <summary>
        /// GHN | Shopee express | J&T | Best | Viettel | GHTK
        /// </summary>
        public string Supplier { get; set; }

        public decimal PrivatePrice { get; set; }

        public decimal OfficialPrice { get; set; }

        public decimal Capacity { get; set; }
    }
}
