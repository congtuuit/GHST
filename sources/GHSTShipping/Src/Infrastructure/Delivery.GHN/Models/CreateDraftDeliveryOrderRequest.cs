using System.Text.Json.Serialization;

namespace Delivery.GHN.Models
{
    public class CreateDraftDeliveryOrderRequest
    {
        public int payment_type_id { get; set; }
        public string? note { get; set; }
        public required string required_note { get; set; }
        public string? return_phone { get; set; }
        public string? return_address { get; set; }
        public string? return_district_id { get; set; }
        public string? return_ward_code { get; set; }
        public string? client_order_code { get; set; }
        public required string to_name { get; set; }
        public required string to_phone { get; set; }
        public required string to_address { get; set; }
        public required string to_ward_code { get; set; }
        public int to_district_id { get; set; }
        public int cod_amount { get; set; }
        public string? content { get; set; }
        public int weight { get; set; }
        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int pick_station_id { get; set; }
        public int insurance_value { get; set; }
        public int service_id { get; set; }
        public int service_type_id { get; set; }
        public string? coupon { get; set; }
        public List<int> pick_shift { get; set; }
        public List<DeliveryOrderItemDto> items { get; set; }
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
        public OrderCategoryDto category { get; set; }
    }

    public class OrderCategoryDto
    {
        public string level1 { get; set; }
    }
}
