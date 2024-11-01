using GHSTShipping.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    [Table(nameof(OrderStatusHistory))]
    public class OrderStatusHistory : AuditableBaseEntity
    {
        public Guid OrderId { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        [MaxLength(200)]
        public string ChangedBy { get; set; }

        [MaxLength(200)]
        public string Notes { get; set; }
    }

    /// <summary>
    /// These order status before send to delivery partner
    /// </summary>
    public static class OrderStatus
    {
        public const string CANCEL = "cancel";

        public const string DRAFT = "draft";

        public const string WAITING_CONFIRM = "waiting_confirm";

        public const string READY_TO_PICK = "ready_to_pick";
    }

}
