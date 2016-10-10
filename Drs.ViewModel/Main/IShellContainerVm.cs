using System.Collections.ObjectModel;
using System.Windows;
using Drs.Model.UiView.Shared;
using Drs.ViewModel.Shared;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace Drs.ViewModel.Main
{
    public interface IShellContainerVm
    {
        IUcViewModel CurrentView { get; set; }
        ReadOnlyDictionary<StatusScreen, IUcViewModel> DictionaryViews { get; set; }
        void ChangeCurrentView(StatusScreen statusScreen, bool bHasToInit, bool bForzeToInit = false, string parameters = null);
        Visibility HeaderVisibility { get; set; }
        IReactiveList<Flyout> Flyouts { get; }
        BootstrapperBase BootStrapper { get; set; }
        bool IsInOrder { get; set; }
        string UserTitle { get; set; }
        void AddOrUpdateFlyouts(IFlyoutBaseVm flyout);
        void Initialize();
    }
}
