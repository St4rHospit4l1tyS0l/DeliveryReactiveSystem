using System.Diagnostics;
using System.Threading;

namespace Drs.Service.Store
{
    public interface IUpdateOrderService
    {
        void DoUpdateOrderTask(CancellationToken token);
    }
}
