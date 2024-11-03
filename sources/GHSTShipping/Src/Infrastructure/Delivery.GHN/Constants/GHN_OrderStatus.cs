using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Delivery.GHN.Constants
{
    [JsonConverter(typeof(JsonStringEnumConverter))] // Enables JSON conversion with custom strings
    public enum GHN_OrderStatus
    {
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
}
