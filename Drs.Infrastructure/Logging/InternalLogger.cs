using System;
using System.Web.Script.Serialization;
using Drs.Infrastructure.Resources;
using log4net;

namespace Drs.Infrastructure.Logging
{
    public static class InternalLogger
    {
        private static readonly log4net.ILog Log = LogManager.GetLogger(typeof(InternalLogger));


        public static void WriteException(Exception ex, params object[] arrVal)
        {
            var msgException = ex.Message;
            var exceptionLogUid = Guid.NewGuid();
            var innerException = GetInnerExceptions(ex);
            var paramsValues = GetSerializedValues(arrVal);
            var stackTrace = ex.StackTrace;
            Log.ErrorFormat("Uid: {0} | Msg: {1} | InExc: {2} | Params: {3} | St: {4}", exceptionLogUid, msgException, innerException, paramsValues, stackTrace);
        }

        public static string GetInnerExceptions(Exception exception)
        {
            try
            {
                if (exception.InnerException == null)
                {
                    return "|EOE|";
                }
                return string.Format("|{0}|{1}", exception.InnerException.Message, GetInnerExceptions(exception.InnerException));
            }
            catch (Exception)
            {
                return string.Format(ResShared.ERROR_RECURSION, "GetInnerExceptions");
            }
        }


        public static string GetSerializedValues(object[] arrVal)
        {
            try
            {
                dynamic serializer = new JavaScriptSerializer();
                return serializer.Serialize(arrVal);
            }
            catch (Exception)
            {
                return string.Format(ResShared.ERROR_RECURSION, "GetSerializedValues");
            }
        }

    }
}
