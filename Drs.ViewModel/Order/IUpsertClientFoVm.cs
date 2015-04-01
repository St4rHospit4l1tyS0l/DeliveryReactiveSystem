using System;
using System.Reactive;
using Drs.Model.Order;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public interface IUpsertClientFoVm : IFlyoutBaseVm
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Company { get; set; }
        int? CompanyId { get; set; }
        PhoneModel SecondPhone { get; set; }
        DateTime? BirthDate { get; set; }
        IReactiveCommand<Unit> UpsertCommand { get; }
        IAutoCompleteTextVm CompanySearchVm { get; }
        IAutoCompleteTextVm SecondPhoneVm { get; }
        void Clean();
        void Fill(ClientInfoGrid clInfo);
    }
}
