using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Drs.Model.Settings;
using Drs.Repository.Address;
using Drs.Repository.Log;
using Drs.Repository.Setting;
using Drs.Service.Configuration;
using Drs.Service.Settings;

namespace CentralManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InitializeSettings();

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
    }
}
