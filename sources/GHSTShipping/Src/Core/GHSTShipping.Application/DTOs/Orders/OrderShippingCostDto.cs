using System;

namespace GHSTShipping.Application.DTOs.Orders
{
    public class OrderShippingCostDto
    {
        public Guid ShopDeliveryPricePlaneId { get; set; }
        public decimal OrderWeight { get; set; } // Khối lượng tính toán cuối cùng (gram)
        public long ShippingCost { get; set; } // Chi phí giao hàng (VND)
    }
}
