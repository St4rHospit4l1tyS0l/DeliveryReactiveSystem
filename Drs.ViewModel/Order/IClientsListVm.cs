using System;
using System.Reactive;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public interface IClientsListVm : IUcViewModel
    {
        IUpsertClientFoVm UpsertClient { get; set; }
        IReactiveCommand<Unit> AddClient { get; set; }
        IReactiveCommand<Unit> EditClient { get; set; }
        IReactiveCommand<Unit> RemoveClient { get; set; }
        ClientInfoGrid ClientSelection { get; set; }
        IReactiveCommand<Unit> RetrySaveClient { get; set; }
        Func<ClientFlags.ValidateOrder, ResponseMessage> ValidateModel { get; set; }
        //void SetLstClient(IReactiveList<ClientInfoGrid> lstClientInfo);
        void OnClientChanged(ClientInfoGrid obj);
        void ProcessPhone(ListItemModel model);
        
        event Action<ClientInfoGrid> ClientSelected;
        void SetOrderModel(Func<OrderModel> orderModel);
    }
}
