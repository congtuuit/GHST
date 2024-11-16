using System;

namespace GHSTShipping.Application.DTOs.Shop
{
    /// <summary>
    /// Simple shop model with basic data
    /// </summary>
    public class BasicShopInfoDto
    {
        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsVerified { get; set; }
        public string WardId { get; set; }
        public string WardName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
    }
}
