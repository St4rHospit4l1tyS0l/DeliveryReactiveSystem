using System;
using System.Collections.Generic;
using Drs.Model.Menu;
using Drs.Repository.Shared;

namespace Drs.Repository.Order
{
    public interface IFranchiseRepository : IBaseOneRepository, IDisposable
    {
        IEnumerable<ButtonItemModel> GetFranchiseButtons();
    }
}
