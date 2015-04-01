using System.ServiceModel;
using Drs.Model.Settings;

namespace ManagementCallCenter.Setting
{
    [ServiceContract]
    public interface ISettingSvc
    {
        [OperationContract]
        ResponseMessageSetting FindAllSettings();

        [OperationContract]
        ResponseMessageSetting FindAllControlTitlesByLanguage(int iLanguageId);
    }
}
