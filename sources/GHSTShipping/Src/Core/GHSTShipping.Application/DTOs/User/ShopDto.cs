using GHSTShipping.Domain.Enums;
using System;

namespace GHSTShipping.Application.DTOs.User
{
    public class ShopDto
    {
        public int No { get; set; }
        public Guid Id { get; set; }
        public string ShopUniqueCode { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string ShopName { get; set; }
        public string FullName { get; set; }
        public decimal AvgMonthlyCapacity { get; set; }
        public bool IsVerified { get; set; }
        public string Status
        {
            get
            {
                return this.IsVerified ? EnumShopStatus.Activated.GetName() : EnumShopStatus.WaitingForActive.GetName();
            }
        }
    }
}
