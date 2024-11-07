using Delivery.GHN.Constants;

namespace Delivery.GHN
{
    public static class ShippingOrderStatusHelper
    {
        private static readonly Dictionary<string, string> VietnameseDescriptions = new Dictionary<string, string>
        {
            { ShippingOrderStatusConstants.ReadyToPick, "Đơn hàng vừa được tạo" },
            { ShippingOrderStatusConstants.Picking, "Shipper đang đến lấy hàng" },
            { ShippingOrderStatusConstants.Cancel, "Đơn hàng đã bị hủy" },
            { ShippingOrderStatusConstants.MoneyCollectPicking, "Shipper đang liên hệ với người bán" },
            { ShippingOrderStatusConstants.Picked, "Shipper đã lấy hàng" },
            { ShippingOrderStatusConstants.Storing, "Hàng đang được lưu kho" },
            { ShippingOrderStatusConstants.Transporting, "Hàng đang được luân chuyển" },
            { ShippingOrderStatusConstants.Sorting, "Hàng đang được phân loại tại kho" },
            { ShippingOrderStatusConstants.Delivering, "Shipper đang giao hàng cho khách" },
            { ShippingOrderStatusConstants.MoneyCollectDelivering, "Shipper đang liên hệ với người mua" },
            { ShippingOrderStatusConstants.Delivered, "Hàng đã được giao cho khách" },
            { ShippingOrderStatusConstants.DeliveryFail, "Không giao được hàng cho khách" },
            { ShippingOrderStatusConstants.WaitingToReturn, "Hàng chờ được hoàn lại" },
            { ShippingOrderStatusConstants.Return, "Hàng chờ hoàn lại sau 3 lần giao không thành công" },
            { ShippingOrderStatusConstants.ReturnTransporting, "Hàng đang được luân chuyển hoàn lại" },
            { ShippingOrderStatusConstants.ReturnSorting, "Hàng đang được phân loại hoàn lại" },
            { ShippingOrderStatusConstants.Returning, "Shipper đang hoàn lại hàng cho người bán" },
            { ShippingOrderStatusConstants.ReturnFail, "Hoàn hàng thất bại" },
            { ShippingOrderStatusConstants.Returned, "Hàng đã được hoàn lại cho người bán" },
            { ShippingOrderStatusConstants.Exception, "Xử lý ngoại lệ hàng hóa" },
            { ShippingOrderStatusConstants.Damage, "Hàng bị hư hỏng" },
            { ShippingOrderStatusConstants.Lost, "Hàng bị thất lạc" }
        };

        public static string GetVietnameseDescription(string status)
        {
            if (VietnameseDescriptions.TryGetValue(status, out string description))
            {
                return description;
            }

            return "Trạng thái không xác định";
        }
    }
}
