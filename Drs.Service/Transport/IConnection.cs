using System;
using System.Collections.Generic;
using System.Reactive;
using Drs.Model.Transport;
using Microsoft.AspNet.SignalR.Client;

namespace Drs.Service.Transport
{
    public interface IConnection
    {
        IObservable<ConnectionInfo> StatusStream { get; }
        IObservable<Unit> Initialize();
        string Address { get; }
        void SetAuthToken(string authToken);
        IDictionary<string, IHubProxy> DicHubProxy { get; }
    }
 }