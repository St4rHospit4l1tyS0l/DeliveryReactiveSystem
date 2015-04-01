using System;
using System.Collections.Generic;
using Drs.Model.Menu;

namespace Drs.Repository.Order
{
    public interface IFranchiseRepository : IDisposable
    {
        IEnumerable<ButtonItemModel> GetFranchiseButtons();
    }
}
