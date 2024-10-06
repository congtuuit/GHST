using GHSTShipping.Domain.Entities;

namespace GHSTShipping.Domain.DTOs
{
    public class OrderDto : Order
    {
        public int No { get; set; }
        public int PrivateTotalFee { get; set; }
        public string PrivateOrderCode { get; set; }
    }
}
