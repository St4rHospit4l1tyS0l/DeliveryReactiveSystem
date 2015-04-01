using System.Windows;
using Drs.Model.Shared;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Account
{
    public interface ILoginVm : IUcViewModel
    {
        string UserName { get; set; }
        string Message { get; set; }
        IReactiveCommand<ResponseMessage> SignInCommand { get; set; }
        Visibility IsSignInVisible { get; set; }

    }
}
