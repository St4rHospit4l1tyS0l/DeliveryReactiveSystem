using System;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public interface IFlyoutBaseVm : IUcViewModel
    {
        string Header { get; set; }
        Boolean IsOpen { get; set; }
        Position Position { get; set; }
        IReactiveCommand<object> CancelCommand { get; }
        bool IsOpenFinished { get; set; }
    }
}
