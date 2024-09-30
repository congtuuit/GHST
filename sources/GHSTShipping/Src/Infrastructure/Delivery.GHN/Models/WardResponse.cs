using Newtonsoft.Json;

namespace Delivery.GHN.Models
{
    public class WardResponse
    {

        [JsonProperty("WardCode")]
        public int WardCode { get; set; }

        [JsonProperty("DistrictID")]
        public int DistrictId { get; set; }

        [JsonProperty("WardName")]
        public string WardName { get; set; }
    }
}
