using System;

namespace StackExchangeApiTest
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimestamp(this DateTime d)
        {
            var duration = d - new DateTime(1970, 1, 1, 0, 0, 0);

            return (long)duration.TotalSeconds;
        }
    }
}