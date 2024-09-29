namespace GHSTShipping.Domain.Enums
{
    public enum EnumShopStatus
    {
        WaitingForActive = 0,

        Activated = 1,
    }

    public static class EnumShopStatusExtension
    {
        public static string GetName(this EnumShopStatus input)
        {
            switch (input)
            {
                case EnumShopStatus.Activated:
                    return "Đang hoạt động";
                case EnumShopStatus.WaitingForActive:
                default:
                    return "Chờ xác thực";
            }
        }
    }
}
