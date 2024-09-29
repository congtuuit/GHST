using System.Collections.Generic;

namespace GHSTShipping.Domain.Enums
{
    public enum EnumSupplier
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
        public static string GetCode(this EnumSupplier input)
        {
            switch (input)
            {
                case EnumSupplier.GHN:
                default:
                    return EnumSupplierConstants.GHN;
                case EnumSupplier.SPX:
                    return EnumSupplierConstants.SHOPEE_EXPRESS;
                case EnumSupplier.JT:
                    return EnumSupplierConstants.J_T;
                case EnumSupplier.Best:
                    return EnumSupplierConstants.BEST;
                case EnumSupplier.Viettel:
                    return EnumSupplierConstants.VIETTEL;
                case EnumSupplier.GHTK:
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

