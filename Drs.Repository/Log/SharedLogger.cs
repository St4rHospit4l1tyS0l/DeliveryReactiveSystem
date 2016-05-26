using System;
using System.Web;
using System.Web.Script.Serialization;
using Drs.Infrastructure.Logging;
using Drs.Repository.Entities;
using Drs.Repository.Resources;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Drs.Repository.Log
{
    public class SharedLogger
    {

        public static void LogError(Exception ex, params object[] arrVal)
        {
            try
            {
                dynamic username = GetUser();
                dynamic modelExcep = new ExceptionLog();

                modelExcep.MsgException = ex.Message;
                modelExcep.ExceptionLogUid = Guid.NewGuid();
                modelExcep.InnerException = InternalLogger.GetInnerExceptions(ex);
                modelExcep.ParamsValues = InternalLogger.GetSerializedValues(arrVal);
                modelExcep.StackTrace = ex.StackTrace;
                modelExcep.Timestamp = DateTime.Now;
                modelExcep.Username = username;
                SaveLogToDb(modelExcep);
            }
            catch (Exception)
            {
                return;
            }
        }

        public static void LogErrorToFile(Exception ex, params object[] arrVal)
        {
            try
            {
                dynamic username = GetUser();
                dynamic modelExcep = new ExceptionLog();

                modelExcep.MsgException = ex.Message;
                modelExcep.ExceptionLogUid = Guid.NewGuid();
                modelExcep.InnerException = InternalLogger.GetInnerExceptions(ex);
                modelExcep.ParamsValues = InternalLogger.GetSerializedValues(arrVal);
                modelExcep.StackTrace = ex.StackTrace;
                modelExcep.Timestamp = DateTime.Now;
                modelExcep.Username = username;
                SaveLogToFile(modelExcep);
            }
            catch (Exception)
            {
                return;
            }
        }

        private static void SaveLogToDb(ExceptionLog modelExcep)
        {
            try
            {
                using (var dbConn = new LogCallCenterEntities())
                {
                    dbConn.ExceptionLog.Add(modelExcep);
                    dbConn.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                modelExcep.MsgException = String.Format("Error: {0} - Second Error: {1} -ST- {2}", modelExcep.MsgException, ex.Message, ex.StackTrace);
                SaveLogToFile(modelExcep);
            }
        }

        private static void SaveLogToFile(ExceptionLog exceptionLog)
        {
            try
            {
                LogManager.GetLogger("ERROR_LOGGER").Fatal(GetSerializedValue(exceptionLog));
            }
            catch (Exception)
            {
                return;
            }
        }

        private static string GetSerializedValue(ExceptionLog exceptionLog)
        {
            try
            {
                return new JavaScriptSerializer().Serialize(exceptionLog);
            }
            catch (Exception)
            {
                return string.Format(ResShared.ERROR_RECURSION, "GetSerializedValue");
            }
        }

        private static string GetUser()
        {
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.User == null)
                {
                    return string.Empty;
                }

                return HttpContext.Current.User.Identity.Name;

            }
            catch (Exception)
            {
                return ResShared.ERROR_NOUSER_FROMCONTEXT;
            }
        }
    }
}

