namespace Delivery.GHN.Models
{
    public class RegisterShopRequest
    {
        public int district_id { get; set; }
        public string ward_code { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
    }
}
