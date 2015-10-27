using System;
using System.Reactive;
using System.Windows;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Service.Client;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public interface ISendOrderVm : IUcViewModel
    {
        void OnPosOrderChanged(PosCheck posCheck);
        Func<ClientFlags.ValidateOrder, ResponseMessage> ValidateModel { get; set; }
        IReactiveCommand<Unit> SendOrderToStore { get; set; }
        IMainOrderService OrderService { get; set; }
        string SendOrderTitleBtn { get; set; }
        Visibility IsSending { get; set; }
        Visibility IsReadyToSend { get; set; }
        string EventsMsg { get; set; }
        Visibility HasError { get; set; }
        Visibility HasSuccess { get; set; }
        string ErrorMsg { get; set; }
        string SuccessMsg { get; set; }

        event Action EndOrder;
        
        event Action<ItemCatalog> ChangeStore;

        void OnSendOrderToStoreStatusChanged(OrderModelDto model);
        bool ImmediateDelivery { get; set; }
        bool FutureDelivery { get; set; }
        Visibility FutureOrderVisibility { get; set; }
        String ExtraNotes { get; set; }
        String PromiseTimeTx { get; set; }
        DateTime MinDateTime { get; set; }
        String CultureSystem { get; set; }
        ItemCatalog Payment { get; set; }

    }
}
