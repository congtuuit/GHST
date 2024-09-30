namespace Delivery.GHN.Models
{
    public class FeeOrderSocResponse
    {
        public string _id { get; set; }
        public string order_code { get; set; }
        public Detail detail { get; set; }
        public List<Payment> payment { get; set; }
        public DateTime cod_collect_date { get; set; }
        public string transaction_id { get; set; }
        public string created_ip { get; set; }
        public DateTime created_date { get; set; }
        public string updated_ip { get; set; }
        public int updated_client { get; set; }
        public int updated_employee { get; set; }
        public string updated_source { get; set; }
        public DateTime updated_date { get; set; }
    }

    public class Detail
    {
        public int main_service { get; set; }
        public int insurance { get; set; }
        public int station_do { get; set; }
        public int station_pu { get; set; }
        public int @return { get; set; }
        public int r2s { get; set; }
        public int coupon { get; set; }
    }

    public class Payment
    {
        public int value { get; set; }
        public int payment_type { get; set; }
        public DateTime paid_date { get; set; }
        public DateTime created_date { get; set; }
    }
}
