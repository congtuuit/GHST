using GHSTShipping.Domain.Entities;

namespace GHSTShipping.Domain.DTOs
{
    public class OrderDto : Order
    {
        public int No { get; set; }
        public int PrivateTotalFee { get; set; }
        public string PrivateOrderCode { get; set; }

        public string ShopName { get; set; }
        public string OrderCode { get; set; }
    }
}
