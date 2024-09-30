namespace Delivery.GHN.Models
{
    public class GetAllShopsRequest
    {
        public int offset { get; set; }
        public int limit { get; set; }
        public string client_phone { get; set; }
    }
}
