using Drs.ViewModel.Shared;

namespace Drs.ViewModel.Order
{
    public interface ISearchNewPhoneVm : IUcViewModel
    {
        IAutoCompleteTextVm PhoneSearchVm{ get; set; }
    }
}
