using System.Collections.Generic;
using Drs.Model.Menu;

namespace Drs.Service.Order
{
    public interface IFranchiseService
    {
        IEnumerable<ButtonItemModel> GetFranchiseButtons();
    }
}
