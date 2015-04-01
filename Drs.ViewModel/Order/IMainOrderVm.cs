using Drs.ViewModel.Shared;

namespace Drs.ViewModel.Order
{
    public interface IMainOrderVm : IUcViewModel
    {
        IUcViewModel BackPrevious { get; set; }
        IUcViewModel SearchNewPhone { get; set; }
        IUcViewModel Franchises { get; set; }
        IUcViewModel ClientsList { get; set; }
        IUcViewModel AddressList { get; set; }
        IUcViewModel OrderSummary { get; set; }
        IUcViewModel OrderPos { get; set; }
        IUcViewModel SendOrder { get; set; }
        int SelectedTab { get; set; }

    }
}
