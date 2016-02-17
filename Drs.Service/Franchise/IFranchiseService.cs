using System.Collections.Generic;
using Drs.Model.Franchise;
using Drs.Model.Menu;
using Drs.Model.Shared;

namespace Drs.Service.Franchise
{
    public interface IFranchiseService
    {
        IEnumerable<ButtonItemModel> GetFranchiseButtons();
        IEnumerable<SyncFranchiseModel> GetListSyncFiles(string s);
        IEnumerable<OptionModel> LstFranchise();

    }
}
