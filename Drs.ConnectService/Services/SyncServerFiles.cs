using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Drs.Service.Franchise;
using Drs.Service.Settings;
using Drs.Service.Store;

namespace Drs.ConnectService.Services
{
    partial class SyncServerFiles : ServiceBase
    {
        private Task _updateOrderTask;
        private CancellationTokenSource _syncServerFilesCancellation;
        private ISyncServerFilesService _syncServerFiles;

        public SyncServerFiles()
        {
            InitializeComponent();
            eventLog = new EventLog();
            if (!EventLog.SourceExists("Drs.SyncServerFiles"))
            {
                EventLog.CreateEventSource("Drs.SyncServerFiles", "SyncServerFilesLog");
            }
            eventLog.Source = "Drs.SyncServerFiles";
            eventLog.Log = "SyncServerFilesLog";
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                eventLog.WriteEntry("Starting SyncServerFiles Service");
                InitializeSettingsService.InitializeConstants();
                _syncServerFiles = new SyncServerFilesService(eventLog);
                _syncServerFilesCancellation = new CancellationTokenSource();
                _updateOrderTask = Task.Run(() => DoSyncServerFilesTask(_syncServerFilesCancellation.Token));
                eventLog.WriteEntry("Started SyncServerFiles Service");
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
            }
        }

        private void DoSyncServerFilesTask(CancellationToken token)
        {
            try
            {
                _syncServerFiles.DoSyncServerFilesTask(token);
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
                eventLog.WriteEntry("Stopping SyncServerFiles Service");
                _syncServerFiles = null;
                _syncServerFilesCancellation.Cancel();
                _updateOrderTask.Wait();
                eventLog.WriteEntry("Stopped SyncServerFiles Service");
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
            }
        }
    }
}
