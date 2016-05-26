using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using Autofac;
using Drs.Infrastructure.Crypto;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Infrastructure.Logging;
using Drs.Infrastructure.Model;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Resources.Network;
using Drs.Service.Configuration;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.Ui.Ui;
using Drs.Ui.Ui.Splash;
using Drs.ViewModel.Catalog;
using Drs.ViewModel.Main;
using Drs.ViewModel.Pos;
using Drs.ViewModel.Setting;
using Drs.ViewModel.Shared;
using Drs.ViewModel.SignalR;
using log4net;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using ReactiveUI;
using ILog = log4net.ILog;

[assembly: OwinStartup(typeof(Startup))]

namespace Drs.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private const string SIGNAL_ADDRESS = "http://localhost:41956";
        public static readonly ILog Log = LogManager.GetLogger(typeof(App));
        private const int MILI_SECONDS_TO_INIT = 1000;
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
                var splashVm = new SplashVm(MILI_SECONDS_TO_INIT);
                var splash = new SplashWnd { DataContext = splashVm };
                splash.Show();

                splashVm.ShowProgress.Execute(null);
                await Task.Delay(TimeSpan.FromMilliseconds(MILI_SECONDS_TO_INIT));

                var bootstrapper = new Bootstrapper();
                var container = bootstrapper.Build();
                Log.Info("Iniciando Delivery Reactive System...");
                await CatalogsClientConfigure.Initialize();
                var reactiveDeliveryClient = container.Resolve<IReactiveDeliveryClient>();
                var lstHubProxies = new List<string> {
                        SharedConstants.Server.ACCOUNT_HUB,
                        SharedConstants.Server.CLIENT_HUB,
                        SharedConstants.Server.FRANCHISE_HUB,
                        SharedConstants.Server.ORDER_HUB,
                        SharedConstants.Server.ADDRESS_HUB,
                        SharedConstants.Server.STORE_HUB,
                        SharedConstants.Server.TRACK_HUB
                };

                reactiveDeliveryClient.Initialize(Cypher.Encrypt(Environment.MachineName), container.Resolve<IConfigurationProvider>().Servers, lstHubProxies, container.Resolve<ILoggerFactory>());
                SettingsData.Client.Container = container;
                await SettingConfigureWs.Initialize(reactiveDeliveryClient);
                var mainWindow = container.Resolve<MainWindow>();
                var vm = container.Resolve<IShellContainerVm>();
                vm.BootStrapper = bootstrapper;

                InitializeSignalRPosConnection();

                vm.Initialize();
                mainWindow.DataContext = vm;

                ShowNextWindow(reactiveDeliveryClient, () =>
                {
                    splash.Close();
                    mainWindow.Show();                    
                }, vm);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar", String.Format("Se presentó el siguiente error: {0} - {1}", ex.Message, ex.InnerException == null ? String.Empty : ex.InnerException.Message));
                Shutdown();
            }
        }

        private void ShowNextWindow(IReactiveDeliveryClient reactiveDeliveryClient, Action showWnd, IShellContainerVm vm)
        {
            var showWndSec = showWnd;
            reactiveDeliveryClient.ExecutionProxy.ExecuteRequest<ResponseMessageData<string>, ResponseMessageData<string>>
                (SharedConstants.Server.ACCOUNT_HUB, SharedConstants.Server.ACCOUNT_INFO_ACCOUNT_HUB_METHOD, TransferDto.SameType)
                .ObserveOn(reactiveDeliveryClient.ConcurrencyService.Dispatcher)
                .SubscribeOn(reactiveDeliveryClient.ConcurrencyService.TaskPool)
                .Subscribe(e => OnInfoAccountOk(reactiveDeliveryClient, e, showWnd, vm), i => OnInfoAccountError(i, showWndSec));
        }

        private void OnInfoAccountError(Exception ex, Action showWnd)
        {
            OnInfoAccountError(ex.Message, showWnd);
        }


        private void OnInfoAccountError(string msgError, Action showWnd)
        {
            MessageBus.Current.SendMessage(msgError, SharedMessageConstants.INITIALIZE_ERROR_CHECK);
            showWnd();
        }

        private void OnInfoAccountOk(IReactiveDeliveryClient reactiveDeliveryClient, IStale<ResponseMessageData<string>> obj, Action showWnd, IShellContainerVm vm)
        {
            if (obj.IsStale)
            {
                OnInfoAccountError(ResNetwork.ERROR_NETWORK_DOWN, showWnd);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnInfoAccountError(obj.Data.Message, showWnd);
                return;
            }

            ConnectionInfoResponse response;
            try
            {
                response = new JavaScriptSerializer().Deserialize<ConnectionInfoResponse>(Cypher.Decrypt(obj.Data.Data));
            }
            catch (Exception)
            {
                OnInfoAccountError("No fue posible obtener la respuesta del servidor. Revise que tenga la versión correcta en el cliente o en el servidor"
                    , showWnd);
                return;
            }

            SyncPosFiles.GetUnsyncFiles(reactiveDeliveryClient, showWnd, vm, response);

        }


        private void InitializeSignalRPosConnection()
        {
            try
            {
                _signalr = WebApp.Start(SIGNAL_ADDRESS);
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
