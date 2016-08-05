using System;
using System.Windows;
using Drs.Model.Constants;
using ReactiveUI;

namespace Drs.Ui.Ui.Account
{
    /// <summary>
    /// Interaction logic for LoginUc.xaml
    /// </summary>
    public partial class LoginUc
    {
        public LoginUc()
        {
            InitializeComponent();
            VwBoxLogin.Height = SystemParameters.PrimaryScreenHeight;
            VwBoxLogin.Width = SystemParameters.PrimaryScreenWidth;
            MessageBus.Current.Listen<string>(SharedMessageConstants.LOGIN_FOCUS_USERNAME)
                .Subscribe(_ =>
                {
                    UserName.Focus();
                    UserName.SelectAll();
                });

        }
    }
}
