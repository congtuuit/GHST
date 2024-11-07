using Newtonsoft.Json;
using System.Text.Json.Serialization;

public class CreateDeliveryOrderRequest
{
    [JsonProperty("pick_shift")]
    [JsonPropertyName("pick_shift")]
    public List<int> PickShift { get; set; }

    /// <summary>
    /// 2: Hàng nhẹ, 5: Hàng nặng
    /// </summary>
    [JsonProperty("service_type_id")]
    [JsonPropertyName("service_type_id")]
    public int ServiceTypeId { get; set; }

    [JsonProperty("from_phone")]
    [JsonPropertyName("from_phone")]
    public required string FromPhone { get; set; }

    [JsonProperty("from_name")]
    [JsonPropertyName("from_name")]
    public required string FromName { get; set; }

    [JsonProperty("from_province_id")]
    [JsonPropertyName("from_province_id")]
    public int? FromProvinceId { get; set; }

    [JsonProperty("from_province_name")]
    [JsonPropertyName("from_province_name")]
    public required string FromProvinceName { get; set; }

    [JsonProperty("from_address")]
    [JsonPropertyName("from_address")]
    public required string FromAddress { get; set; }

    [JsonProperty("from_district_id")]
    [JsonPropertyName("from_district_id")]
    public int? FromDistrictId { get; set; }

    [JsonProperty("from_district_name")]
    [JsonPropertyName("from_district_name")]
    public required string FromDistrictName { get; set; }

    [JsonProperty("from_ward_id")]
    [JsonPropertyName("from_ward_id")]
    public int? FromWardId { get; set; }

    [JsonProperty("from_ward_name")]
    [JsonPropertyName("from_ward_name")]
    public required string FromWardName { get; set; }

    [JsonProperty("to_phone")]
    [JsonPropertyName("to_phone")]
    public required string ToPhone { get; set; }

    [JsonProperty("to_name")]
    [JsonPropertyName("to_name")]
    public required string ToName { get; set; }

    [JsonProperty("to_province_id")]
    [JsonPropertyName("to_province_id")]
    public int? ToProvinceId { get; set; }

    [JsonProperty("to_province_name")]
    [JsonPropertyName("to_province_name")]
    public required string ToProvinceName { get; set; }

    [JsonProperty("to_address")]
    [JsonPropertyName("to_address")]
    public required string ToAddress { get; set; }

    [JsonProperty("to_district_id")]
    [JsonPropertyName("to_district_id")]
    public int? ToDistrictId { get; set; }

    [JsonProperty("to_district_name")]
    [JsonPropertyName("to_district_name")]
    public required string ToDistrictName { get; set; }

    [JsonProperty("to_ward_id")]
    [JsonPropertyName("to_ward_id")]
    public int? ToWardId { get; set; }

    [JsonProperty("to_ward_name")]
    [JsonPropertyName("to_ward_name")]
    public required string ToWardName { get; set; }

    [JsonProperty("items")]
    [JsonPropertyName("items")]
    public required List<Item> Items { get; set; }

    [JsonProperty("weight")]
    [JsonPropertyName("weight")]
    public int Weight { get; set; }

    [JsonProperty("length")]
    [JsonPropertyName("length")]
    public int Length { get; set; }

    [JsonProperty("width")]
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonProperty("height")]
    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonProperty("cod_amount")]
    [JsonPropertyName("cod_amount")]
    public int CodAmount { get; set; }

    [JsonProperty("insurance_value")]
    [JsonPropertyName("insurance_value")]
    public int InsuranceValue { get; set; }

    [JsonProperty("required_note")]
    [JsonPropertyName("required_note")]
    public string RequiredNote { get; set; }

    [JsonProperty("note")]
    [JsonPropertyName("note")]
    public string? Note { get; set; }

    [JsonProperty("content")]
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonProperty("payment_type_id")]
    [JsonPropertyName("payment_type_id")]
    public int PaymentTypeId { get; set; }

    [JsonProperty("client_order_code")]
    [JsonPropertyName("client_order_code")]
    public string? ClientOrderCode { get; set; }

    // Optional

    [JsonProperty("return_phone")]
    [JsonPropertyName("return_phone")]
    public string? ReturnPhone { get; set; }

    [JsonProperty("return_address")]
    [JsonPropertyName("return_address")]
    public string? ReturnAddress { get; set; }

    [JsonProperty("return_district_name")]
    [JsonPropertyName("return_district_name")]
    public string? ReturnDistrictName { get; set; }

    [JsonProperty("return_ward_name")]
    [JsonPropertyName("return_ward_name")]
    public string? ReturnWardName { get; set; }

    [JsonProperty("pick_station_id")]
    [JsonPropertyName("pick_station_id")]
    public int? PickStationId { get; set; }

    [JsonProperty("deliver_station_id")]
    [JsonPropertyName("deliver_station_id")]
    public int? DeliverStationId { get; set; }

    [JsonProperty("service_id")]
    [JsonPropertyName("service_id")]
    public int ServiceId { get; set; }

    [JsonProperty("coupon")]
    [JsonPropertyName("coupon")]
    public string? Coupon { get; set; }

    public class Item
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("code")]
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonProperty("weight")]
        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        [JsonProperty("price")]
        [JsonPropertyName("price")]
        public int? Price { get; set; }

        [JsonProperty("length")]
        [JsonPropertyName("length")]
        public int? Length { get; set; }

        [JsonProperty("width")]
        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonProperty("height")]
        [JsonPropertyName("height")]
        public int? Height { get; set; }
    }
}
