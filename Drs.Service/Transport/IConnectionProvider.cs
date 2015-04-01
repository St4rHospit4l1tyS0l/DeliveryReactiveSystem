using System;

namespace Drs.Service.Transport
{
    public interface IConnectionProvider
    {
        IObservable<IConnection> GetActiveConnection();
    }
}