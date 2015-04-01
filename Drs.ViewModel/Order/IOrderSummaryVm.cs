using Drs.Model.Address;
using Drs.Model.Order;
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

        //string FullName { get; set; }
        //string CheckInfo { get; set; }
        //string CheckTotal { get; set; }
        //string PosCheckMsgErr { get; set; }
    }
}
