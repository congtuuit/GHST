using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Delivery.GHN.Models
{
    public class SearchOrderResponse
    {
        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public List<OrderDataDto> Data { get; set; }

        [JsonProperty("soc")]
        [JsonPropertyName("soc")]
        public List<SocDto> Soc { get; set; }

        [JsonProperty("total")]
        [JsonPropertyName("total")]
        public int Total { get; set; }

    }

    public class Category
    {
    }

    public class OrderDataDto
    {
        [JsonProperty("ahamove_order_code")]
        [JsonPropertyName("ahamove_order_code")]
        public string AhamoveOrderCode { get; set; }

        [JsonProperty("ahamove_recipient_address_detail")]
        [JsonPropertyName("ahamove_recipient_address_detail")]
        public string AhamoveRecipientAddressDetail { get; set; }

        [JsonProperty("ahamove_seller_address_detail")]
        [JsonPropertyName("ahamove_seller_address_detail")]
        public string AhamoveSellerAddressDetail { get; set; }

        [JsonProperty("pd_service_id")]
        [JsonPropertyName("pd_service_id")]
        public int PdServiceId { get; set; }

        [JsonProperty("province_id")]
        [JsonPropertyName("province_id")]
        public int ProvinceId { get; set; }

        [JsonProperty("shop_id")]
        [JsonPropertyName("shop_id")]
        public int ShopId { get; set; }

        [JsonProperty("client_id")]
        [JsonPropertyName("client_id")]
        public int ClientId { get; set; }

        [JsonProperty("return_name")]
        [JsonPropertyName("return_name")]
        public string ReturnName { get; set; }

        [JsonProperty("return_phone")]
        [JsonPropertyName("return_phone")]
        public string ReturnPhone { get; set; }

        [JsonProperty("return_address")]
        [JsonPropertyName("return_address")]
        public string ReturnAddress { get; set; }

        [JsonProperty("return_ward_code")]
        [JsonPropertyName("return_ward_code")]
        public string ReturnWardCode { get; set; }

        [JsonProperty("return_district_id")]
        [JsonPropertyName("return_district_id")]
        public int ReturnDistrictId { get; set; }

        [JsonProperty("return_location")]
        [JsonPropertyName("return_location")]
        public ReturnLocationDto ReturnLocation { get; set; }

        [JsonProperty("from_name")]
        [JsonPropertyName("from_name")]
        public string FromName { get; set; }

        [JsonProperty("from_phone")]
        [JsonPropertyName("from_phone")]
        public string FromPhone { get; set; }

        [JsonProperty("from_hotline")]
        [JsonPropertyName("from_hotline")]
        public string FromHotline { get; set; }

        [JsonProperty("from_address")]
        [JsonPropertyName("from_address")]
        public string FromAddress { get; set; }

        [JsonProperty("from_ward_code")]
        [JsonPropertyName("from_ward_code")]
        public string FromWardCode { get; set; }

        [JsonProperty("from_district_id")]
        [JsonPropertyName("from_district_id")]
        public int FromDistrictId { get; set; }

        [JsonProperty("from_location")]
        [JsonPropertyName("from_location")]
        public FromLocationDto FromLocation { get; set; }

        [JsonProperty("deliver_station_id")]
        [JsonPropertyName("deliver_station_id")]
        public int DeliverStationId { get; set; }

        [JsonProperty("to_name")]
        [JsonPropertyName("to_name")]
        public string ToName { get; set; }

        [JsonProperty("to_phone")]
        [JsonPropertyName("to_phone")]
        public string ToPhone { get; set; }

        [JsonProperty("to_address")]
        [JsonPropertyName("to_address")]
        public string ToAddress { get; set; }

        [JsonProperty("to_ward_code")]
        [JsonPropertyName("to_ward_code")]
        public string ToWardCode { get; set; }

        [JsonProperty("to_district_id")]
        [JsonPropertyName("to_district_id")]
        public int ToDistrictId { get; set; }

        [JsonProperty("to_location")]
        [JsonPropertyName("to_location")]
        public ToLocationDto ToLocation { get; set; }

        [JsonProperty("weight")]
        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        [JsonProperty("length")]
        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonProperty("width")]
        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonProperty("converted_weight")]
        [JsonPropertyName("converted_weight")]
        public int ConvertedWeight { get; set; }

        [JsonProperty("calculate_weight")]
        [JsonPropertyName("calculate_weight")]
        public int CalculateWeight { get; set; }

        [JsonProperty("image_ids")]
        [JsonPropertyName("image_ids")]
        public object ImageIds { get; set; }

        [JsonProperty("service_type_id")]
        [JsonPropertyName("service_type_id")]
        public int ServiceTypeId { get; set; }

        [JsonProperty("service_id")]
        [JsonPropertyName("service_id")]
        public int ServiceId { get; set; }

        [JsonProperty("payment_type_id")]
        [JsonPropertyName("payment_type_id")]
        public int PaymentTypeId { get; set; }

        [JsonProperty("payment_type_ids")]
        [JsonPropertyName("payment_type_ids")]
        public List<int> PaymentTypeIds { get; set; }

        [JsonProperty("custom_service_fee")]
        [JsonPropertyName("custom_service_fee")]
        public int CustomServiceFee { get; set; }

        [JsonProperty("sort_code")]
        [JsonPropertyName("sort_code")]
        public string SortCode { get; set; }

        [JsonProperty("cod_amount")]
        [JsonPropertyName("cod_amount")]
        public int CodAmount { get; set; }

        [JsonProperty("cod_collect_date")]
        [JsonPropertyName("cod_collect_date")]
        public object CodCollectDate { get; set; }

        [JsonProperty("cod_transfer_date")]
        [JsonPropertyName("cod_transfer_date")]
        public object CodTransferDate { get; set; }

        [JsonProperty("is_cod_transferred")]
        [JsonPropertyName("is_cod_transferred")]
        public bool IsCodTransferred { get; set; }

        [JsonProperty("is_cod_collected")]
        [JsonPropertyName("is_cod_collected")]
        public bool IsCodCollected { get; set; }

        [JsonProperty("insurance_value")]
        [JsonPropertyName("insurance_value")]
        public int InsuranceValue { get; set; }

        [JsonProperty("order_value")]
        [JsonPropertyName("order_value")]
        public int OrderValue { get; set; }

        [JsonProperty("pick_station_id")]
        [JsonPropertyName("pick_station_id")]
        public int PickStationId { get; set; }

        [JsonProperty("client_order_code")]
        [JsonPropertyName("client_order_code")]
        public string ClientOrderCode { get; set; }

        [JsonProperty("cod_failed_amount")]
        [JsonPropertyName("cod_failed_amount")]
        public int CodFailedAmount { get; set; }

        [JsonProperty("cod_failed_collect_date")]
        [JsonPropertyName("cod_failed_collect_date")]
        public object CodFailedCollectDate { get; set; }

        [JsonProperty("required_note")]
        [JsonPropertyName("required_note")]
        public string RequiredNote { get; set; }

        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonProperty("note")]
        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonProperty("employee_note")]
        [JsonPropertyName("employee_note")]
        public string EmployeeNote { get; set; }

        [JsonProperty("seal_code")]
        [JsonPropertyName("seal_code")]
        public string SealCode { get; set; }

        [JsonProperty("pickup_time")]
        [JsonPropertyName("pickup_time")]
        public DateTime PickupTime { get; set; }

        [JsonProperty("items")]
        [JsonPropertyName("items")]
        public List<ItemDto> Items { get; set; }

        [JsonProperty("coupon")]
        [JsonPropertyName("coupon")]
        public string Coupon { get; set; }

        [JsonProperty("coupon_campaign_id")]
        [JsonPropertyName("coupon_campaign_id")]
        public int CouponCampaignId { get; set; }

        [JsonProperty("_id")]
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonProperty("order_code")]
        [JsonPropertyName("order_code")]
        public string OrderCode { get; set; }

        [JsonProperty("version_no")]
        [JsonPropertyName("version_no")]
        public string VersionNo { get; set; }

        [JsonProperty("updated_ip")]
        [JsonPropertyName("updated_ip")]
        public string UpdatedIp { get; set; }

        [JsonProperty("updated_employee")]
        [JsonPropertyName("updated_employee")]
        public int UpdatedEmployee { get; set; }

        [JsonProperty("updated_client")]
        [JsonPropertyName("updated_client")]
        public int UpdatedClient { get; set; }

        [JsonProperty("updated_source")]
        [JsonPropertyName("updated_source")]
        public string UpdatedSource { get; set; }

        [JsonProperty("updated_date")]
        [JsonPropertyName("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("updated_warehouse")]
        [JsonPropertyName("updated_warehouse")]
        public int UpdatedWarehouse { get; set; }

        [JsonProperty("created_ip")]
        [JsonPropertyName("created_ip")]
        public string CreatedIp { get; set; }

        [JsonProperty("created_employee")]
        [JsonPropertyName("created_employee")]
        public int CreatedEmployee { get; set; }

        [JsonProperty("created_client")]
        [JsonPropertyName("created_client")]
        public int CreatedClient { get; set; }

        [JsonProperty("created_source")]
        [JsonPropertyName("created_source")]
        public string CreatedSource { get; set; }

        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("internal_process")]
        [JsonPropertyName("internal_process")]
        public InternalProcess InternalProcess { get; set; }

        [JsonProperty("pick_warehouse_id")]
        [JsonPropertyName("pick_warehouse_id")]
        public int PickWarehouseId { get; set; }

        [JsonProperty("deliver_warehouse_id")]
        [JsonPropertyName("deliver_warehouse_id")]
        public int DeliverWarehouseId { get; set; }

        [JsonProperty("current_warehouse_id")]
        [JsonPropertyName("current_warehouse_id")]
        public int CurrentWarehouseId { get; set; }

        [JsonProperty("return_warehouse_id")]
        [JsonPropertyName("return_warehouse_id")]
        public int ReturnWarehouseId { get; set; }

        [JsonProperty("next_warehouse_id")]
        [JsonPropertyName("next_warehouse_id")]
        public int NextWarehouseId { get; set; }

        [JsonProperty("current_transport_warehouse_id")]
        [JsonPropertyName("current_transport_warehouse_id")]
        public int CurrentTransportWarehouseId { get; set; }

        [JsonProperty("leadtime")]
        [JsonPropertyName("leadtime")]
        public DateTime Leadtime { get; set; }

        [JsonProperty("order_date")]
        [JsonPropertyName("order_date")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public OrderDataDto Data { get; set; }

        [JsonProperty("soc_id")]
        [JsonPropertyName("soc_id")]
        public string SocId { get; set; }

        [JsonProperty("finish_date")]
        [JsonPropertyName("finish_date")]
        public DateTime FinishDate { get; set; }

        [JsonProperty("tag")]
        [JsonPropertyName("tag")]
        public List<string> Tag { get; set; }

        [JsonProperty("log")]
        [JsonPropertyName("log")]
        public List<OrderLogDto> Log { get; set; }

        [JsonProperty("is_partial_return")]
        [JsonPropertyName("is_partial_return")]
        public bool IsPartialReturn { get; set; }

        [JsonProperty("is_document_return")]
        [JsonPropertyName("is_document_return")]
        public bool IsDocumentReturn { get; set; }

        [JsonProperty("pick_shift")]
        [JsonPropertyName("pick_shift")]
        public List<int> PickShift { get; set; }

        [JsonProperty("pickup_shift")]
        [JsonPropertyName("pickup_shift")]
        public PickupShiftDto PickupShift { get; set; }

        [JsonProperty("updated_date_pick_shift")]
        [JsonPropertyName("updated_date_pick_shift")]
        public DateTime UpdatedDatePickShift { get; set; }

        [JsonProperty("transaction_ids")]
        [JsonPropertyName("transaction_ids")]
        public List<string> TransactionIds { get; set; }

        [JsonProperty("transportation_status")]
        [JsonPropertyName("transportation_status")]
        public string TransportationStatus { get; set; }

        [JsonProperty("transportation_phase")]
        [JsonPropertyName("transportation_phase")]
        public string TransportationPhase { get; set; }

        [JsonProperty("extra_service")]
        [JsonPropertyName("extra_service")]
        public ExtraServiceDto ExtraService { get; set; }

        [JsonProperty("config_fee_id")]
        [JsonPropertyName("config_fee_id")]
        public string ConfigFeeId { get; set; }

        [JsonProperty("extra_cost_id")]
        [JsonPropertyName("extra_cost_id")]
        public string ExtraCostId { get; set; }

        [JsonProperty("standard_config_fee_id")]
        [JsonPropertyName("standard_config_fee_id")]
        public string StandardConfigFeeId { get; set; }

        [JsonProperty("standard_extra_cost_id")]
        [JsonPropertyName("standard_extra_cost_id")]
        public string StandardExtraCostId { get; set; }

        [JsonProperty("ecom_config_fee_id")]
        [JsonPropertyName("ecom_config_fee_id")]
        public int EcomConfigFeeId { get; set; }

        [JsonProperty("ecom_extra_cost_id")]
        [JsonPropertyName("ecom_extra_cost_id")]
        public int EcomExtraCostId { get; set; }

        [JsonProperty("ecom_standard_config_fee_id")]
        [JsonPropertyName("ecom_standard_config_fee_id")]
        public int EcomStandardConfigFeeId { get; set; }

        [JsonProperty("ecom_standard_extra_cost_id")]
        [JsonPropertyName("ecom_standard_extra_cost_id")]
        public int EcomStandardExtraCostId { get; set; }

        [JsonProperty("is_b2b")]
        [JsonPropertyName("is_b2b")]
        public bool IsB2b { get; set; }

        [JsonProperty("operation_partner")]
        [JsonPropertyName("operation_partner")]
        public string OperationPartner { get; set; }

        [JsonProperty("process_partner_name")]
        [JsonPropertyName("process_partner_name")]
        public string ProcessPartnerName { get; set; }
    }

    public class SearchGhnOrderDetailDto
    {
        [JsonProperty("cod_failed_fee")]
        [JsonPropertyName("cod_failed_fee")]
        public int CodFailedFee { get; set; }

        [JsonProperty("cod_fee")]
        [JsonPropertyName("cod_fee")]
        public int CodFee { get; set; }

        [JsonProperty("coupon")]
        [JsonPropertyName("coupon")]
        public int Coupon { get; set; }

        [JsonProperty("deliver_remote_areas_fee")]
        [JsonPropertyName("deliver_remote_areas_fee")]
        public int DeliverRemoteAreasFee { get; set; }

        [JsonProperty("deliver_remote_areas_fee_return")]
        [JsonPropertyName("deliver_remote_areas_fee_return")]
        public int DeliverRemoteAreasFeeReturn { get; set; }

        [JsonProperty("document_return")]
        [JsonPropertyName("document_return")]
        public int DocumentReturn { get; set; }

        [JsonProperty("double_check")]
        [JsonPropertyName("double_check")]
        public int DoubleCheck { get; set; }

        [JsonProperty("double_check_deliver")]
        [JsonPropertyName("double_check_deliver")]
        public int DoubleCheckDeliver { get; set; }

        [JsonProperty("insurance")]
        [JsonPropertyName("insurance")]
        public int Insurance { get; set; }

        [JsonProperty("main_service")]
        [JsonPropertyName("main_service")]
        public int MainService { get; set; }

        [JsonProperty("pick_remote_areas_fee")]
        [JsonPropertyName("pick_remote_areas_fee")]
        public int PickRemoteAreasFee { get; set; }

        [JsonProperty("pick_remote_areas_fee_return")]
        [JsonPropertyName("pick_remote_areas_fee_return")]
        public int PickRemoteAreasFeeReturn { get; set; }

        [JsonProperty("r2s")]
        [JsonPropertyName("r2s")]
        public int R2s { get; set; }

        [JsonProperty("return")]
        [JsonPropertyName("return")]
        public int Return { get; set; }

        [JsonProperty("return_again")]
        [JsonPropertyName("return_again")]
        public int ReturnAgain { get; set; }

        [JsonProperty("station_do")]
        [JsonPropertyName("station_do")]
        public int StationDo { get; set; }

        [JsonProperty("station_pu")]
        [JsonPropertyName("station_pu")]
        public int StationPu { get; set; }
    }

    public class GhnDiffDto
    {
        [JsonProperty("cod_failed_fee")]
        [JsonPropertyName("cod_failed_fee")]
        public int CodFailedFee { get; set; }

        [JsonProperty("cod_fee")]
        [JsonPropertyName("cod_fee")]
        public int CodFee { get; set; }

        [JsonProperty("coupon")]
        [JsonPropertyName("coupon")]
        public int Coupon { get; set; }

        [JsonProperty("deliver_remote_areas_fee")]
        [JsonPropertyName("deliver_remote_areas_fee")]
        public int DeliverRemoteAreasFee { get; set; }

        [JsonProperty("deliver_remote_areas_fee_return")]
        [JsonPropertyName("deliver_remote_areas_fee_return")]
        public int DeliverRemoteAreasFeeReturn { get; set; }

        [JsonProperty("document_return")]
        [JsonPropertyName("document_return")]
        public int DocumentReturn { get; set; }

        [JsonProperty("double_check")]
        [JsonPropertyName("double_check")]
        public int DoubleCheck { get; set; }

        [JsonProperty("double_check_deliver")]
        [JsonPropertyName("double_check_deliver")]
        public int DoubleCheckDeliver { get; set; }

        [JsonProperty("insurance")]
        [JsonPropertyName("insurance")]
        public int Insurance { get; set; }

        [JsonProperty("main_service")]
        [JsonPropertyName("main_service")]
        public int MainService { get; set; }

        [JsonProperty("pick_remote_areas_fee")]
        [JsonPropertyName("pick_remote_areas_fee")]
        public int PickRemoteAreasFee { get; set; }

        [JsonProperty("pick_remote_areas_fee_return")]
        [JsonPropertyName("pick_remote_areas_fee_return")]
        public int PickRemoteAreasFeeReturn { get; set; }

        [JsonProperty("r2s")]
        [JsonPropertyName("r2s")]
        public int R2s { get; set; }

        [JsonProperty("return")]
        [JsonPropertyName("return")]
        public int Return { get; set; }

        [JsonProperty("return_again")]
        [JsonPropertyName("return_again")]
        public int ReturnAgain { get; set; }

        [JsonProperty("station_do")]
        [JsonPropertyName("station_do")]
        public int StationDo { get; set; }

        [JsonProperty("station_pu")]
        [JsonPropertyName("station_pu")]
        public int StationPu { get; set; }
    }

    public class InternalProcess
    {
        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class GhnOrderPaymentDto
    {
        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("diff")]
        [JsonPropertyName("diff")]
        public GhnDiffDto Diff { get; set; }

        [JsonProperty("paid_date")]
        [JsonPropertyName("paid_date")]
        public DateTime PaidDate { get; set; }

        [JsonProperty("payment_type")]
        [JsonPropertyName("payment_type")]
        public int PaymentType { get; set; }

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }


    public class SocDto
    {
        [JsonProperty("_id")]
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonProperty("cod_collect_date")]
        [JsonPropertyName("cod_collect_date")]
        public DateTime CodCollectDate { get; set; }

        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("created_ip")]
        [JsonPropertyName("created_ip")]
        public string CreatedIp { get; set; }

        [JsonProperty("detail")]
        [JsonPropertyName("detail")]
        public SearchGhnOrderDetailDto Detail { get; set; }

        [JsonProperty("order_code")]
        [JsonPropertyName("order_code")]
        public string OrderCode { get; set; }

        [JsonProperty("payment")]
        [JsonPropertyName("payment")]
        public List<GhnOrderPaymentDto> Payment { get; set; }

        [JsonProperty("standard")]
        [JsonPropertyName("standard")]
        public StandardDto Standard { get; set; }

        [JsonProperty("transaction_id")]
        [JsonPropertyName("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("updated_client")]
        [JsonPropertyName("updated_client")]
        public int UpdatedClient { get; set; }

        [JsonProperty("updated_date")]
        [JsonPropertyName("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("updated_employee")]
        [JsonPropertyName("updated_employee")]
        public int UpdatedEmployee { get; set; }

        [JsonProperty("updated_ip")]
        [JsonPropertyName("updated_ip")]
        public string UpdatedIp { get; set; }

        [JsonProperty("updated_source")]
        [JsonPropertyName("updated_source")]
        public string UpdatedSource { get; set; }
    }

    public class StandardDto
    {
        [JsonProperty("cod_failed_fee")]
        [JsonPropertyName("cod_failed_fee")]
        public int CodFailedFee { get; set; }

        [JsonProperty("cod_fee")]
        [JsonPropertyName("cod_fee")]
        public int CodFee { get; set; }

        [JsonProperty("coupon")]
        [JsonPropertyName("coupon")]
        public int Coupon { get; set; }

        [JsonProperty("deliver_remote_areas_fee")]
        [JsonPropertyName("deliver_remote_areas_fee")]
        public int DeliverRemoteAreasFee { get; set; }

        [JsonProperty("deliver_remote_areas_fee_return")]
        [JsonPropertyName("deliver_remote_areas_fee_return")]
        public int DeliverRemoteAreasFeeReturn { get; set; }

        [JsonProperty("document_return")]
        [JsonPropertyName("document_return")]
        public int DocumentReturn { get; set; }

        [JsonProperty("double_check")]
        [JsonPropertyName("double_check")]
        public int DoubleCheck { get; set; }

        [JsonProperty("double_check_deliver")]
        [JsonPropertyName("double_check_deliver")]
        public int DoubleCheckDeliver { get; set; }

        [JsonProperty("insurance")]
        [JsonPropertyName("insurance")]
        public int Insurance { get; set; }

        [JsonProperty("main_service")]
        [JsonPropertyName("main_service")]
        public int MainService { get; set; }

        [JsonProperty("pick_remote_areas_fee")]
        [JsonPropertyName("pick_remote_areas_fee")]
        public int PickRemoteAreasFee { get; set; }

        [JsonProperty("pick_remote_areas_fee_return")]
        [JsonPropertyName("pick_remote_areas_fee_return")]
        public int PickRemoteAreasFeeReturn { get; set; }

        [JsonProperty("r2s")]
        [JsonPropertyName("r2s")]
        public int R2s { get; set; }

        [JsonProperty("return")]
        [JsonPropertyName("return")]
        public int Return { get; set; }

        [JsonProperty("return_again")]
        [JsonPropertyName("return_again")]
        public int ReturnAgain { get; set; }

        [JsonProperty("station_do")]
        [JsonPropertyName("station_do")]
        public int StationDo { get; set; }

        [JsonProperty("station_pu")]
        [JsonPropertyName("station_pu")]
        public int StationPu { get; set; }
    }
}

