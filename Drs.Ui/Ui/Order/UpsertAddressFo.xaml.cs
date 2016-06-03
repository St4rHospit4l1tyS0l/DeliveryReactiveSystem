using System;
using System.Windows;
using System.Windows.Threading;
using Drs.Ui.Gmap;
using Drs.ViewModel.Shared;

namespace Drs.Ui.Ui.Order
{
    /// <summary>
    /// Interaction logic for UpsertAddressFo.xaml
    /// </summary>
    public partial class UpsertAddressFo 
    {
        readonly string _url = AppDomain.CurrentDomain.BaseDirectory + "Gmap/map.html";
        public UpsertAddressFo()
        {
            InitializeComponent();
            IsOpenChanged += OnIsOpenChanged;
        }

        private void OnIsOpenChanged(object sender, RoutedEventArgs e)
        {
            var upsertVm = DataContext as IFlyoutBaseVm;
            if (upsertVm == null)
                return;

            upsertVm.IsOpenFinished = true;
        }

        //private void SetupObjectForScripting(object sender, RoutedEventArgs e)
        //{
        //}

        private void Flyout_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsOpen)
            {
                WebBrowser.ObjectForScripting = new HtmlInteropService(this);
                var uri = new Uri(_url);
                WebBrowser.Navigate(uri);

                var dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += DispatcherTimerTick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();

                /*Task.Run(() => Dispatcher.Invoke(() =>
                {
                    Thread.Sleep(1000);
                    WebBrowser.UpdateLayout();
                }, DispatcherPriority.Render));*/
            }
            else
            {
                WebBrowser.Visibility = Visibility.Hidden;
            }
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            var dispatcherTimer = sender as DispatcherTimer;
            if (dispatcherTimer != null) dispatcherTimer.Stop();
            Dispatcher.Invoke(() =>
            {
                WebBrowser.Visibility = Visibility.Visible;
            });
        }
        
    }
}
