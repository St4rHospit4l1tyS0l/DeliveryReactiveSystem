using System;
using System.IO;
using System.Threading;
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
