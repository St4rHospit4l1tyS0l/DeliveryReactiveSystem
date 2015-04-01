using System;
using System.Reactive.Linq;
using Drs.Model.UiView.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public class BackPreviousVm : UcViewModelBase, IBackPreviousVm
    {
        public BackPreviousVm()
        {
            BackCommand = ReactiveCommand.Create(Observable.Return(true));
            BackCommand.Subscribe(_ => ShellContainerVm.ChangeCurrentView(StatusScreen.ShMenu, true));
        }

        public IReactiveCommand<object> BackCommand { get; private set; }
    }
}
