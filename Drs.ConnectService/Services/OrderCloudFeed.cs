using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Drs.Service.Order;
using Drs.Service.Settings;

namespace Drs.ConnectService.Services
{
    partial class OrderCloudFeed : ServiceBase
    {
        private Task _updateOrderTask;
        private CancellationTokenSource _cancellationToken;
        private IOrderCloudFeedService _service;

        public OrderCloudFeed()
        {
            InitializeComponent();
            eventLog = new EventLog();
            if (!EventLog.SourceExists("Drs.OrderCloudFeed"))
            {
                EventLog.CreateEventSource("Drs.OrderCloudFeed", "OrderCloudFeedLog");
            }
            eventLog.Source = "Drs.OrderCloudFeed";
            eventLog.Log = "OrderCloudFeedLog";
        }


        protected override void OnStart(string[] args)
        {
            try
            {
                eventLog.WriteEntry("Starting OrderCloudFeed Service");
                InitializeSettingsService.InitializeConstants();
                _service = new OrderCloudFeedService(eventLog);
                _cancellationToken = new CancellationTokenSource();
                _updateOrderTask = Task.Run(() => DoTask(_cancellationToken.Token));
                eventLog.WriteEntry("Started OrderCloudFeed Service");
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
            }
        }

        private void DoTask(CancellationToken token)
        {
            try
            {
                _service.DoOrderCloudFeedTask(token);
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            try
            {
                eventLog.WriteEntry("Stopping OrderCloudFeed Service");
                _service = null;
                _cancellationToken.Cancel();
                _updateOrderTask.Wait();
                eventLog.WriteEntry("Stopped OrderCloudFeed Service");
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
            }
        }
    }

}
