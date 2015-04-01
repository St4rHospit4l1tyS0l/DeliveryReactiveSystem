using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Drs.Infrastructure.Logging;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Service.Configuration;
using Drs.Service.ReactiveDelivery;
using Drs.Ui.Ui;
using Drs.Ui.Ui.Splash;
using Drs.ViewModel.Main;
using Drs.ViewModel.Setting;
using Drs.ViewModel.Shared;
using Drs.ViewModel.SignalR;
using log4net;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using ILog = log4net.ILog;

[assembly: OwinStartup(typeof(Startup))]

namespace Drs.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private const string SignalAddress = "http://localhost:41956";
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));
        private const int MiliSecondsToInit = 1000;
        private IDisposable _signalr;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                InitializeLogging();
                Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error iniciar aplicación", String.Format("Se presentó el siguiente error: {0} - {1}", ex.Message, ex.InnerException == null ? String.Empty : ex.InnerException.Message));
            }
        }

        private async void Start()
        {
            try
            {
                var splashVm = new SplashVm(MiliSecondsToInit);
                var splash = new SplashWnd { DataContext = splashVm };
                splash.Show();

                splashVm.ShowProgress.Execute(null);
                await Task.Delay(TimeSpan.FromMilliseconds(MiliSecondsToInit));

                var bootstrapper = new Bootstrapper();
                var container = bootstrapper.Build();
                Log.Info("Iniciando Delivery Reactive System...");
                var reactiveDeliveryClient = container.Resolve<IReactiveDeliveryClient>();
                var lstHubProxies = new List<string> {
                        SharedConstants.Server.ACCOUNT_HUB,
                        SharedConstants.Server.CLIENT_HUB,
                        SharedConstants.Server.ORDER_HUB,
                        SharedConstants.Server.ADDRESS_HUB,
                        SharedConstants.Server.STORE_HUB,
                        SharedConstants.Server.TRACK_HUB
                };

                reactiveDeliveryClient.Initialize(Environment.MachineName, container.Resolve<IConfigurationProvider>().Servers, lstHubProxies, container.Resolve<ILoggerFactory>());
                SettingsData.Client.Container = container;
                await SettingConfigureWs.Initialize(reactiveDeliveryClient);
                var mainWindow = container.Resolve<MainWindow>();
                var vm = container.Resolve<IShellContainerVm>();
                vm.BootStrapper = bootstrapper;

                InitializeSignalRPosConnection();

                vm.Initialize();
                mainWindow.DataContext = vm;

                splash.Close();
                mainWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar", String.Format("Se presentó el siguiente error: {0} - {1}", ex.Message, ex.InnerException == null ? String.Empty : ex.InnerException.Message));
                Shutdown();
            }
        }

        private void InitializeSignalRPosConnection()
        {
            try
            {
                _signalr = WebApp.Start(SignalAddress);
            }
            catch (Exception exception)
            {
                Log.Error("An error occurred while starting SignalR", exception);
                throw;
            }    
        }

        private void InitializeLogging()
        {
            Thread.CurrentThread.Name = "UI";
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (_signalr != null)
                _signalr.Dispose();
            _signalr = null;
        }
    }
}
