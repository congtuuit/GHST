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
        /// <summary>
        /// The shop ID in delivery has been connected 
        /// </summary>
        public string PartnerShopId { get; set; }

        /// <summary>
        /// System customer shop id
        /// </summary>
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

        /// <summary>
        /// Delivery fee has been orverried and just display on Admin
        /// </summary>
        public int CustomDeliveryFee { get; private set; }

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
        public string? ReturnDistrictName { get; set; }

        [MaxLength(100)]
        public string? ReturnWardCode { get; set; }

        [MaxLength(100)]
        public string? ReturnWardName { get; set; }

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
        public string private_sort_code { get; private  set; }

        [MaxLength(100)]
        public string private_trans_type { get; private  set; }

        public int private_total_fee { get; private  set; }

        public DateTime? private_expected_delivery_time { get; private  set; }

        [MaxLength(100)]
        public string private_operation_partner { get; private  set; }

        #endregion


        [MaxLength(100)]
        public string CurrentStatus { get; set; }


        public void GenerateOrderCode(long sequenceCode, string prefix)
        {
            int length = 5;
            string sequenceCodeFormatted =  $"{sequenceCode}".PadLeft(length, '0');
            string uniqueCode = prefix + sequenceCodeFormatted;

            this.UniqueCode = uniqueCode;
            this.ClientOrderCode = uniqueCode;
        }

        public void Publish()
        {
            this.PublishDate = DateTime.UtcNow;
            this.IsPublished = true;
            this.CurrentStatus = OrderStatus.READY_TO_PICK;
        }

        public void Cancel()
        {
            this.CurrentStatus = OrderStatus.CANCEL;
        }

        public void OrrverideDeliveryFee(int fee)
        {
            this.CustomDeliveryFee = fee;
        }

        public void PrivateUpdateFromPartner(
            string private_order_code,
            string private_sort_code,
            string private_trans_type,
            int private_total_fee,
            DateTime private_expected_delivery_time,
            string private_operation_partner
            )
        {
            this.private_order_code = private_order_code;
            this.private_sort_code = private_sort_code;
            this.private_trans_type = private_trans_type;
            this.private_total_fee = private_total_fee;
            this.private_expected_delivery_time = private_expected_delivery_time;
            this.private_operation_partner = private_operation_partner;
        }
    }
}
