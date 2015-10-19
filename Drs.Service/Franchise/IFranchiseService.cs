using System.Collections.Generic;
using Drs.Model.Franchise;
using Drs.Model.Menu;

namespace Drs.Service.Franchise
{
    public interface IFranchiseService
    {
        IEnumerable<ButtonItemModel> GetFranchiseButtons();
        IEnumerable<SyncFranchiseModel> GetListSyncFiles(string s);
    }
}
