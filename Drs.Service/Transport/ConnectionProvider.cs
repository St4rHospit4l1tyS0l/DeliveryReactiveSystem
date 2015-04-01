using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Infrastructure.Logging;
using Microsoft.AspNet.SignalR.Client;

namespace Drs.Service.Transport
{
    /// <summary>
    /// Connection provider provides always the same connection until it fails then create a new one a yield it
    /// Connection provider randomizes the list of server specified in configuration and then round robin through the list
    /// </summary>
    public class ConnectionProvider : IConnectionProvider, IDisposable
    {
        private readonly SingleAssignmentDisposable _disposable = new SingleAssignmentDisposable();
        private readonly string _username;
        private readonly IObservable<IConnection> _connectionSequence;
        private readonly string[] _servers;
        private readonly ILoggerFactory _loggerFactory;
        private readonly List<string> _lstHubProxy;
        private readonly Action<IDictionary<string, IHubProxy>> _initializeListeners;

        private int _currentIndex;
        private readonly ILog _log;

        public ConnectionProvider(string username, string[] servers, ILoggerFactory loggerFactory, List<string> lstHubProxy, Action<IDictionary<string, IHubProxy>> initializeListeners)
        {
            _username = username;
            _servers = servers;
            _loggerFactory = loggerFactory;
            _lstHubProxy = lstHubProxy;
            _initializeListeners = initializeListeners;
            _log = _loggerFactory.Create(typeof (ConnectionProvider));
            _connectionSequence = CreateConnectionSequence();
        }

        public IObservable<IConnection> GetActiveConnection()
        {
            return _connectionSequence;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        private IObservable<IConnection> CreateConnectionSequence()
        {
            return Observable.Create<IConnection>(o =>
            {
                _log.Info("Creando nueva conexión...");
                var connection = GetNextConnection();

                var statusSubscription = connection.StatusStream.Subscribe(
                    _ => { },
                    ex => o.OnCompleted(),
                    () =>
                    {
                        _log.Info("Estatus: suscripción completada");
                        o.OnCompleted();
                    });

                var connectionSubscription =
                    connection.Initialize().Subscribe(
                        _ => o.OnNext(connection),
                        ex => o.OnCompleted(),
                        o.OnCompleted);

                return new CompositeDisposable { statusSubscription, connectionSubscription };
            })
                .Repeat()
                .Replay(1)
                .LazilyConnect(_disposable);
        }

        private IConnection GetNextConnection()
        {
            var connection = new Connection(_servers[_currentIndex++], _username, _loggerFactory, _lstHubProxy);
            _initializeListeners(connection.DicHubProxy);
            if (_currentIndex == _servers.Length)
            {
                _currentIndex = 0;
            }
            return connection;
        }
    }
}