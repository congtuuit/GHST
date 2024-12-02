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

        /// <summary>
        /// Nếu đơn hàng bị third trả lỗi
        /// </summary>
        public const string RESEND_THIRDPARTY = "resend_thirdparty";

        // Mapping to GHN_OrderStatus
        public const string READY_TO_PICK = "ready_to_pick";
        public const string PICKING = "picking";
        public const string MONEY_COLLECT_PICKING = "money_collect_picking";
        public const string PICKED = "picked";
        public const string SORTING = "sorting";
        public const string STORING = "storing";
        public const string TRANSPORTING = "transporting";
        public const string DELIVERING = "delivering";
        public const string DELIVERY_FAIL = "delivery_fail";
        public const string MONEY_COLLECT_DELIVERING = "money_collect_delivering";
        public const string RETURN = "return";
        public const string RETURNING = "returning";
        public const string RETURN_FAIL = "return_fail";
        public const string RETURN_TRANSPORTING = "return_transporting";
        public const string RETURN_SORTING = "return_sorting";
        public const string WAITING_TO_RETURN = "waiting_to_return";
        public const string RETURNED = "returned";
        public const string DELIVERED = "delivered";
        public const string LOST = "lost";
        public const string DAMAGE = "damage";

        public static string GetStatusColor(string status)
        {
            return status switch
            {
                READY_TO_PICK => OrderStatusColors.READY_TO_PICK,
                PICKING => OrderStatusColors.PICKING,
                MONEY_COLLECT_PICKING => OrderStatusColors.MONEY_COLLECT_PICKING,
                PICKED => OrderStatusColors.PICKED,
                SORTING => OrderStatusColors.SORTING,
                STORING => OrderStatusColors.STORING,
                TRANSPORTING => OrderStatusColors.TRANSPORTING,
                DELIVERING => OrderStatusColors.DELIVERING,
                DELIVERY_FAIL => OrderStatusColors.DELIVERY_FAIL,
                MONEY_COLLECT_DELIVERING => OrderStatusColors.MONEY_COLLECT_DELIVERING,
                RETURN => OrderStatusColors.RETURN,
                RETURNING => OrderStatusColors.RETURNING,
                RETURN_FAIL => OrderStatusColors.RETURN_FAIL,
                RETURN_TRANSPORTING => OrderStatusColors.RETURN_TRANSPORTING,
                RETURN_SORTING => OrderStatusColors.RETURN_SORTING,
                WAITING_TO_RETURN => OrderStatusColors.WAITING_TO_RETURN,
                RETURNED => OrderStatusColors.RETURNED,
                DELIVERED => OrderStatusColors.DELIVERED,
                CANCEL => OrderStatusColors.CANCEL,
                LOST => OrderStatusColors.LOST,
                DAMAGE => OrderStatusColors.DAMAGE,
                _ => "#000000" // Default to black if unknown status
            };
        }

        public static string GetStatusTranslation(string status)
        {
            return status switch
            {
                READY_TO_PICK => "Sẵn sàng để lấy hàng",
                PICKING => "Đang lấy hàng",
                MONEY_COLLECT_PICKING => "Đang thu tiền và lấy hàng",
                PICKED => "Đã lấy hàng",
                SORTING => "Đang phân loại",
                STORING => "Đang lưu kho",
                TRANSPORTING => "Đang vận chuyển",
                DELIVERING => "Đang giao hàng",
                DELIVERY_FAIL => "Giao hàng thất bại",
                MONEY_COLLECT_DELIVERING => "Đang giao hàng và thu tiền",
                RETURN => "Đang trả hàng",
                RETURNING => "Đang hoàn hàng",
                RETURN_FAIL => "Trả hàng thất bại",
                RETURN_TRANSPORTING => "Đang vận chuyển hàng trả",
                RETURN_SORTING => "Đang phân loại hàng trả",
                WAITING_TO_RETURN => "Đang chờ trả hàng",
                RETURNED => "Đã trả hàng",
                DELIVERED => "Đã giao hàng",
                CANCEL => "Đã hủy",
                LOST => "Đã mất",
                DAMAGE => "Hư hỏng",
                _ => "Không xác định" // Default if status is unknown
            };
        }

    }

    public static class OrderStatusColors
    {
        public const string READY_TO_PICK = "#4CAF50"; // Green
        public const string PICKING = "#8BC34A"; // Light Green
        public const string MONEY_COLLECT_PICKING = "#FF9800"; // Orange
        public const string PICKED = "#009688"; // Teal
        public const string SORTING = "#3F51B5"; // Indigo
        public const string STORING = "#9C27B0"; // Purple
        public const string TRANSPORTING = "#673AB7"; // Deep Purple
        public const string DELIVERING = "#03A9F4"; // Light Blue
        public const string DELIVERY_FAIL = "#FF5722"; // Deep Orange
        public const string MONEY_COLLECT_DELIVERING = "#FFC107"; // Amber
        public const string RETURN = "#607D8B"; // Blue Grey
        public const string RETURNING = "#795548"; // Brown
        public const string RETURN_FAIL = "#FF5252"; // Red (lighter for visibility)
        public const string RETURN_TRANSPORTING = "#00BCD4"; // Cyan
        public const string RETURN_SORTING = "#E91E63"; // Pink
        public const string WAITING_TO_RETURN = "#9E9E9E"; // Grey
        public const string RETURNED = "#8E24AA"; // Dark Purple
        public const string DELIVERED = "#4CAF50"; // Green (same as Ready to Pick)
        public const string CANCEL = "#F44336"; // Red
        public const string LOST = "#D32F2F"; // Dark Red
        public const string DAMAGE = "#FF1744"; // Bright Red
    }
}
