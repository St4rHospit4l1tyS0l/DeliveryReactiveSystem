using System;
using System.Data.Entity.Validation;
using System.Text;
using System.Linq;
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
            var innerException = GetInternalErrors(ex);
            var paramsValues = GetSerializedValues(arrVal);
            var stackTrace = ex.StackTrace;
            Log.ErrorFormat("Uid: {0} | Msg: {1} | InExc: {2} | Params: {3} | St: {4}", exceptionLogUid, msgException, innerException, paramsValues, stackTrace);
        }

        public static string GetInnerExceptions(Exception exception, int iVal = 0)
        {
            try
            {
                if (iVal == 10)
                {
                    return "|EOECNT|";
                }

                if (exception.InnerException == null)
                {
                    return "|EOE|";
                }
                return string.Format("|{0}|{1}", exception.InnerException.Message, GetInnerExceptions(exception.InnerException, iVal+1));
            }
            catch (Exception ex)
            {
                return string.Format(ResShared.ERROR_RECURSION, "GetInnerExceptions " + ex.Message);
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

        public static string GetInnerErrorValidations(Exception exception)
        {
            var validationErrors = exception as DbEntityValidationException;

            if (validationErrors == null)
                return String.Empty;

            var sb = new StringBuilder();
            var iVal = 0;
            foreach (var validationError in validationErrors.EntityValidationErrors)
            {
                if (validationError.ValidationErrors.Count > 0)
                    sb.Append(String.Format("Error {0}: {1}{2}", ++iVal,
                        String.Join(",", validationError.ValidationErrors.Select(e => e.ErrorMessage).ToArray()),
                        Environment.NewLine));
            }

            return sb.ToString();
        }

        public static string GetInternalErrors(Exception ex)
        {
            return GetInnerExceptions(ex) + GetInnerErrorValidations(ex);
        }
    }
}
