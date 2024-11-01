using System;

namespace GHSTShipping.Domain.DTOs
{
    public class OrderDetailDto : OrderDto
    {
        public string PrivateOrderCode { get; set; }
    }
}
