namespace Delivery.GHN.Models
{
    public class DistrictResponse
    {
        public int DistrictID { get; set; }
        public int ProvinceID { get; set; }
        public string? ProvinceName { get; set; }
        public string DistrictName { get; set; }

        public string Display
        {
            get
            {
                return this.DistrictName + " - " + this.ProvinceName;
            }
        }

        public int Type { get; set; }
        public int SupportType { get; set; }
        public List<string> NameExtension { get; set; }
        public bool CanUpdateCOD { get; set; }
        public int Status { get; set; }
        public int PickType { get; set; }
        public int DeliverType { get; set; }
        public WhiteListClient WhiteListClient { get; set; }
        public WhiteListDistrict WhiteListDistrict { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
        public object OnDates { get; set; }
        public string CreatedIP { get; set; }
        public int CreatedEmployee { get; set; }
        public string CreatedSource { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedIP { get; set; }
        public int UpdatedEmployee { get; set; }
        public string UpdatedSource { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class WhiteListClient
    {
        public List<object> From { get; set; }
        public List<object> To { get; set; }
        public List<object> Return { get; set; }
    }

    public class WhiteListDistrict
    {
        public object From { get; set; }
        public object To { get; set; }
    }
}
