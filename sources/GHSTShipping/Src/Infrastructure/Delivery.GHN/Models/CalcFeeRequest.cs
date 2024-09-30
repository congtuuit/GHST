namespace Delivery.GHN.Models
{
    public class CalcFeeRequest
    {
        public int from_district_id { get; set; }
        public string from_ward_code { get; set; }
        public int service_id { get; set; }
        public int? service_type_id { get; set; }
        public int to_district_id { get; set; }
        public string to_ward_code { get; set; }
        public int height { get; set; }
        public int length { get; set; }
        public int weight { get; set; }
        public int width { get; set; }
        public int insurance_value { get; set; }
        public int cod_failed_amount { get; set; }
        public string? coupon { get; set; }  // Nullable, can be null
    }
}
