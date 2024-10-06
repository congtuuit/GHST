using GHSTShipping.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    [Table(nameof(Order))]
    public class Order : AuditableBaseEntity
    {
        #region System
        public Guid? ShopId { get; set; }
        public virtual Shop Shop { get; set; }

        public string UniqueCode { get; set; }
        public bool IsPublished { get; set; }
        public string? DeliveryPartner {  get; set; }

        /// <summary>
        /// Delivery fee with price rules
        /// </summary>
        public int DeliveryFee { get; set; }

        public DateTime? PublishDate { get; set; }
        #endregion

        #region GHN
        public string? Note { get; set; }
        public string? ReturnPhone { get; set; }
        public string? ReturnAddress { get; set; }
        public string? ReturnDistrictId { get; set; }
        public string? ReturnWardCode { get; set; }
        public string? ClientOrderCode { get; set; }

        public int PaymentTypeId { get; set; }

        public string RequiredNote { get; set; } = null!;
        public string FromName { get; set; } = null!;
        public string FromPhone { get; set; } = null!;
        public string FromAddress { get; set; } = null!;
        public string FromWardName { get; set; } = null!;
        public string FromDistrictName { get; set; } = null!;
        public string FromProvinceName { get; set; } = null!;
        public string ToName { get; set; } = null!;
        public string ToPhone { get; set; } = null!;
        public string ToAddress { get; set; } = null!;
        public string ToWardCode { get; set; } = null!;
        public string ToDistrictId { get; set; } = null!;

        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public int CodAmount { get; set; }
        public string? Content { get; set; }

        public int? PickStationId { get; set; }
        public int? DeliverStationId { get; set; }

        public int InsuranceValue { get; set; }
        public int ServiceId { get; set; } = 0;
        public int ServiceTypeId { get; set; } = 2;

        public string? Coupon { get; set; }
        public List<int> PickShift { get; set; } = new List<int>();
        #endregion

        #region GHN response

        public string private_order_code { get; set; }
        public string private_sort_code { get; set; }
        public string private_trans_type { get; set; }
        public int private_total_fee { get; set; }
        public DateTime private_expected_delivery_time { get; set; }
        public string private_operation_partner { get; set; }

        #endregion
    }
}
