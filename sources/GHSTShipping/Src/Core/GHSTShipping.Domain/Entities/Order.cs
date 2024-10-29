using GHSTShipping.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    [Table(nameof(Order))]
    public class Order : AuditableBaseEntity
    {
        #region System
        public Guid? ShopId { get; set; }
        public virtual Shop Shop { get; set; }

        [MaxLength(200)]
        public string UniqueCode { get; set; }
        public bool IsPublished { get; set; }

        [MaxLength(100)]
        public string? DeliveryPartner {  get; set; }

        /// <summary>
        /// Delivery fee with price rules
        /// </summary>
        public int DeliveryFee { get; set; }

        public DateTime? PublishDate { get; set; }
        #endregion

        #region GHN
        [MaxLength(500)]
        public string? Note { get; set; }

        [MaxLength(100)]
        public string? ReturnPhone { get; set; }

        [MaxLength(500)]
        public string? ReturnAddress { get; set; }

        [MaxLength(100)]
        public string? ReturnDistrictId { get; set; }

        [MaxLength(100)]
        public string? ReturnWardCode { get; set; }

        [MaxLength(200)]
        public string? ClientOrderCode { get; set; }

        public int PaymentTypeId { get; set; }

        [MaxLength(500)]
        public string RequiredNote { get; set; } = null!;

        [MaxLength(100)]
        public string FromName { get; set; } = null!;

        [MaxLength(100)]
        public string FromPhone { get; set; } = null!;

        [MaxLength(500)]
        public string FromAddress { get; set; } = null!;

        [MaxLength(100)]
        public string FromWardName { get; set; } = null!;

        public int? FromWardId { get; set; }

        [MaxLength(100)]
        public string FromDistrictName { get; set; } = null!;

        public int? FromDistrictId { get; set; }

        [MaxLength(100)]
        public string FromProvinceName { get; set; } = null!;

        public int? FromProvinceId { get; set; }

        [MaxLength(100)]
        public string ToName { get; set; } = null!;

        [MaxLength(100)]
        public string ToPhone { get; set; } = null!;

        [MaxLength(500)]
        public string ToAddress { get; set; } = null!;

        [MaxLength(100)]
        public string ToWardName { get; set; } = null!;

        public int? ToWardId { get; set; }

        [MaxLength(100)]
        public string ToDistrictName { get; set; } = null!;

        public int? ToDistrictId { get; set; }

        [MaxLength(100)]
        public string ToProvinceName { get; set; } = null!;

        public int? ToProvinceId { get; set; } = null;

        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public int CodAmount { get; set; }

        [MaxLength(4000)]
        public string? Content { get; set; }

        public int? PickStationId { get; set; }
        public int? DeliverStationId { get; set; }

        public int InsuranceValue { get; set; }
        public int ServiceId { get; set; } = 0;
        public int ServiceTypeId { get; set; } = 2;

        [MaxLength(100)]
        public string? Coupon { get; set; }
        public List<int> PickShift { get; set; } = new List<int>();
        #endregion

        #region GHN response

        [MaxLength(100)]
        public string private_order_code { get; set; }

        [MaxLength(100)]
        public string private_sort_code { get; set; }

        [MaxLength(100)]
        public string private_trans_type { get; set; }

        public int private_total_fee { get; set; }
        public DateTime private_expected_delivery_time { get; set; }

        [MaxLength(100)]
        public string private_operation_partner { get; set; }

        #endregion

        public void GenerateOrderCode(long sequenceCode, string prefix)
        {
            int length = 5;
            string sequenceCodeFormatted =  $"{sequenceCode}".PadLeft(length, '0');
            string uniqueCode = prefix + sequenceCodeFormatted;

            this.UniqueCode = uniqueCode;
            this.ClientOrderCode = uniqueCode;
        }

    }
}
