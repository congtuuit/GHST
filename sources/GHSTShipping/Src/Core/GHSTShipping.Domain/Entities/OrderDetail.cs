using GHSTShipping.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GHSTShipping.Domain.Entities
{
    [Table(nameof(OrderDetail))]
    public class OrderDetail : AuditableBaseEntity
    {
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }

        [MaxLength(100)]
        public string DeliveryPartner { get; set; }

        /// <summary>
        /// Delivery parner shop ID, map to shopId of partner
        /// </summary>
        [MaxLength(100)]
        public string PartnerShopId { get; set; }


        #region <Delivery parter column mapping>

        public int ClientId { get; set; }

        [MaxLength(100)]
        public string? ReturnName { get; set; }

        [MaxLength(100)]
        public string? ReturnPhone { get; set; }

        [MaxLength(100)]
        public string? ReturnAddress { get; set; }

        [MaxLength(100)]
        public string? ReturnWardCode { get; set; }

        public int ReturnDistrictId { get; set; }
        //public ReturnLocationDto ReturnLocation { get; set; }

        [MaxLength(100)]
        public string? FromName { get; set; }

        [MaxLength(100)]
        public string? FromPhone { get; set; }

        [MaxLength(100)]
        public string? FromHotline { get; set; }

        [MaxLength(100)]
        public string? FromAddress { get; set; }

        [MaxLength(100)]
        public string? FromWardCode { get; set; }

        public int FromDistrictId { get; set; }
        //public FromLocationDto FromLocation { get; set; }
        public int DeliverStationId { get; set; }

        [MaxLength(100)]
        public string? ToName { get; set; }

        [MaxLength(100)]
        public string? ToPhone { get; set; }

        [MaxLength(100)]
        public string? ToAddress { get; set; }

        [MaxLength(100)]
        public string? ToWardCode { get; set; }

        public int ToDistrictId { get; set; }
        //public ToLocationDto ToLocation { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ConvertedWeight { get; set; }
        public int CalculateWeight { get; set; }
        //public object ImageIds { get; set; }
        public int ServiceTypeId { get; set; }
        public int ServiceId { get; set; }
        public int PaymentTypeId { get; set; }
        //public List<int> PaymentTypeIds { get; set; }
        public int CustomServiceFee { get; set; }

        [MaxLength(100)]
        public string? SortCode { get; set; }

        public int CodAmount { get; set; }
        //public object CodCollectDate { get; set; }
        //public object CodTransferDate { get; set; }
        public bool IsCodTransferred { get; set; }
        public bool IsCodCollected { get; set; }
        public int InsuranceValue { get; set; }
        public int OrderValue { get; set; }
        public int PickStationId { get; set; }

        [MaxLength(100)]
        public string? ClientOrderCode { get; set; }

        public int CodFailedAmount { get; set; }
        //public object CodFailedCollectDate { get; set; }

        [MaxLength(1000)]
        public string? RequiredNote { get; set; }

        [MaxLength(10000)]
        public string? Content { get; set; }

        [MaxLength(10000)]
        public string? Note { get; set; }

        [MaxLength(1000)]
        public string? EmployeeNote { get; set; }

        [MaxLength(100)]
        public string? SealCode { get; set; }
        public DateTime? PickupTime { get; set; }
        //public List<ItemDto> Items { get; set; }

        [MaxLength(100)]
        public string? Coupon { get; set; }

        public int CouponCampaignId { get; set; }

        [MaxLength(200)]
        public string? Id { get; set; }

        [MaxLength(100)]
        public string? OrderCode { get; set; }

        [MaxLength(100)]
        public string? VersionNo { get; set; }

        [MaxLength(100)]
        public string? UpdatedIp { get; set; }

        public int UpdatedEmployee { get; set; }
        public int UpdatedClient { get; set; }

        [MaxLength(100)]
        public string? UpdatedSource { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public int UpdatedWarehouse { get; set; }

        [MaxLength(100)]
        public string? CreatedIp { get; set; }
        public int CreatedEmployee { get; set; }
        public int CreatedClient { get; set; }

        [MaxLength(100)]
        public string? CreatedSource { get; set; }
        public DateTime? CreatedDate { get; set; }

        [MaxLength(100)]
        public string? Status { get; set; }
        //public PublicProcessDto publicProcess { get; set; }
        public int PickWarehouseId { get; set; }
        public int DeliverWarehouseId { get; set; }
        public int CurrentWarehouseId { get; set; }
        public int ReturnWarehouseId { get; set; }
        public int NextWarehouseId { get; set; }
        public int CurrentTransportWarehouseId { get; set; }
        public DateTime? Leadtime { get; set; }
        public DateTime? OrderDate { get; set; }
        //public Datato Data { get; set; }

        [MaxLength(100)]
        public string? SocId { get; set; }
        public DateTime? FinishDate { get; set; }
        //public List<string> Tag { get; set; }
        //public List<OrderLogDto> Log { get; set; }
        public bool IsPartialReturn { get; set; }
        public bool IsDocumentReturn { get; set; }
        //public List<int> PickShift { get; set; }
        //public PickupShiftDto PickupShift { get; set; }
        public DateTime? UpdatedDatePickShift { get; set; }
        //public List<string> TransactionIds { get; set; }

        [MaxLength(100)]
        public string? TransportationStatus { get; set; }

        [MaxLength(100)]
        public string? TransportationPhase { get; set; }

        /// <summary>
        /// JSON object
        /// </summary>
        public object ExtraService { get; set; }

        [MaxLength(100)]
        public string? ConfigFeeId { get; set; }

        [MaxLength(100)]
        public string? ExtraCostId { get; set; }

        [MaxLength(100)]
        public string? StandardConfigFeeId { get; set; }

        [MaxLength(100)]
        public string? StandardExtraCostId { get; set; }

        public int EcomConfigFeeId { get; set; }
        public int EcomExtraCostId { get; set; }
        public int EcomStandardConfigFeeId { get; set; }
        public int EcomStandardExtraCostId { get; set; }
        public bool IsB2b { get; set; }

        [MaxLength(100)]
        public string? OperationPartner { get; set; }

        [MaxLength(100)]
        public string? ProcessPartnerName { get; set; }

        [MaxLength(100)]
        public string? TypeOrder { get; set; }

        [MaxLength(100)]
        public string? TypeOrderCode { get; set; }

        #endregion </Delivery parter column mapping>
    }
}
