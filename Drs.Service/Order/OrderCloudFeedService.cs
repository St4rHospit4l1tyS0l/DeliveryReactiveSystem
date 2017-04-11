using System.Diagnostics;
using System.Threading;
using Drs.Model.Settings;

namespace Drs.Service.Order
{
    public class OrderCloudFeedService : IOrderCloudFeedService
    {
        private readonly EventLog _eventLog;

        public OrderCloudFeedService(EventLog eventLog)
        {
            _eventLog = eventLog;
        }

        public void DoOrderCloudFeedTask(CancellationToken token)
        {
            if (!SettingsData.Store.EnableOrderFeed)
                return;
            throw new System.NotImplementedException();
        }
    }
}