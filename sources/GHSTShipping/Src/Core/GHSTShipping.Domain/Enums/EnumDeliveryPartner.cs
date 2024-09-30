using System.Collections.Generic;

namespace GHSTShipping.Domain.Enums
{
    public enum EnumDeliveryPartner
    {
        None = 0,
        GHN = 1,
        SPX = 2,
        JT = 3,
        Best = 4,
        Viettel = 5,
        GHTK = 6,
    }

    public static class EnumSupplierConstants
    {
        public const string GHN = "GHN";
        public const string SHOPEE_EXPRESS = "SHOPEE EXPRESS";
        public const string J_T = "J&T";
        public const string BEST = "Best";
        public const string VIETTEL = "Viettel";
        public const string GHTK = "GHTK";
    }

    public static class EnumSupplierExtension
    {
        public static string GetCode(this EnumDeliveryPartner input)
        {
            switch (input)
            {
                case EnumDeliveryPartner.GHN:
                default:
                    return EnumSupplierConstants.GHN;
                case EnumDeliveryPartner.SPX:
                    return EnumSupplierConstants.SHOPEE_EXPRESS;
                case EnumDeliveryPartner.JT:
                    return EnumSupplierConstants.J_T;
                case EnumDeliveryPartner.Best:
                    return EnumSupplierConstants.BEST;
                case EnumDeliveryPartner.Viettel:
                    return EnumSupplierConstants.VIETTEL;
                case EnumDeliveryPartner.GHTK:
                    return EnumSupplierConstants.GHTK;
            }
        }

        public static List<string> GetSuppliers()
        {
            return new List<string>()
            {
                EnumSupplierConstants.GHN,
                EnumSupplierConstants.SHOPEE_EXPRESS,
                EnumSupplierConstants.J_T,
                EnumSupplierConstants.BEST,
                EnumSupplierConstants.VIETTEL,
                EnumSupplierConstants.GHTK
            };
        }
    }
}

