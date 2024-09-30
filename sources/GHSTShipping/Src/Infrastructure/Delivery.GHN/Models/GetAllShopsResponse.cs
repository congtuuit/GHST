namespace Delivery.GHN.Models
{
    public class GetAllShopsResponse
    {
        public int last_offset { get; set; }
        public List<GhnShopDto> shops { get; set; }

        public class GhnShopDto
        {
            public int _id { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public string address { get; set; }
            public string ward_code { get; set; }
            public int district_id { get; set; }
            public int client_id { get; set; }
            public int bank_account_id { get; set; }
            public int status { get; set; }
            public string version_no { get; set; }
            public bool is_created_chat_channel { get; set; }
            public string updated_ip { get; set; }
            public int updated_employee { get; set; }
            public int updated_client { get; set; }
            public string updated_source { get; set; }
            public DateTime updated_date { get; set; }
            public string created_ip { get; set; }
            public int created_employee { get; set; }
            public int created_client { get; set; }
            public string created_source { get; set; }
            public DateTime created_date { get; set; }
        }
    }
}
