using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Drs.Infrastructure.Crypto;
using Drs.Infrastructure.Extensions.Io;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Service.SyncServerSvc;

namespace Drs.Service.Sync
{
    public static class SyncFileService
    {
        public static ResponseMessage StartSyncFiles(IList<SyncFranchiseModel> lstFranchiseSyncFiles)
        {
            var res = new ResponseMessage();

            if (lstFranchiseSyncFiles.Any() == false)
            {
                res.IsSuccess = true;
                return res;
            }

            using (var client = new SyncServerSvcClient())
            {
                foreach (var syncFranchiseModel in lstFranchiseSyncFiles)
                {
                    var clientIn = client;
                    var tasks = new List<Task>();
                    var syncFranchiseModelCopy = syncFranchiseModel;
                    var subscribe = syncFranchiseModel.LstFiles.ToObservable().Subscribe(syncFile =>
                    {
                        switch (syncFile.FileType)
                        {
                            case SettingsData.Constants.FranchiseConst.SYNC_FILE_TYPE_DATA:
                                {
                                    tasks.Add(CheckFilesAndSync(syncFile, syncFranchiseModelCopy.Code, clientIn,
                                        syncFranchiseModelCopy.FranchiseDataVersionUid));
                                    break;
                                }
                            case SettingsData.Constants.FranchiseConst.SYNC_FILE_TYPE_LOGO:
                                {
                                    tasks.Add(CheckLogosAndSync(syncFile, syncFranchiseModelCopy.Code, clientIn));
                                    break;
                                }
                            default:
                                syncFile.HasError = false;
                                break;
                        }

                    });

                    Task.WaitAll(tasks.ToArray());
                    subscribe.Dispose();
                }

                var sb = new StringBuilder();
                res.IsSuccess = true;
                foreach (var syncFranchiseModel in lstFranchiseSyncFiles)
                {
                    try
                    {
                        var franchiseSuccess = true;
                        foreach (var syncFile in syncFranchiseModel.LstFiles.Where(syncFile => syncFile.HasError))
                        {
                            sb.AppendLine(syncFile.Message);
                            res.IsSuccess = false;
                            franchiseSuccess = false;
                        }

                        if (franchiseSuccess)
                        {
                            client.SetFranchiseVersionTerminalOk(syncFranchiseModel.FranchiseId, syncFranchiseModel.Version,
                                Cypher.Encrypt(Environment.MachineName));
                        }
                    }
                    catch (Exception ex)
                    {
                        res.IsSuccess = false;
                        sb.AppendLine(ex.Message + " -ST- " + ex.StackTrace);
                    }
                }

                if (res.IsSuccess == false)
                {
                    res.Message = sb.ToString();
                }
            }

            return res;
        }

        private static Task CheckLogosAndSync(SyncFileModel syncFile, string code, SyncServerSvcClient clientIn)
        {
            return Task.Run(() =>
            {
                try
                {
                    var path = Path.Combine(DirExt.GetCurrentDirectory(), SharedConstants.Client.URI_RESOURCE);
                    var fileNamePath = Path.Combine(path, syncFile.FileName);

                    if (CreateAndCheckFile(syncFile, path, fileNamePath)) 
                        return;

                    GetAndSaveStreamToDisk(syncFile, clientIn, fileNamePath, SettingsData.Constants.FranchiseConst.SYNC_FILE_TYPE_LOGO);

                }
                catch (Exception ex)
                {
                    syncFile.HasError = true;
                    syncFile.Message = String.Format("Se presentó el siguiente error en el archivo (logo) {0}: {1}", syncFile.FileName, ex.Message + " -ST- " + ex.StackTrace);
                }
            });
        }

        private static bool CreateAndCheckFile(SyncFileModel syncFile, string path, string fileNamePath)
        {
            if (Directory.Exists(path))
            {
                if (File.Exists(fileNamePath))
                {
                    if (fileNamePath.GetChecksum() == syncFile.CheckSum)
                    {
                        syncFile.HasError = false;
                        return true;
                    }
                    FileExt.ForceDeleteFile(fileNamePath);
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                }
            }
            return false;
        }

        private static Task CheckFilesAndSync(SyncFileModel syncFile, string code, SyncServerSvcClient client, Guid franchiseDataVersionUid)
        {
            return Task.Run(() =>
            {
                try
                {
                    var subPath = Path.GetDirectoryName(syncFile.FileName);

                    if (String.IsNullOrWhiteSpace(subPath))
                    {
                        syncFile.HasError = true;
                        syncFile.Message = String.Format("El archivo {0} no tiene subdirectorio.", syncFile.FileName);
                        return;
                    }

                    subPath = String.Format("{0}_{1}", subPath, code);
                    subPath = Path.Combine(SettingsData.AlohaPath, subPath);
                    var fileNamePath = Path.Combine(subPath, Path.GetFileName(syncFile.FileName));

                    if (CreateAndCheckFile(syncFile, subPath, fileNamePath)) return;

                    GetAndSaveStreamToDisk(syncFile, client, fileNamePath, SettingsData.Constants.FranchiseConst.SYNC_FILE_TYPE_DATA, franchiseDataVersionUid);
                }
                catch (Exception ex)
                {
                    syncFile.HasError = true;
                    syncFile.Message = String.Format("Se presentó el siguiente error en el archivo {0}: {1}", syncFile.FileName, ex.Message + " -ST- " + ex.StackTrace);
                }
            });
        }

        private static void GetAndSaveStreamToDisk(SyncFileModel syncFile, SyncServerSvcClient client, string fileNamePath, int fileType
            , Guid franchiseDataVersionUid = default(Guid))
        {
            Stream stream;
            String sMsg;

            var hasError = client.GetFileByName(syncFile.FileName, fileType, franchiseDataVersionUid, out sMsg, out stream);

            if (hasError)
            {
                syncFile.HasError = true;
                syncFile.Message = sMsg;
                return;
            }

            stream.SaveToFile(fileNamePath);
            stream.Dispose();

            syncFile.HasError = false;
        }
    }
}
