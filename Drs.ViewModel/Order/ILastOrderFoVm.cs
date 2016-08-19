using System.Reactive;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public interface ILastOrderFoVm : IFlyoutBaseVm
    {
        void ProcessPhone(ListItemModel model);
        PropagateOrderModel PropagateOrder { get; set; }
        IReactiveCommand<Unit> DoLastOrderCommand { get; set; }
        IReactiveCommand<Unit> DoEditLastOrderCommand { get; set; }
        bool IsGettingSelectedOrder { get; set; }
    }
}
