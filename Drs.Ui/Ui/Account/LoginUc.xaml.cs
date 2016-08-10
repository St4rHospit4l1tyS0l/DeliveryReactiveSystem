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
            MessageBus.Current.Listen<string>(SharedMessageConstants.LOGIN_FOCUS_USERNAME)
                .Subscribe(_ =>
                {
                    UserName.Focus();
                    UserName.SelectAll();
                });

        }
    }
}
