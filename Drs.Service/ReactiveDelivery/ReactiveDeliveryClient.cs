using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Drs.Infrastructure.Logging;
using Drs.Model.Transport;
using Drs.Service.Concurrency;
using Drs.Service.Proxy;
using Drs.Service.ServiceClient;
using Drs.Service.Transport;

namespace Drs.Service.ReactiveDelivery
{
    public class ReactiveDeliveryClient : IReactiveDeliveryClient, IDisposable
    {
        private ConnectionProvider _connectionProvider;
        private ILoggerFactory _loggerFactory;
        private ILog _log;
        private IExecutionProxy _executionProxy;
        private IConcurrencyService _concurrencyService;
        //private IControlRepository _controlRepository;

        public void Initialize(string username, string[] servers, List<string> lstHubProxies, ILoggerFactory loggerFactory = null, string authToken = null) 
        {
            _loggerFactory = loggerFactory ?? new DebugLoggerFactory();
            _log = _loggerFactory.Create(typeof(ReactiveDeliveryClient));
            HubListeners = new HubListeners();

            _connectionProvider = new ConnectionProvider(username, servers, _loggerFactory, lstHubProxies, HubListeners.InitializeListeners);
            var executeServiceClient = new ExecuteServiceClient(_connectionProvider);

            if (authToken != null)
            {
                //var controlServiceClient = new ControlServiceClient(new AuthTokenProvider(authToken), _connectionProvider, _loggerFactory);
                //_controlRepository = new ControlRepository(controlServiceClient);
            }

            _concurrencyService = new ConcurrencyService();
            _executionProxy = new ExecutionProxy(executeServiceClient, _concurrencyService);
        }

        public HubListeners HubListeners { get; set; }

        /*
        public IControlRepository Control
        {
            get
            {
                if (_controlRepository == null)
                    throw new InvalidOperationException("You must supply an authentication token when initializing to use the control API.");
                return _controlRepository;
            }
        }*/

        public IExecutionProxy ExecutionProxy
        {
            get { return _executionProxy; }
        }

        public IConcurrencyService ConcurrencyService
        {
            get { return _concurrencyService; }
        }

        public IObservable<ConnectionInfo> ConnectionStatusStream
        {
            get
            {
                return _connectionProvider.GetActiveConnection()
                    .Do(_ => _log.Info("Nueva conexión creada por el proveedor"))
                    .Select(c => c.StatusStream)
                    .Switch()
                    .Publish()
                    .RefCount();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_connectionProvider != null)
                _connectionProvider.Dispose();
        }
    }
}
