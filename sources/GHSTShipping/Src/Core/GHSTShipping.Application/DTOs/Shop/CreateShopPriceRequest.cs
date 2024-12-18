﻿using System;

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


        public int Weight { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        /// <summary>
        /// single | mutilple
        /// </summary>
        public string Mode { get; set; }
        public int MaxConvertedWeight { get; set; }
        public int StepWeight { get; set; }
        public int StepPrice { get; set; }
    }
}
