using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions.Io;
using Drs.Model.Franchise;
using Drs.Model.Settings;
using Drs.Repository.Entities;
using Drs.Repository.Order;
using Drs.Service.SyncService;

namespace Drs.Service.Franchise
{
    public class SyncServerFilesService : ISyncServerFilesService
    {
        private readonly EventLog _eventLog;

        public SyncServerFilesService(EventLog eventLog)
        {
            _eventLog = eventLog;
        }

        public void DoSyncServerFilesTask(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    GetListOfFilesToSyncWithServer(token);
                }
                catch (Exception ex)
                {
                    _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                }


                try
                {
                    DownloadFilesToSyncWithServer(token);
                }
                catch (Exception ex)
                {
                    _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                }

                Task.Delay(TimeSpan.FromSeconds(SettingsData.Store.TimeSyncServerFilesOrder), token).Wait(token);
            }

        }

        private void GetListOfFilesToSyncWithServer(CancellationToken token)
        {
            var tasks = new List<Task>();

            using (var repository = new FranchiseRepository())
            {
                repository.Db.Configuration.ValidateOnSaveEnabled = false;
                var query = repository.GetUnSyncListOfFiles();
                var suscribe = query.ToObservable().Subscribe(syncListModel => tasks.Add(ExecuteGetUnSync(syncListModel, token)));

                Task.WaitAll(tasks.ToArray(), token);
                suscribe.Dispose();
            }
        }

        private Task ExecuteGetUnSync(UnSyncListModel syncListModel, CancellationToken token)
        {
            return Task.Run(() =>
            {
                try
                {
                    _eventLog.WriteEntry("Se inicia la petición de los archivos con UID: " + syncListModel.FranchiseDataVersionUid, EventLogEntryType.Information);
                    var response = GetListOfFilesFromFranchiseServer(syncListModel);
                    if (response.HasError)
                    {
                        _eventLog.WriteEntry("Al consultar la lista de archivos se generó el error: " + response.Message, EventLogEntryType.Error);
                        return;
                    }

                    SaveListOfFiles(syncListModel, response);

                    _eventLog.WriteEntry(String.Format("Termina la petición de los archivos con UID: {0}. Total de archivos: {1}",
                        syncListModel.FranchiseDataVersionUid, response.LstFiles.Length), EventLogEntryType.Information);

                }
                catch (Exception ex)
                {
                    _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                }
            }, token);
        }

        private void SaveListOfFiles(UnSyncListModel syncListModel, ResponseMessageFcUnSync response)
        {
            using (var repository = new FranchiseRepository())
            {
                repository.SaveListOfFranchiseDataFile(syncListModel, response.LstFiles.Select(e => new FranchiseDataFile
                {
                    CheckSum = e.CheckSum,
                    FileName = e.FileName,
                    FranchiseDataVersionId = syncListModel.FranchiseDataVersionId,
                    IsSync = false
                }));
            }
        }

        private ResponseMessageFcUnSync GetListOfFilesFromFranchiseServer(UnSyncListModel syncListModel)
        {
            using (var client = new SyncServiceClient(new BasicHttpBinding(), new EndpointAddress(syncListModel.WsAddress +
                SettingsData.Constants.Franchise.WS_SYNC_FILES)))
            {
                ((BasicHttpBinding)client.Endpoint.Binding).MessageEncoding = WSMessageEncoding.Mtom;
                var res = client.GetUnSyncListOfFiles(syncListModel.FranchiseDataVersionUid);
                return res;
            }
        }


        private void DownloadFilesToSyncWithServer(CancellationToken token)
        {
            List<UnSyncListModel> lstDataVersions;


            using (var repository = new FranchiseRepository())
            {
                lstDataVersions = repository.GetDataVersionsIdsReadyToDownload();
            }

            if (lstDataVersions == null || lstDataVersions.Any() == false)
                return;

            foreach (var syncListModel in lstDataVersions)
            {
                try
                {
                    var syncListModelCs = syncListModel;
                    using (var client = new SyncServiceClient(new BasicHttpBinding(), new EndpointAddress(syncListModel.WsAddress +
                        SettingsData.Constants.Franchise.WS_SYNC_FILES)))
                    {
                        var basicBinding = ((BasicHttpBinding)client.Endpoint.Binding);
                        basicBinding.MessageEncoding = WSMessageEncoding.Mtom;
                        basicBinding.MaxReceivedMessageSize = 100000000;
                        
                        var tasks = new List<Task>();
                        using (var repository = new FranchiseRepository())
                        {
                            var inClient = client;

                            var query = repository.GetFilesToSyncByVersionId(syncListModel.FranchiseDataVersionId);
                            var suscribe = query.ToObservable().Subscribe(fileModel => tasks.Add(DownloadFileAndVerifyCheckSum(syncListModelCs.FranchiseDataVersionUid,
                                fileModel, token, inClient)));
                            Task.WaitAll(tasks.ToArray(), token);
                            suscribe.Dispose();

                            repository.TrySetFranchiseSyncFilesCompleted(syncListModelCs.FranchiseDataVersionId);

                        }
                    }    
                }
                catch (Exception ex)
                {
                    _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                }
            }
        }

        private Task DownloadFileAndVerifyCheckSum(Guid franchiseDataVersionUid, SyncFileModel fileModel, CancellationToken token, SyncServiceClient client)
        {
            return Task.Run(() =>
            {
                try
                {
                    Stream stream;
                    String sMsg;
                    var hasError = client.GetFileByName(fileModel.FileName, franchiseDataVersionUid, out sMsg, out stream);

                    if (hasError)
                    {
                        _eventLog.WriteEntry("Se sucitó el siguiente error: " + sMsg, EventLogEntryType.Error);
                        return;
                    }
                    SaveSyncFileAndVerifyCheckSum(fileModel, stream);

                }
                catch (Exception ex)
                {
                    _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                }

            }, token);
        }

        private void SaveSyncFileAndVerifyCheckSum(SyncFileModel fileModel, Stream stream)
        {
            var subPath = Path.GetDirectoryName(fileModel.FileName);

            if(subPath == null)
                throw new Exception(String.Format("El archivo {0}, no tiene directorio", fileModel.FileName));

            var path = Path.Combine(SettingsData.Server.PathToSaveSyncFiles, subPath);
            path.CreateDirectoryIfNotExist();
            var fullFileName = Path.Combine(SettingsData.Server.PathToSaveSyncFiles, fileModel.FileName);
            stream.SaveToFile(fullFileName);

            var checksumVerification = fullFileName.GetChecksum();
            if (fileModel.CheckSum != checksumVerification)
            {
                _eventLog.WriteEntry(String.Format("La verificación del archivo {0} no corresponde: valor original {1}, valor calculado {2}", 
                    fullFileName, fileModel.CheckSum, checksumVerification));
                return;
            }

            using (var repository = new FranchiseRepository())
            {
                repository.UpdateSyncOkFile(fileModel.FranchiseDataFileId);
            }
        }
    }
}