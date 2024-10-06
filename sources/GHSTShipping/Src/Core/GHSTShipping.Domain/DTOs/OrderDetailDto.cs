using System;

namespace GHSTShipping.Domain.DTOs
{
    public class OrderDetailDto
    {
        public Guid Id { get; set; }

        public string PrivateOrderCode { get; set; }

        public string ClientOrderCode { get; set; }

        #region
        public string Status { get; set; }
        #endregion
    }
}
