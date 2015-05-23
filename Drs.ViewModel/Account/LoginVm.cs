using System;
using System.Windows;
using System.Windows.Controls;
using Drs.Infrastructure.Crypto;
using Drs.Model.Account;
using Drs.Model.Constants;
using Drs.Model.Shared;
using Drs.Model.UiView.Shared;
using Drs.Resources.Network;
using Drs.ViewModel.LoginSvc;
using Drs.ViewModel.Shared;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace Drs.ViewModel.Account
{
    public class LoginVm : UcViewModelBase, ILoginVm
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { this.RaiseAndSetIfChanged(ref _userName, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { this.RaiseAndSetIfChanged(ref _message, value); }
        }


        private Visibility _isSignInVisible;
        public Visibility IsSignInVisible
        {
            get { return _isSignInVisible; }
            set { this.RaiseAndSetIfChanged(ref _isSignInVisible, value); }
        }

        private Visibility _isOnSignIn;
        public Visibility IsOnSignIn
        {
            get { return _isOnSignIn; }
            set { this.RaiseAndSetIfChanged(ref _isOnSignIn, value); }
        }


        private Visibility _isOnWaiting;
        public Visibility IsOnWaiting
        {
            get { return _isOnWaiting; }
            set { this.RaiseAndSetIfChanged(ref _isOnWaiting, value); }
        }

        public IReactiveCommand<ResponseMessage> SignInCommand { get; set; }

        public LoginVm()
        {
            InitializeProperties();
            InitializeCommands();
            //canSignIn.Subscribe(x =)
        }

        private void InitializeCommands()
        {
            var canSignIn = this.WhenAny(vm => vm.UserName, s => !String.IsNullOrWhiteSpace(s.Value));
            canSignIn.Subscribe(x => IsSignInVisible = x ? Visibility.Visible : Visibility.Hidden);
            
            SignInCommand = ReactiveCommand.CreateAsyncTask(canSignIn, async x =>
            {
                IsOnSignIn = Visibility.Collapsed;
                IsOnWaiting = Visibility.Visible;
                var pass = (PasswordBox)x;
                using (var loginSvc = new LoginSvcClient())
                {
                    try
                    {
                        var response =
                            await
                                loginSvc.LoginAsync(new LoginModel
                                {
                                    Password = Cypher.Encrypt(pass.Password),
                                    Username = Cypher.Encrypt(UserName)
                                });
                        return response;
                    }
                    catch (Exception ex)
                    {
                        return new ResponseMessage { IsSuccess = false, Message = ResNetwork.ERROR_NETWORK_DOWN + ex.Message};
                    }
                    finally
                    {
                        pass.Password = String.Empty;
                        IsOnSignIn = Visibility.Visible;
                        IsOnWaiting = Visibility.Collapsed;
                        MessageBus.Current.SendMessage(String.Empty, SharedMessageConstants.LOGIN_FOCUS_USERNAME);
                    }
                }
            });


            SignInCommand.Subscribe(x =>
            {
                if (x.IsSuccess)
                {
                    var bForceToInit = CurrentUserSettings.UserInfo.Username == null ||
                                        CurrentUserSettings.UserInfo.Username != UserName;

                    CurrentUserSettings.UserInfo.Username = UserName; 
                    ShellContainerVm.ChangeCurrentView(StatusScreen.ShMenu, true, bForceToInit);  //Only when comes from login, it should be reinit
                }
                else
                {
                    MessageBus.Current.SendMessage(new MessageBoxSettings
                    {
                        Message = x.Message,
                        Title = "Error al ingresar",
                    }, SharedMessageConstants.MSG_SHOW_ERRQST);
                    //Message = x.Message;
                }
            });
        }

        private void InitializeProperties()
        {
            IsOnSignIn = Visibility.Visible;
            IsOnWaiting = Visibility.Collapsed;
            IsSignInVisible = Visibility.Hidden;
        }
    }
}
