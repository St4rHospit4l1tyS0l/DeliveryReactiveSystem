using System.Threading;

namespace Drs.Service.Franchise
{
    public interface ISyncServerFilesService
    {
        void DoSyncServerFilesTask(CancellationToken token);
    }
}