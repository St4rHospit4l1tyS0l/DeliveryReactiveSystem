using System;
using System.Reactive;
using Drs.Model.Address;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public interface IAddressListVm : IUcViewModel
    {
        IUpsertAddressFoVm UpsertAddress { get; set; }
        IReactiveCommand<Unit> Add { get; set; }
        IReactiveCommand<Unit> Edit { get; set; }
        IReactiveCommand<Unit> Remove { get; set; }
        AddressInfoGrid AddressSelection { get; set; }
        IReactiveCommand<Unit> RetrySaveItem { get; set; }
        Func<ClientFlags.ValidateOrder, ResponseMessage> ValidateModel { get; set; }
        //void SetLstAddress(IReactiveList<AddressInfoGrid> lstDataInfo);
        void OnAddressChanged(AddressInfoGrid obj);
        void ProcessPhone(ListItemModel model);

        event Action<AddressInfoGrid> ItemSelected;
        AddressControlSetting Setting { get; set; }
        void SetOrderModel(Func<OrderModel> orderModel);
    }
}
