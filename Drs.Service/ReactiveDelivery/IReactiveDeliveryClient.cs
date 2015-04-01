using System;
using System.Collections.Generic;
using Drs.Infrastructure.Logging;
using Drs.Model.Transport;
using Drs.Service.Concurrency;
using Drs.Service.Proxy;

namespace Drs.Service.ReactiveDelivery
{
    public interface IReactiveDeliveryClient
    {
        IExecutionProxy ExecutionProxy { get; }
        IConcurrencyService ConcurrencyService { get; }
        IObservable<ConnectionInfo> ConnectionStatusStream { get; }
        void Initialize(string username, string[] servers, List<string> lstHubProxies, ILoggerFactory loggerFactory = null, string authToken = null);

        //IControlRepository Control { get; }
        HubListeners HubListeners { get; set; }
        
    }
}