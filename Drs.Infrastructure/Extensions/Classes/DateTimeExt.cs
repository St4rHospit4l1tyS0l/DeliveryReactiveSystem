using System;

namespace Drs.Infrastructure.Extensions.Classes
{
    public static class DateTimeExt
    {
        public static long ToDateShort(this DateTime value)
        {
            return value.Day + value.Month*100 + value.Year*10000;
        }

        public static DateTime RecurrenceType(this DateTime value, string comodin)
        {
            switch (comodin)
            {
                case "M":
                    return value.AddMonths(-1);

                case "S":
                    return  value.AddMonths(-6);

            }

            return value;
        }
    }
}
