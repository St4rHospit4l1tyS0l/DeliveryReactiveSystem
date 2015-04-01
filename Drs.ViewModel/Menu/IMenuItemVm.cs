using System;
using System.Windows.Media;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Menu
{
    public interface IMenuItemVm : IUcViewModel
    {
        IReactiveCommand<object> ExecuteCommand { get; }
        String Title { get; set; }
        ImageSource MenuItemLogo { get; set; }
        Brush MenuItemBackgroundColor { get; set; }
        Brush MenuItemBackgroundOverColor{ get; set; }
        Brush MenuItemBackgroundPressedColor { get; set; }
    }
}
