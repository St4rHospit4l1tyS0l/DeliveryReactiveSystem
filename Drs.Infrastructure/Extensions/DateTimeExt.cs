using System;
using System.Globalization;

namespace Drs.Infrastructure.Extensions
{
    public static class DateTimeExt
    {
        public static DateTime FloorDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static DateTime CeilDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day).AddDays(1);
        }

        public static DateTime ExtractDateOrDefault(this string sDate, DateTime defaultDate)
        {
            if (String.IsNullOrEmpty(sDate))
                return defaultDate;
            DateTime dt;
            return !DateTime.TryParseExact(sDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dt) ? defaultDate : dt;
        }

    }
}
