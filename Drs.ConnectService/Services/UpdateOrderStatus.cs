using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Drs.Service.Settings;
using Drs.Service.Store;

namespace Drs.ConnectService.Services
{
    partial class UpdateOrderStatus : ServiceBase
    {
        private Task _updateOrderTask;
        private CancellationTokenSource _updateOrderCancellation;
        private IUpdateOrderService _updateOrderService;

        public UpdateOrderStatus()
        {
            InitializeComponent();
            eventLog = new EventLog();
            if (!EventLog.SourceExists("Drs.UpdateOrderStatus"))
            {
                EventLog.CreateEventSource("Drs.UpdateOrderStatus", "UpdateOrderStatusLog");
            }
            eventLog.Source = "Drs.UpdateOrderStatus";
            eventLog.Log = "UpdateOrderStatusLog";
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                eventLog.WriteEntry("Starting UpdateOrderStatus Service");
                InitializeSettingsService.InitializeConstants();
                _updateOrderService = new UpdateOrderService(eventLog);
                _updateOrderCancellation = new CancellationTokenSource();
                _updateOrderTask = Task.Run(() => DoUpdateOrderTask(_updateOrderCancellation.Token));
                eventLog.WriteEntry("Started UpdateOrderStatus Service");
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
            }
        }

        private void DoUpdateOrderTask(CancellationToken token)
        {
            try
            {
                _updateOrderService.DoUpdateOrderTask(token);
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
                eventLog.WriteEntry("Stopping UpdateOrderStatus Service");
                _updateOrderService = null;
                _updateOrderCancellation.Cancel();
                _updateOrderTask.Wait();
                eventLog.WriteEntry("Stopped UpdateOrderStatus Service");
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
            }
        }
    }
}
