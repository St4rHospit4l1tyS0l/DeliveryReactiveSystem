using System;
using System.Text;

namespace Drs.Infrastructure.Extensions
{
    public static class StringExt
    {
        public static string SubstringMax(this string value, int len)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;

            return value.Length <= len ? value : value.Substring(0, len);
        }

        public static DateTime ToDateTimeSafe(this string value, DateTime? dtDefault = null)
        {
            if (dtDefault == null)
                dtDefault = DateTime.Now.AddDays(1).AddMilliseconds(-1);

            if (String.IsNullOrWhiteSpace(value))
                return dtDefault.Value;

            try
            {
                return DateTime.Parse(value);
            }
            catch (Exception)
            {
                return dtDefault.Value;
                throw;
            }

        }

        public static string ToVersion(this string value, int nChars, int nSplits, char splitChar)
        {
            if (String.IsNullOrWhiteSpace(value))
                return value;

            var i = 1;
            var max = (nSplits * nChars) + 2;
            var sb = new StringBuilder();
            foreach (var c in value)
            {
                sb.Append(c);
                if (i++ % nChars == 0 && i < max)
                    sb.Append(splitChar);
            }

            return sb.ToString();
        }

    }
}
