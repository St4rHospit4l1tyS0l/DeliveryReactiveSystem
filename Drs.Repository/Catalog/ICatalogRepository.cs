using System;
using System.Collections.Generic;
using Drs.Model.Shared;

namespace Drs.Repository.Catalog
{
    public interface ICatalogRepository : IDisposable
    {
        IList<ItemCatalog> GetPayments();
        IList<ItemCatalog> GetDeliveryStatus();
        IList<ItemCatalog> GetStores();
    }
}
