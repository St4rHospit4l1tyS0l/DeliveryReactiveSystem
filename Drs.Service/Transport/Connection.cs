using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Drs.Infrastructure.Crypto;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Infrastructure.Hinfo;
using Drs.Infrastructure.Logging;
using Drs.Model.Constants;
using Drs.Model.Transport;
using Microsoft.AspNet.SignalR.Client;

namespace Drs.Service.Transport
{
    /// <summary>
    /// This represents a single SignalR connection.
    /// The <see cref="Drs.Service.Transport.ConnectionProvider"/> creates connections and is responsible for creating new one when a connection is closed.
    /// </summary>
    internal class Connection : IConnection
    {
        private readonly ISubject<ConnectionInfo> _statusStream;
        private readonly HubConnection _hubConnection;

        private bool _initialized;
        private readonly ILog _log;

        public Connection(string address, string username, ILoggerFactory loggerFactory, IEnumerable<string> lstHubProxy)
        {
            _log = loggerFactory.Create(typeof (Connection));
            _statusStream = new BehaviorSubject<ConnectionInfo>(new ConnectionInfo(ConnectionStatus.Uninitialized, address));
            Address = address;
            _hubConnection = new HubConnection(address);
            _hubConnection.Headers.Add(SharedConstants.Server.USERNAME_HEADER, username);
            _hubConnection.Headers.Add(SharedConstants.Server.CONNECTION_ID_HEADER, Cypher.Encrypt(ManagementExt.GetKey()));

            CreateStatus().Subscribe(
                s => _statusStream.OnNext(new ConnectionInfo(s, address)),
                _statusStream.OnError,
                _statusStream.OnCompleted);
            _hubConnection.Error += exception => _log.Error("Hubo un error de conexión a la dirección " + address, exception);

            DicHubProxy = new Dictionary<string, IHubProxy>();

            foreach (var hubName in lstHubProxy){
                DicHubProxy.Add(hubName, _hubConnection.CreateHubProxy(hubName));
            }
        }

        public IObservable<Unit> Initialize()
        {
            if (_initialized)
            {
                throw new InvalidOperationException("La conexión ya ha sido inicializada");
            }
            _initialized = true;

            return Observable.Create<Unit>(async observer =>
            {
                _statusStream.OnNext(new ConnectionInfo(ConnectionStatus.Connecting, Address)); 

                try
                {
                    _log.InfoFormat("Conectándose a la dirección {0}", Address);
                    await _hubConnection.Start();
                    _statusStream.OnNext(new ConnectionInfo(ConnectionStatus.Connected, Address));
                    observer.OnNext(Unit.Default);
                }
                catch (Exception e)
                {
                    _log.Error("Un error ocurrió cuando inicia la conexión con SignalR", e);
                    observer.OnError(e);
                }

                return Disposable.Create(() =>
                {
                    try
                    {
                        _log.Info("Deteniendo la conexión...");
                        _hubConnection.Stop();
                        _log.Info("Conexión detenida");
                    }
                    catch (Exception e)
                    {
                        // we must never throw in a disposable
                        _log.Error("Un error ocurrió cuando se detenía la conexión", e);
                    }
                });
            })
            .Publish()
            .RefCount();
        } 

        private IObservable<ConnectionStatus> CreateStatus()
        {
            var closed = Observable.FromEvent(h => _hubConnection.Closed += h, h => _hubConnection.Closed -= h).Select(_ => ConnectionStatus.Closed);
            var connectionSlow = Observable.FromEvent(h => _hubConnection.ConnectionSlow += h, h => _hubConnection.ConnectionSlow -= h).Select(_ => ConnectionStatus.ConnectionSlow);
            var reconnected = Observable.FromEvent(h => _hubConnection.Reconnected += h, h => _hubConnection.Reconnected -= h).Select(_ => ConnectionStatus.Reconnected);
            var reconnecting = Observable.FromEvent(h => _hubConnection.Reconnecting += h, h => _hubConnection.Reconnecting -= h).Select(_ => ConnectionStatus.Reconnecting);
            return Observable.Merge(closed, connectionSlow, reconnected, reconnecting)
                .TakeUntilInclusive(status => status == ConnectionStatus.Closed); // complete when the connection is closed (it's terminal, SignalR will not attempt to reconnect anymore)
        }

        public IObservable<ConnectionInfo> StatusStream
        {
            get { return _statusStream; }
        }

        public string Address { get; private set; }

        public IDictionary<string, IHubProxy> DicHubProxy { get; private set; }


        public void SetAuthToken(string authToken)
        {
            _hubConnection.Headers[AuthTokenProvider.AuthTokenKey] = authToken;
        }

        public override string ToString()
        {
            return string.Format("Dirección: {0}", Address);
        }
    }
}