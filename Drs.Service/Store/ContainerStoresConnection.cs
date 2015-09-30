using System.Collections.Generic;
using Drs.Model.Store;
using Drs.Repository.Store;

namespace Drs.Service.Store
{
    public static class ContainerStoresConnection
    {
        public static List<StoreConnection> Stores { get; private set; }
        static ContainerStoresConnection()
        {
            using (var repository = new StoreRepository())
            {
                Stores = repository.GetStoreConnection();
            }        
        }
    }
}
