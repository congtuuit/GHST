using GHSTShipping.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    [Table(nameof(DeliveryPricePlane))]
    public class DeliveryPricePlane : AuditableBaseEntity
    {
        /// <summary>
        /// If the root price plan that value will be empty
        /// </summary>
        public Guid? ShopId { get; set; }
        public Guid? RelatedToDeliveryPricePlaneId { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public long MinWeight { get; set; }

        public long MaxWeight { get; set; }

        public long PublicPrice { get; set; }

        public long PrivatePrice { get; set; }

        /// <summary>
        /// Bước nhảy giá cho mỗi bước nhảy cân nặng
        /// </summary>
        public long StepPrice { get; set; }

        /// <summary>
        /// Bước nhảy cân nặng khi vượt quá max weight
        /// </summary>
        public long StepWeight { get; set; }

        /// <summary>
        /// Nếu giá trị đơn hàng dưới limit sẽ free, ngược lại sẽ được tính theo công thức total * rate
        /// </summary>
        public long LimitInsurance { get; set; }

        [Precision(18, 2)]
        public decimal InsuranceFeeRate { get; set; }

        [Precision(18, 2)]
        public decimal ReturnFeeRate { get; set; }

        /// <summary>
        /// Tỷ lệ chuyển đổi
        /// </summary>
        public decimal ConvertWeightRate { get; set; }

        public bool IsActivated { get; set; }

        #region Partner shop config
        public string ApiKey { get; set; }
        public string ProdEnv { get; set; }
        public Guid PartnerConfigId { get; set; }

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

        #endregion

    }
}
