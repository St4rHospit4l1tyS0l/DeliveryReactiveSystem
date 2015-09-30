using System.Collections.Generic;

namespace Drs.Service.Store
{
    public class ConnectionPoolWs<T>
    {
        public List<T> LstConnections { get; set; }
    }
}