using System;

namespace GHSTShipping.Application.DTOs.Orders
{
    public class OrderShippingCostDto
    {
        public Guid ShopDeliveryPricePlaneId { get; set; }

        /// <summary>
        /// Khối lượng tính phí
        /// </summary>
        public decimal OrderWeight { get; set; } // Khối lượng tính toán cuối cùng (gram)

        /// <summary>
        /// Khối lượng chuyển đổi
        /// </summary>
        public decimal CalcOrderWeight { get; set; } // Khối lượng tính toán cuối cùng (gram)
        public long ShippingCost { get; set; } // Chi phí giao hàng (VND)

        public long InsuranceFee { get; set; } // Phí bảo hiểm đơn hàng
    }
}
