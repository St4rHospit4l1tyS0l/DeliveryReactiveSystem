using System.Windows;
using System.Windows.Input;
using Drs.VirtualPos.Infrastructure;
using Drs.VirtualPos.Service;
using Drs.VirtualPos.Ui;

namespace Drs.VirtualPos.Vm
{
    public class NotifyIconVm
    {
        private readonly LogVm _mainvm;
        private readonly HostService _hostService;

        public NotifyIconVm()
        {
            _mainvm = new LogVm();
            _hostService = new HostService(_mainvm);
            Application.Current.MainWindow = null;
        }

        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => Application.Current.MainWindow == null,
                    CommandAction = () =>
                    {
                        var wnd = new LogWnd {DataContext = _mainvm};
                        Application.Current.MainWindow = wnd;
                        Application.Current.MainWindow.Show();
                    }
                };
            }
        }

        public ICommand HideWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        Application.Current.MainWindow.Close();
                        Application.Current.MainWindow = null;
                    },
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }


        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = () =>
                {
                    if(_hostService != null)
                        _hostService.Dispose();
                    Application.Current.Shutdown();
                } };
            }
        }
    }
}
