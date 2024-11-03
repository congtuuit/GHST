using System.Text.Json.Serialization;

namespace Delivery.GHN.Models
{
    public class ShippingOrderSearchRequest
    {
        [JsonPropertyName("shop_id")]
        public int ShopId { get; set; } = 0;

        [JsonPropertyName("status")]
        public List<string> Status { get; set; } = new List<string>
    {
        "ready_to_pick", "picking", "money_collect_picking", "picked", "sorting", "storing",
        "transporting", "delivering", "delivery_fail", "money_collect_delivering", "return",
        "returning", "return_fail", "return_transporting", "return_sorting", "waiting_to_return",
        "returned", "delivered", "cancel", "lost", "damage"
    };

        [JsonPropertyName("payment_type_id")]
        public List<int> PaymentTypeId { get; set; } = new List<int> { 1, 2, 11, 12 };

        [JsonPropertyName("from_time")]
        public long FromTime { get; set; }

        [JsonPropertyName("to_time")]
        public long ToTime { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; } = 0;

        [JsonPropertyName("limit")]
        public int Limit { get; set; } = 100;

        [JsonPropertyName("option_value")]
        public string OptionValue { get; set; } = "";

        [JsonPropertyName("from_cod_amount")]
        public int FromCodAmount { get; set; } = 0;

        [JsonPropertyName("to_cod_amount")]
        public int ToCodAmount { get; set; } = 1000000000;

        [JsonPropertyName("ignore_shop_id")]
        public bool IgnoreShopId { get; set; } = false;

        [JsonPropertyName("shop_ids")]
        public List<int> ShopIds { get; set; } = new List<int>();

        [JsonPropertyName("is_search_exactly")]
        public bool IsSearchExactly { get; set; } = true;

        [JsonPropertyName("is_print")]
        public bool? IsPrint { get; set; } = null;

        [JsonPropertyName("is_cod_failed_collected")]
        public bool? IsCodFailedCollected { get; set; } = null;

        [JsonPropertyName("is_document_pod")]
        public bool? IsDocumentPod { get; set; } = null;

        [JsonPropertyName("type_time")]
        public string TypeTime { get; set; } = "created_date";

        [JsonPropertyName("source")]
        public string Source { get; set; } = "5sao";
    }
}
