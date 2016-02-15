using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Drs.VirtualPos.Infrastructure;

namespace Drs.VirtualPos.Vm
{
    public class LogVm : ViewBaseVp
    {
        public LogVm()
        {
            LogInformation = new ObservableCollection<string> {"Iniciando bitácora"};
        }


        private ObservableCollection<string> _logInformation;
        public ObservableCollection<string> LogInformation
        {
            get { return _logInformation; }
            set
            {
                _logInformation = value;
                OnPropertyChanged("LogInformation");
            }
        }

        public void InsertItem(String sItem)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
              new Action(() => LogInformation.Insert(0, sItem)));
        }
    }
}
