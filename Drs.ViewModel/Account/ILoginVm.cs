using System;
using System.Windows;
using Drs.Model.Account;
using Drs.ViewModel.LoginSvc;
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

        event Action<UserInfoModel> UserChanged;
    }
}
