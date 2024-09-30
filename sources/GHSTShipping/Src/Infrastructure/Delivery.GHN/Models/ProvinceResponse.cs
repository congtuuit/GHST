using Newtonsoft.Json;

namespace Delivery.GHN.Models
{
    public class ProvinceResponse
    {
        [JsonProperty("ProvinceID")]
        public int ProvinceId { get; set; }

        [JsonProperty("ProvinceName")]
        public string ProvinceName { get; set; }

        [JsonProperty("CountryID")]
        public int CountryId { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("NameExtension")]
        public List<string> NameExtension { get; set; }

        [JsonProperty("IsEnable")]
        public int IsEnable { get; set; }

        [JsonProperty("RegionID")]
        public int RegionId { get; set; }

        [JsonProperty("RegionCPN")]
        public int RegionCPN { get; set; }

        [JsonProperty("UpdatedBy")]
        public int UpdatedBy { get; set; }

        [JsonProperty("CreatedAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("UpdatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("AreaID")]
        public int AreaId { get; set; }

        [JsonProperty("CanUpdateCOD")]
        public bool CanUpdateCOD { get; set; }

        [JsonProperty("Status")]
        public int Status { get; set; }

        [JsonProperty("UpdatedIP")]
        public string UpdatedIP { get; set; }

        [JsonProperty("UpdatedEmployee")]
        public int UpdatedEmployee { get; set; }

        [JsonProperty("UpdatedSource")]
        public string UpdatedSource { get; set; }

        [JsonProperty("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }
    }
}
