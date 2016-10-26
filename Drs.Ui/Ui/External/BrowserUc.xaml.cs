using System.Reactive.Concurrency;
using Drs.ViewModel.External;
using ReactiveUI;

namespace Drs.Ui.Ui.External
{
    public partial class BrowserUc
    {
        public BrowserUc()
        {
            InitializeComponent();
        }

        public void OnRefreshBrowser(string url)
        {
            RxApp.MainThreadScheduler.Schedule(_ => WebBrowser.Navigate(url));
        }

        private void OnUserControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var vmSignal = DataContext as IBrowserVm;
            if (vmSignal != null)
                OnRefreshBrowser(vmSignal.UrlBrowser);
        }

    }
}
