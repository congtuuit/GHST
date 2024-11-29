using System;

namespace GHSTShipping.Domain.DTOs
{
    public class ShopDeliveryPricePlaneDto
    {
        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? ShopId { get; set; }
        public string Name { get; set; }
        public long MinWeight { get; set; }
        public long MaxWeight { get; set; }
        public long PublicPrice { get; set; }
        public long PrivatePrice { get; set; }
        public long StepPrice { get; set; }
        public long StepWeight { get; set; }
        public long LimitInsurance { get; set; }
        public decimal InsuranceFeeRate { get; set; }
        public decimal ReturnFeeRate { get; set; }
        public decimal ConvertWeightRate { get; set; }


        //
        public string PartnerShopId { get; set; }
        public string ClientPhone { get; set; }
        public string? Address { get; set; }
        public string? WardName { get; set; }
        public string? WardCode { get; set; }
        public string? DistrictName { get; set; }
        public string? DistrictId { get; set; }
        public string? ProvinceName { get; set; }
        public string? ProvinceId { get; set; }
        public string ShopName { get; set; }
    }
}
