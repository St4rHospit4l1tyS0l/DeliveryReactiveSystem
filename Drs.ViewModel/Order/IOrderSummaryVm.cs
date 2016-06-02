using Drs.Model.Address;
using Drs.Model.Order;
using Drs.Model.Store;
using Drs.Service.Client;
using Drs.ViewModel.Shared;

namespace Drs.ViewModel.Order
{
    public interface IOrderSummaryVm : IUcViewModel
    {
        IMainOrderService OrderService { get; set; }
        void OnPhoneChanged(PhoneModel modelPhone);
        void OnFranchiseChanged(FranchiseInfoModel franchiseInfo);
        void OnClientSelected(ClientInfoGrid clientInfo);
        void OnAddressSelected(AddressInfoGrid info);
        void OnPosOrderChanged(PosCheck obj);
        void OnStoreSelected(StoreModel obj, string sMsg, bool pendingStatus);
    }
}
