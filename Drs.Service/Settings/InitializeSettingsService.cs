using Drs.Model.Settings;
using Drs.Repository.Address;
using Drs.Repository.Setting;
using Drs.Service.Configuration;

namespace Drs.Service.Settings
{
    public static class InitializeSettingsService
    {
        public static void FullInitialize()
        {
            InitializeConstants();
            InitializeControls();
        }

        public static void InitializeConstants()
        {
            var service = new SettingService(new SettingRepository());
            var resConstants = service.FindAllSettings();
            var dicSettings = resConstants.DicSettings;
            SettingConfigure.InitConstants(dicSettings);
        }

        public static void InitializeControls()
        {
            var service = new SettingService(new SettingRepository());
            var resControls = service.FindAllControlTitlesByLanguage(SettingsData.Language);
            var lstAddressSetting = resControls.LstControls;
            SettingConfigure.InitControls(lstAddressSetting);
            SettingAddress.Initialize(new AddressRepository());
        }
    }
}
