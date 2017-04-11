using System.Threading;

namespace Drs.Service.Order
{
    public interface IOrderCloudFeedService
    {
        void DoOrderCloudFeedTask(CancellationToken token);
    }
}
