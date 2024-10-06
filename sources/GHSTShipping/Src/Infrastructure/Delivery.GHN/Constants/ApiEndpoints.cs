namespace Delivery.GHN.Constants
{
    public static class ApiEndpoints
    {
        public const string GET_ALL_SHOPS = "/shiip/public-api/v2/shop/all";

        public const string REGISTER_SHOP = "/shiip/public-api/v2/shop/register";

        public const string GET_FEE_SERVICE = "/shiip/public-api/v2/shipping-order/available-services";

        public const string CALCULATE_FEE = "/shiip/public-api/v2/shipping-order/fee";

        public const string FETCH_FEE_OF_ORDER = "/shiip/public-api/v2/shipping-order/soc";

        public const string CREATE_DELIVERY_ORDER = "/shiip/public-api/v2/shipping-order/create";

        public const string ORDER_DETAIL = "/shiip/public-api/v2/shipping-order/detail";

        public const string CREATE_DRAFT_ORDER = "/shiip/public-api/v2/shipping-order/preview";

        public const string GET_PROVINCE = "/shiip/public-api/master-data/province";

        public const string GET_DISTRICT = "/shiip/public-api/v2/master-data/districts";

        public const string GET_WARD = "/shiip/public-api/master-data/ward";

        public const string PICK_SHIFT = "/shiip/public-api/v2/shift/date";

        public const string CANCEL_SHIFT = "/shiip/public-api/v2/switch-status/cancel";

    }
}
