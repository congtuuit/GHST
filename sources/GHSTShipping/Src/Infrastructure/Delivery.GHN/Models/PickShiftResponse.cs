using Newtonsoft.Json;

namespace Delivery.GHN.Models
{
    public class PickShiftResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("from_time")]
        public int FromTime { get; set; }

        [JsonProperty("to_time")]
        public int ToTime { get; set; }
    }
}
