using Newtonsoft.Json;

namespace Delivery.GHN.Models
{
    public class DistrictResponse
    {
        [JsonProperty("DistrictID")]
        public int DistrictId { get; set; }

        [JsonProperty("ProvinceID")]
        public int ProvinceId { get; set; }

        [JsonProperty("DistrictName")]
        public string DistrictName { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Type")]
        public int Type { get; set; }

        [JsonProperty("SupportType")]
        public int SupportType { get; set; }
    }
}
