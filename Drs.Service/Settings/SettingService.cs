using System;
using Drs.Model.Settings;
using Drs.Repository.Setting;

namespace Drs.Service.Settings
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _repository;
        public SettingService(ISettingRepository repository)
        {
            _repository = repository;
        }


        public ResponseMessageSetting FindAllSettings()
        {
            using (_repository)
            {
                return new ResponseMessageSetting
                {
                    DicSettings = _repository.FindAll(),
                    IsSuccess = true,
                    Message = String.Empty
                };
            }
        }

        public ResponseMessageSetting FindAllControlTitlesByLanguage(int iLanguageId)
        {
            using (_repository)
            {
                return new ResponseMessageSetting
                {
                    LstControls = _repository.FindAllControlTitlesByLanguage(iLanguageId),
                    IsSuccess = true,
                    Message = String.Empty
                };
            }
        }
    }
}
