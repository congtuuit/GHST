using GHSTShipping.Domain.Entities;
using System;
using System.Collections.Generic;

namespace GHSTShipping.Domain.DTOs
{
    public class OrderDetailDto : OrderDto
    {
        public string PrivateOrderCode { get; set; }

        public List<int> PickShift { get; set; } = new List<int>();

        public int? FromProvinceId { get; set; }
        public int? FromDistrictId { get; set; }
        public string FromWardId { get; set; }

        public int? ToProvinceId { get; set; }
        public int? ToDistrictId { get; set; }
        public string ToWardId { get; set; }

        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();

        public string RequiredNote { get; set; }

        public string Note { get; set; }
    }
}
