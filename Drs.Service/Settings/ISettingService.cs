using Drs.Model.Settings;

namespace Drs.Service.Settings
{
    public interface ISettingService
    {
        ResponseMessageSetting FindAllSettings();
        ResponseMessageSetting FindAllControlTitlesByLanguage(int iLanguageId);
    }
}
