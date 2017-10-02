using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Drs.Service.Settings;
using Drs.Service.Store;

namespace Drs.ConnectService.Services
{
    partial class SendEmailToStore : ServiceBase
    {
        private Task _task;
        private CancellationTokenSource _cancellation;
        private ISendEmailToStoreService _service;
        
        public SendEmailToStore()
        {
            InitializeComponent();
            eventLog = new EventLog();
            if (!EventLog.SourceExists("Drs.SendEmailToStore"))
            {
                EventLog.CreateEventSource("Drs.SendEmailToStore", "SendEmailToStoreLog");
            }
            eventLog.Source = "Drs.SendEmailToStore";
            eventLog.Log = "SendEmailToStoreLog";
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                eventLog.WriteEntry("Starting SendEmailToStore Service");
                InitializeSettingsService.InitializeConstants();
                _service = new SendEmailToStoreService(eventLog);
                _cancellation = new CancellationTokenSource();
                _task = Task.Run(() => DoTask(_cancellation.Token));
                eventLog.WriteEntry("Started SendEmailToStore Service");
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
                _service.DoSendEmailTask(token);
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
                eventLog.WriteEntry("Stopping SendEmailToStore Service");
                _service = null;
                _cancellation.Cancel();
                _task.Wait();
                eventLog.WriteEntry("Stopped SendEmailToStore Service");
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
            }
        }
    }
}
