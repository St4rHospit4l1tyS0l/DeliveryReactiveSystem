using System;
using System.Collections.Generic;
using Drs.Model.Address;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Model.Store;

namespace Drs.Service.Client
{
    public interface IStoreAddressService
    {
        void OnAddressSelected(AddressInfoGrid obj);
        IMainOrderService OrderService { get; set; }

        event Action<StoreModel, string> StoreSelected;

        event Action<List<ItemCatalog>> StoresReceivedByAddress;

        void OnFranchiseChanged(FranchiseInfoModel obj);
        void OnChangeStore(ItemCatalog item, bool bIsLastStore);
        void OnUndoPickUpInStore();
    }
}
