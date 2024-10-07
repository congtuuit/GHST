using GHSTShipping.Domain.Enums;
using System;

namespace GHSTShipping.Application.DTOs.Shop
{
    public class ShopViewDetailDto
    {
        public Guid Id { get; set; }
        public string ShopUniqueCode { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string ShopName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal AvgMonthlyCapacity { get; set; }
        public bool IsVerified { get; set; }
        public Guid AccountId { get; set; }
        public string PhoneNumber { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountHolder { get; set; }
        public string BankAddress { get; set; }
        public bool AllowPublishOrder { get; set; }

        public string Status
        {
            get
            {
                return this.IsVerified ? EnumShopStatus.Activated.GetName() : EnumShopStatus.WaitingForActive.GetName();
            }
        }
    }
}
