using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Delivery.GHN.Models
{
    public class ShippingOrderSearchRequest
    {
        [JsonProperty("shop_id")]
        [JsonPropertyName("shop_id")]
        public int ShopId { get; set; } = 0;

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public List<string> Status { get; set; } = new List<string>
        {
            "ready_to_pick", "picking", "money_collect_picking", "picked", "sorting", "storing",
            "transporting", "delivering", "delivery_fail", "money_collect_delivering", "return",
            "returning", "return_fail", "return_transporting", "return_sorting", "waiting_to_return",
            "returned", "delivered", "cancel", "lost", "damage"
        };

        [JsonProperty("payment_type_id")]
        [JsonPropertyName("payment_type_id")]
        public List<int> PaymentTypeId { get; set; } = new List<int> { 1, 2, 11, 12 };

        [JsonProperty("from_time")]
        [JsonPropertyName("from_time")]
        public long FromTime { get; set; }

        [JsonProperty("to_time")]
        [JsonPropertyName("to_time")]
        public long ToTime { get; set; }

        [JsonProperty("offset")]
        [JsonPropertyName("offset")]
        public int Offset { get; set; } = 0;

        [JsonProperty("limit")]
        [JsonPropertyName("limit")]
        public int Limit { get; set; } = 20;

        [JsonProperty("option_value")]
        [JsonPropertyName("option_value")]
        public string OptionValue { get; set; } = "";

        [JsonProperty("from_cod_amount")]
        [JsonPropertyName("from_cod_amount")]
        public int FromCodAmount { get; set; } = 0;

        [JsonProperty("to_cod_amount")]
        [JsonPropertyName("to_cod_amount")]
        public int ToCodAmount { get; set; } = 1000000000;

        [JsonProperty("ignore_shop_id")]
        [JsonPropertyName("ignore_shop_id")]
        public bool IgnoreShopId { get; set; } = false;

        [JsonProperty("shop_ids")]
        [JsonPropertyName("shop_ids")]
        public List<int> ShopIds { get; set; } = new List<int>();

        [JsonProperty("is_search_exactly")]
        [JsonPropertyName("is_search_exactly")]
        public bool IsSearchExactly { get; set; } = true;

        [JsonProperty("is_print")]
        [JsonPropertyName("is_print")]
        public bool? IsPrint { get; set; } = null;

        [JsonProperty("is_cod_failed_collected")]
        [JsonPropertyName("is_cod_failed_collected")]
        public bool? IsCodFailedCollected { get; set; } = null;

        [JsonProperty("is_document_pod")]
        [JsonPropertyName("is_document_pod")]
        public bool? IsDocumentPod { get; set; } = null;

        [JsonProperty("type_time")]
        [JsonPropertyName("type_time")]
        public string TypeTime { get; set; } = "created_date";

        [JsonProperty("source")]
        [JsonPropertyName("source")]
        public string Source { get; set; } = "5sao";
    }
}
