namespace Delivery.GHN.Models
{
    public class CancelOrderResponse
    {
        public string order_code { get; set; }
        public bool result { get; set; }
        public string message { get; set; }
    }
}
