using System;

namespace GHSTShipping.Application.Helpers
{
    public static class DateTimeHelper
    {
        public static long ConvertToUnixTimestamp(DateTime dateTime)
        {
            // Ensure the DateTime is in UTC
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime.ToUniversalTime());
            return dateTimeOffset.ToUnixTimeSeconds();
        }

        public static long ConvertToUnixTimestamp(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToUnixTimeSeconds();
        }
    }
}
