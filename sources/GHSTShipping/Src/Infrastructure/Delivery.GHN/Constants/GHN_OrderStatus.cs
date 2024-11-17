using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Delivery.GHN.Constants
{
    [JsonConverter(typeof(JsonStringEnumConverter))] // Enables JSON conversion with custom strings
    public enum GHN_OrderStatus
    {
        /// <summary>
        /// Custom status
        /// </summary>
        [EnumMember(Value = "waiting_confirm")]
        WaitingConfirm,

        [EnumMember(Value = "ready_to_pick")]
        ReadyToPick,

        [EnumMember(Value = "picking")]
        Picking,

        [EnumMember(Value = "money_collect_picking")]
        MoneyCollectPicking,

        [EnumMember(Value = "picked")]
        Picked,

        [EnumMember(Value = "sorting")]
        Sorting,

        [EnumMember(Value = "storing")]
        Storing,

        [EnumMember(Value = "transporting")]
        Transporting,

        [EnumMember(Value = "delivering")]
        Delivering,

        [EnumMember(Value = "delivery_fail")]
        DeliveryFail,

        [EnumMember(Value = "money_collect_delivering")]
        MoneyCollectDelivering,

        [EnumMember(Value = "return")]
        Return,

        [EnumMember(Value = "returning")]
        Returning,

        [EnumMember(Value = "return_fail")]
        ReturnFail,

        [EnumMember(Value = "return_transporting")]
        ReturnTransporting,

        [EnumMember(Value = "return_sorting")]
        ReturnSorting,

        [EnumMember(Value = "waiting_to_return")]
        WaitingToReturn,

        [EnumMember(Value = "returned")]
        Returned,

        [EnumMember(Value = "delivered")]
        Delivered,

        [EnumMember(Value = "cancel")]
        Cancel,

        [EnumMember(Value = "lost")]
        Lost,

        [EnumMember(Value = "damage")]
        Damage

    }


    public static class OrderStatusHelper
    {
        public static string ToStringValue(this GHN_OrderStatus status)
        {
            // Get the field information for the enum member
            FieldInfo fieldInfo = status.GetType().GetField(status.ToString());

            // Get the EnumMember attribute from the field
            EnumMemberAttribute attribute = fieldInfo.GetCustomAttribute<EnumMemberAttribute>();

            // Return the value from the attribute, or the enum name if not found
            return attribute != null ? attribute.Value : status.ToString();
        }

        public static List<string> GetDetails(this OrderGroupStatus orderGroupStatus)
        {
            switch (orderGroupStatus)
            {
                case OrderGroupStatus.ChoBanGiao:
                    return new List<string>()
                    {
                        GHN_OrderStatus.ReadyToPick.ToStringValue(),
                        GHN_OrderStatus.Picking.ToStringValue(),
                        GHN_OrderStatus.MoneyCollectPicking.ToStringValue(),
                    };

                case OrderGroupStatus.DaBanGiaoDangGiao:
                    return new List<string>
                    {
                        GHN_OrderStatus.Picked.ToStringValue(),
                        GHN_OrderStatus.Sorting.ToStringValue(),
                        GHN_OrderStatus.Storing.ToStringValue(),
                        GHN_OrderStatus.Transporting.ToStringValue(),
                        GHN_OrderStatus.Delivering.ToStringValue(),
                        GHN_OrderStatus.DeliveryFail.ToStringValue(),
                        GHN_OrderStatus.MoneyCollectDelivering.ToStringValue(),
                    };

                case OrderGroupStatus.DaBanGiaoDangHoangHang:
                    return new List<string>
                    {
                        GHN_OrderStatus.Return.ToStringValue(),
                        GHN_OrderStatus.Returning.ToStringValue(),
                        GHN_OrderStatus.ReturnFail.ToStringValue(),
                        GHN_OrderStatus.ReturnTransporting.ToStringValue(),
                        GHN_OrderStatus.ReturnSorting.ToStringValue(),
                    };

                case OrderGroupStatus.ChoXacNhanGiaoLai:
                    return new List<string>
                    {
                        GHN_OrderStatus.WaitingToReturn.ToStringValue(),
                    };

                case OrderGroupStatus.HoanTat:
                    return new List<string>
                    {
                        GHN_OrderStatus.Returned.ToStringValue(),
                        GHN_OrderStatus.Delivered.ToStringValue(),
                    };

                case OrderGroupStatus.DaHuy:
                    return new List<string>
                    {
                        GHN_OrderStatus.Cancel.ToStringValue(),
                    };

                case OrderGroupStatus.ThatLacHong:
                    return new List<string>
                    {
                        GHN_OrderStatus.Lost.ToStringValue(),
                        GHN_OrderStatus.Damage.ToStringValue(),
                    };

                case OrderGroupStatus.ChoXacNhan:
                    return new List<string>
                    {
                        GHN_OrderStatus.WaitingConfirm.ToStringValue(),
                    };

                default:
                    return new List<string>()
                    {
                        GHN_OrderStatus.ReadyToPick.ToStringValue(),
                        GHN_OrderStatus.Picking.ToStringValue(),
                        GHN_OrderStatus.MoneyCollectPicking.ToStringValue(),
                        GHN_OrderStatus.Picked.ToStringValue(),
                        GHN_OrderStatus.Sorting.ToStringValue(),
                        GHN_OrderStatus.Storing.ToStringValue(),
                        GHN_OrderStatus.Transporting.ToStringValue(),
                        GHN_OrderStatus.Delivering.ToStringValue(),
                        GHN_OrderStatus.DeliveryFail.ToStringValue(),
                        GHN_OrderStatus.MoneyCollectDelivering.ToStringValue(),
                        GHN_OrderStatus.Return.ToStringValue(),
                        GHN_OrderStatus.Returning.ToStringValue(),
                        GHN_OrderStatus.ReturnFail.ToStringValue(),
                        GHN_OrderStatus.ReturnTransporting.ToStringValue(),
                        GHN_OrderStatus.ReturnSorting.ToStringValue(),
                        GHN_OrderStatus.WaitingToReturn.ToStringValue(),
                        GHN_OrderStatus.Returned.ToStringValue(),
                        GHN_OrderStatus.Delivered.ToStringValue(),
                        GHN_OrderStatus.Cancel.ToStringValue(),
                        GHN_OrderStatus.Lost.ToStringValue(),
                        GHN_OrderStatus.Damage.ToStringValue(),
                    };
            }
        }
    }

    public enum OrderGroupStatus
    {


        [EnumMember(Value = "nhap")]
        Nhap,

        [EnumMember(Value = "cho_ban_giao")]
        ChoBanGiao,

        [EnumMember(Value = "da_ban_giao_dang_giao")]
        DaBanGiaoDangGiao,

        [EnumMember(Value = "da_ban_giao_dang_hoang")]
        DaBanGiaoDangHoangHang,

        [EnumMember(Value = "cho_xac_nhan_giao_lai")]
        ChoXacNhanGiaoLai,

        [EnumMember(Value = "hoan_tat")]
        HoanTat,

        [EnumMember(Value = "da_huy")]
        DaHuy,

        [EnumMember(Value = "that_lac_hong")]
        ThatLacHong,

        [EnumMember(Value = "ChoXacNhan")]
        ChoXacNhan = 99,

        [EnumMember(Value = "TatCa")]
        TatCa = 100,
    }


}
