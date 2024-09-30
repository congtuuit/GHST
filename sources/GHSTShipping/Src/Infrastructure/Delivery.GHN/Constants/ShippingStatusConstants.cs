namespace Delivery.GHN.Constants
{
    public static class ShippingOrderStatusConstants
    {
        /// <summary>
        /// Shipping order has just been created
        /// </summary>
        public const string ReadyToPick = "ready_to_pick";

        /// <summary>
        /// Shipper is coming to pick up the goods
        /// </summary>
        public const string Picking = "picking";

        /// <summary>
        /// Shipping order has been cancelled
        /// </summary>
        public const string Cancel = "cancel";

        /// <summary>
        /// Shipper is interacting with the seller
        /// </summary>
        public const string MoneyCollectPicking = "money_collect_picking";

        /// <summary>
        /// Shipper has picked the goods
        /// </summary>
        public const string Picked = "picked";

        /// <summary>
        /// The goods have been shipped to GHN sorting hub
        /// </summary>
        public const string Storing = "storing";

        /// <summary>
        /// The goods are being rotated
        /// </summary>
        public const string Transporting = "transporting";

        /// <summary>
        /// The goods are being classified (at the warehouse classification)
        /// </summary>
        public const string Sorting = "sorting";

        /// <summary>
        /// Shipper is delivering the goods to customer
        /// </summary>
        public const string Delivering = "delivering";

        /// <summary>
        /// Shipper is interacting with the buyer
        /// </summary>
        public const string MoneyCollectDelivering = "money_collect_delivering";

        /// <summary>
        /// The goods have been delivered to customer
        /// </summary>
        public const string Delivered = "delivered";

        /// <summary>
        /// The goods haven't been delivered to customer
        /// </summary>
        public const string DeliveryFail = "delivery_fail";

        /// <summary>
        /// The goods are pending delivery (can be delivered within 24/48h)
        /// </summary>
        public const string WaitingToReturn = "waiting_to_return";

        /// <summary>
        /// The goods are waiting to return to seller/merchant after 3 times delivery failed
        /// </summary>
        public const string Return = "return";

        /// <summary>
        /// The goods are being rotated
        /// </summary>
        public const string ReturnTransporting = "return_transporting";

        /// <summary>
        /// The goods are being classified (at the warehouse classification)
        /// </summary>
        public const string ReturnSorting = "return_sorting";

        /// <summary>
        /// The shipper is returning for seller
        /// </summary>
        public const string Returning = "returning";

        /// <summary>
        /// The returning has failed
        /// </summary>
        public const string ReturnFail = "return_fail";

        /// <summary>
        /// The goods have been returned to seller/merchant
        /// </summary>
        public const string Returned = "returned";

        /// <summary>
        /// The goods exception handling (cases that go against the process)
        /// </summary>
        public const string Exception = "exception";

        /// <summary>
        /// Damaged goods
        /// </summary>
        public const string Damage = "damage";

        /// <summary>
        /// The goods are lost
        /// </summary>
        public const string Lost = "lost";
    }
}
