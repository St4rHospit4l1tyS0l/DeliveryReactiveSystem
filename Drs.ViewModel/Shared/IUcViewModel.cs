using Drs.Model.Shared;
using Drs.ViewModel.Main;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public interface IUcViewModel : IReactiveObject, IWizItem
    {
        bool IsInitialize { get; set; }
        IShellContainerVm ShellContainerVm { get; set; }
        bool Initialize(bool bForceToInit = false, string parameters = null);
        ResponseMessage OnViewSelected(int iSelectedTab);
    }
}
