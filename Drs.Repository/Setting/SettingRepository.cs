using System.Collections.Generic;
using System.Linq;
using Drs.Model.Settings;
using Drs.Repository.Shared;

namespace Drs.Repository.Setting
{
    public class SettingRepository : BaseOneRepository, ISettingRepository
    {
        public IDictionary<string, string> FindAll()
        {
            return DbEntities.Setting.Select(e => new { e.GroupName, e.KeyName, e.Value })
                .ToDictionary(e => e.GroupName + SettingsData.Constants.SETTING_SEPARATOR + e.KeyName, e => e.Value);
        }

        public IList<ControlTitleModel> FindAllControlTitlesByLanguage(int iLanguageId)
        {
            return DbEntities.ControlTitle.Where(e => e.LanguageId == iLanguageId)
                .Select(e => new ControlTitleModel
                {
                    Container = e.Container,
                    ControlName = e.ControlName,
                    Name = e.Name,
                    IsEnabled = e.IsEnabled
                }).ToList();
        }
    }
}
