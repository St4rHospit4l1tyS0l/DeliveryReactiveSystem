using System.Threading;

namespace Drs.Service.Store
{
    public interface ISendEmailToStoreService
    {
        void DoSendEmailTask(CancellationToken token);
    }
}
