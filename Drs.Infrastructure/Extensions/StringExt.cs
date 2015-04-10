using System;

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
    }
}
