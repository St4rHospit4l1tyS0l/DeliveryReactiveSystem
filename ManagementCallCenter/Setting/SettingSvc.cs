using System;
using Drs.Model.Settings;
using Drs.Service.Settings;

namespace ManagementCallCenter.Setting
{

    public class SettingSvc : ISettingSvc
    {
        private readonly ISettingService _serviceSetting;

        public SettingSvc(ISettingService service)
        {
            _serviceSetting = service;
        }

        public ResponseMessageSetting FindAllSettings()
        {
            try
            {
                //Thread.Sleep(4000);
                return _serviceSetting.FindAllSettings();
            }
            catch (Exception ex)
            {
                return new ResponseMessageSetting { IsSuccess = false, Message = ex.Message };
            }
        }

        public ResponseMessageSetting FindAllControlTitlesByLanguage(int iLanguageId)
        {
            try
            {
                //Thread.Sleep(4000);
                return _serviceSetting.FindAllControlTitlesByLanguage(iLanguageId);
            }
            catch (Exception ex)
            {
                return new ResponseMessageSetting { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
