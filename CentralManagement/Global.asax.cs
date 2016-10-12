using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.Http;
using Drs.Model.Constants;
using Drs.Repository.Catalog;
using Drs.Repository.Log;
using Drs.Service.Settings;


namespace CentralManagement
{
    public class MvcApplication : HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InitializeSettings();
            InitializeMenu();
        }

        private void InitializeMenu()
        {
            try
            {
                using (var repository = new CatalogRepository())
                {
                    WebSharedContext.DicWebMenu = repository.GetWebMenu();
                }
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
            }
        }

        private void InitializeSettings()
        {
            try
            {
                InitializeSettingsService.FullInitialize();
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
            }
            

        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private static bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != null && HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }

    }
}
