using System;
using System.Reflection;

namespace Drs.Infrastructure.Logging
{
    public static class AlohaExt
    {
        public static string AlohaError(this Exception ex)
        {
            try
            {
                PropertyInfo propInfo = ex.GetType().GetProperty("HResult", BindingFlags.Instance | BindingFlags.NonPublic);
                Int32 hresult = Convert.ToInt32(propInfo.GetValue(ex, null));

                return ((hresult >> 16) & 0x07ff) != 0x06
                    ? string.Format("COM error: " + ex.Message)
                    : string.Format("Aloha returned error code {0}", hresult & 0xFFF);

            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
}
