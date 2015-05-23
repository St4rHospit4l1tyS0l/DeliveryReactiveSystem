using System;
using System.Reactive;
using System.Threading.Tasks;
using Drs.Model.Settings;
using Drs.Service.ReactiveDelivery;
using Drs.Service.Settings;
using Drs.ViewModel.SettingSvc;

namespace Drs.ViewModel.Setting
{
    public static class SettingConfigureWs
    {
        
        public async static Task<Unit> Initialize(IReactiveDeliveryClient client)
        {
            using (var settingSvc = new SettingSvcClient())
            {
                var resConstants = await settingSvc.FindAllSettingsAsync();

                if (resConstants.IsSuccess == false)
                    throw new Exception("Se sucitó el siguiente error: " + resConstants.Message);

                var dicSettings = resConstants.DicSettings;

                SettingConfigure.InitConstants(dicSettings);

                var resControls = await settingSvc.FindAllControlTitlesByLanguageAsync(SettingsData.Language);

                if (resControls.IsSuccess == false)
                    throw new Exception("Se sucitó el siguiente error: " + resControls.Message);

                var lstAddressSetting = resControls.LstControls;

                SettingConfigure.InitControls(lstAddressSetting);

            }

            return new Unit();
        }
    }
}
