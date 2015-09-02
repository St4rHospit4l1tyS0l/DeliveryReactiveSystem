using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Drs.Model.Settings;
using Drs.Repository.Log;
using Drs.Repository.Setting;
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
                var service = new SettingService(new SettingRepository());
                var resConstants = service.FindAllSettings();
                var dicSettings = resConstants.DicSettings;
                SettingConfigure.InitConstants(dicSettings);
                service = new SettingService(new SettingRepository());
                var resControls = service.FindAllControlTitlesByLanguage(SettingsData.Language);
                var lstAddressSetting = resControls.LstControls;
                SettingConfigure.InitControls(lstAddressSetting);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
            }
            

        }
    }
}
