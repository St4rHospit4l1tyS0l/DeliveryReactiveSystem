using Drs.Model.Shared;
using Drs.ViewModel.Shared;

namespace Drs.ViewModel.Order
{
    public interface ILastOrderFoVm : IFlyoutBaseVm
    {
        void ProcessPhone(ListItemModel model);
    }
}
