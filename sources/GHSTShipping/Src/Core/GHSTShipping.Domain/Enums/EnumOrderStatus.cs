using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHSTShipping.Domain.Enums
{
    public enum EnumOrderStatus
    {
        DRAFT,
        WAIT_CONFIRM,
        DELIVERING,
        RETURN,
        WAIT_CONFIRM_DELIVERY,
        COMPLETED = 200,
        CANCEL,
        LOST
    }

    public static class EnumOrderStatusConstants
    {
        public const string DRAFT = "DRAFT";
        public const string WAIT_CONFIRM = "WAIT_CONFIRM";
        public const string DELIVERING = "DELIVERING";
        public const string RETURN = "RETURN";
        public const string WAIT_CONFIRM_DELIVERY = "WAIT_CONFIRM_DELIVERY";
        public const string COMPLETED = "COMPLETED";
        public const string CANCEL = "CANCEL";
        public const string LOST = "LOST";
    }
}
