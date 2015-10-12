using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FranchiseChannel.Service.Infrastructure.Io;
using FranchiseChannel.Service.Model;
using Microsoft.VisualBasic.FileIO;
using SearchOption = System.IO.SearchOption;

namespace FranchiseChannel.Service.Sync
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SyncService" in both code and config file together.
    public class SyncService : ISyncService
    {

        public ResponseMessageFc QueryForFiles(Guid uidVersion)
        {
            var msg = new ResponseMessageFc();
            try
            {
                msg.TotalFiles = SnapshotPathsAndFiles(uidVersion.ToString());
                msg.HasError = false;
            }
            catch (Exception ex)
            {
                msg.Message = ex.Message + " - " + ex.StackTrace;
                msg.HasError = true;
            }

            return msg;
        }

        public ResponseMessageFcUnSync GetUnSyncListOfFiles(Guid uidVersion)
        {
            var msg = new ResponseMessageFcUnSync();
            try
            {
                var lst = GetListOfFilesByGuid(uidVersion, Settings.DATA);
                lst.AddRange(GetListOfFilesByGuid(uidVersion, Settings.NEWDATA));
                msg.HasError = false;
                msg.LstFiles = lst;
            }
            catch (Exception ex)
            {
                msg.Message = ex.Message + " - " + ex.StackTrace;
                msg.HasError = true;
            }

            return msg;
        }

        private List<UnSyncFilesModel> GetListOfFilesByGuid(Guid pathUid, string pathAlohaDataDir)
        {
           var path = Path.Combine(Settings.ContainerPath, pathUid.ToString(), pathAlohaDataDir);

            var lstFiles = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            var lstUnSyncFiles = lstFiles.Select(file => new UnSyncFilesModel
            {
                FileName = Path.GetFileName(file), 
                CheckSum = file.GetChecksum()
            }).ToList();

            return lstUnSyncFiles;
        }

        private static int SnapshotPathsAndFiles(string newPath)
        {
            var paths = CreatePaths(newPath);
            FileSystem.CopyDirectory(Settings.DataPath, paths.Data);
            FileSystem.CopyDirectory(Settings.NewDataPath, paths.NewData);

            var totFiles = Directory.GetFiles(Settings.DataPath, "*", SearchOption.TopDirectoryOnly).Length;
            totFiles += Directory.GetFiles(Settings.NewDataPath, "*", SearchOption.TopDirectoryOnly).Length;
            return totFiles;
        }

        private static DataPaths CreatePaths(string newPath)
        {
            var path = Path.Combine(Settings.ContainerPath, newPath);
            Directory.CreateDirectory(path);

            var paths = new DataPaths
            {
                Data = Path.Combine(path, Settings.DATA),
                NewData = Path.Combine(path, Settings.NEWDATA),
            };

            Directory.CreateDirectory(paths.Data);
            Directory.CreateDirectory(paths.NewData);

            return paths;
        }
    }
}
