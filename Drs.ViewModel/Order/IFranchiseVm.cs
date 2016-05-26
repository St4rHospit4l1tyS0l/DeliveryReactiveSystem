using System;
using System.Windows.Media;
using Drs.Model.Order;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public interface IFranchiseVm : IUcViewModel
    {
        IReactiveCommand<object> ExecuteCommand { get; }
        String Title { get; set; }
        ImageSource ItemLogo { get; set; }
        Brush ItemBackgroundColor { get; set; }
        Brush ItemBackgroundOverColor{ get; set; }
        Brush ItemBackgroundPressedColor { get; set; }
        string Code { get; set; }
        bool IsChecked { get; set; }
        dynamic DataInfo { get; set; }
        string StoresCoverage { get; set; }
        string LastConfig { get; set; }
    }
}
