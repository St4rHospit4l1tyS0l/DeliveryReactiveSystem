using System;
using Drs.Model.Address;
using Drs.Model.Order;
using Drs.Model.Store;

namespace Drs.Service.Client
{
    public interface IStoreAddressService
    {
        void OnAddressSelected(AddressInfoGrid obj);
        IMainOrderService OrderService { get; set; }

        event Action<StoreModel, string> StoreSelected;

        void OnFranchiseChanged(FranchiseInfoModel obj);
    }
}
