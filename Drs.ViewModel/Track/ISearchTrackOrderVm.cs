using System;
using Drs.Model.Shared;
using Drs.ViewModel.Shared;

namespace Drs.ViewModel.Track
{
    public interface ISearchTrackOrderVm : IUcViewModel
    {
        IAutoCompleteTextVm PhoneSearchVm { get; set; }
        IAutoCompleteTextVm NameSearchVm { get; set; }
        event Action<String> PhoneChanged;
        event Action<long> ClientNameChanged;
    }
}
