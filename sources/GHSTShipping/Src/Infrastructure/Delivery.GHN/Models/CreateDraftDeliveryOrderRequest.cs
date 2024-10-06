namespace Delivery.GHN.Models
{
    public class CreateDraftDeliveryOrderRequest
    {
        public string? note { get; set; }
        public string? return_phone { get; set; }
        public string? return_address { get; set; }
        public string? return_district_id { get; set; }
        public string? return_ward_code { get; set; }

        public string? client_order_code { get; set; }

        #region Required
        public int payment_type_id { get; set; }

        public required string required_note { get; set; }

        public required string from_name { get; set; }
        public required string from_phone { get; set; }
        public required string from_address { get; set; }
        public required string from_ward_name { get; set; }
        public required string from_district_name { get; set; }
        public required string from_province_name { get; set; }

        /// <summary>
        /// [Required] Client name. (Customer / Buyer)
        /// </summary>
        public required string to_name { get; set; }
        public required string to_phone { get; set; }
        public required string to_address { get; set; }
        public required string to_ward_code { get; set; }
        public required string to_district_id { get; set; }

        public int weight { get; set; }
        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public List<DeliveryOrderItemDto> items { get; set; }
        #endregion

        /// <summary>
        /// Tiền thu hộ cho người gửi.
        /// Maximum :50.000.000
        /// Giá trị mặc định: 0
        /// </summary>
        public int cod_amount { get; set; }
        public string? content { get; set; }

        public int? pick_station_id { get; set; }
        public int? deliver_station_id { get; set; }

        /// <summary>
        /// Use to declare parcel value. GHN will base on this value for compensation if any unexpected things happen (lost, broken...).
        /// Giá trị của đơn hàng ( Trường hợp mất hàng , bể hàng sẽ đền theo giá trị của đơn hàng).
        /// Tối đa 5.000.000
        /// Giá trị mặc định: 0
        /// </summary>
        public int insurance_value { get; set; }
        public int service_id { get; set; } = 0;
        public int service_type_id { get; set; } = 2;

        public string? coupon { get; set; }
        public List<int> pick_shift { get; set; }
    }

    public class DeliveryOrderItemDto
    {
        public string name { get; set; }
        public string code { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public OrderCategoryDto? category { get; set; }
    }

    public class OrderCategoryDto
    {
        public string? level1 { get; set; }
    }
}
