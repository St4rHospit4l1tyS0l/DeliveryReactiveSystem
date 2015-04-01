using System;
using System.Collections.Generic;
using Drs.Model.Settings;

namespace Drs.Repository.Setting
{
    public interface ISettingRepository : IDisposable
    {
        IDictionary<string, string> FindAll();
        IList<ControlTitleModel> FindAllControlTitlesByLanguage(int iLanguageId);

    }
}