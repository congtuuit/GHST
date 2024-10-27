using GHSTShipping.Application.DTOs.PartnerConfig;
using GHSTShipping.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public int? GhnShopId { get; set; }

        public string Status
        {
            get
            {
                return this.IsVerified ? EnumShopStatus.Activated.GetName() : EnumShopStatus.WaitingForActive.GetName();
            }
        }

        public IDictionary<Guid, IEnumerable<GhnShopDetailDto>> GhnShopDetails { get; set; }

        public IEnumerable<ShopConfigDto> ShopConfigs { get; set; }

        public IEnumerable<ShopConfigViewDto> Partners { get; set; }
    }

    public class GhnShopDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get { return $"{this.Id} - {this.Name}"; } }

        // Another fields
        // https://api.ghn.vn/home/docs/detail?id=106
        public string Phone { get; set; }
        public string Address { get; set; }
        public string WardCode { get; set; }
        public string DistrictId { get; set; }
    }
}
