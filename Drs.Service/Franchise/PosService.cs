using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions;
using Drs.Infrastructure.Extensions.Io;
using Drs.Infrastructure.Extensions.Proc;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Model.Shared;
using ReactiveUI;

namespace Drs.Service.Franchise
{
    public class PosService : IPosService
    {
        private static readonly object Lock = new object();

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public void OnFranchiseChanged(FranchiseInfoModel model)
        {
            Task.Run(() =>
            {
                var dataFolderFranchise = model.DataInfo[StaticReflection.GetMemberName<FranchiseDataModel>(x => x.DataFolder)];
                var newDataFolderFranchise = model.DataInfo[StaticReflection.GetMemberName<FranchiseDataModel>(x => x.NewDataFolder)];
                var dataFolder = Path.Combine(SettingsData.AlohaPath, SettingsData.Constants.Franchise.DATA_FOLDER);
                var newDataFolder = Path.Combine(SettingsData.AlohaPath, SettingsData.Constants.Franchise.NEWDATA_FOLDER);

                lock (Lock)
                {
                    
                    //Delete STOP file if exists
                    var stopFile = Path.Combine(Path.Combine(SettingsData.AlohaPath, SettingsData.Constants.Franchise.TMP_FOLDER), SettingsData.Constants.Franchise.STOP_FILE);
                    if (File.Exists(stopFile))
                        FileExt.ForceDeleteFile(stopFile);

                    //Check if DATA y NEWDATA has already franchise selected
                    if (!File.Exists(Path.Combine(dataFolder, model.Code)))
                    {
                        //Kill Iber process if exists
                        ProcessExt.ForceKillProcess(SettingsData.AlohaIber.Replace(
                            SettingsData.Constants.EXTENSION_EXE, String.Empty));

                        //Delete DATA folders
                        DirExt.ForceDeleteFolder(dataFolder);
                        //Copy directories of franchise 
                        DirExt.ForceCopyFolder(Path.Combine(SettingsData.AlohaPath, dataFolderFranchise.ToString()),
                            dataFolder);
                        //WaitForTopMostToDisable(process);

                    }

                    //Start Iber
                    var process = ProcessExt.ForceStartProcess(
                            Path.Combine(SettingsData.AlohaPath, SettingsData.Constants.Franchise.BIN_FOLDER),
                            SettingsData.AlohaIberToInit,
                            SettingsData.AlohaIber.Replace(SettingsData.Constants.EXTENSION_EXE, String.Empty));


                    if (process == null)
                    {
                        MessageBus.Current.SendMessage(new MessageBoxSettings
                        {
                            Message =
                                "No fue posible ejecutar el proceso del POS, por favor reporte a soporte técnico.",
                            Title = "Error al ejecutar la aplicación",
                        }, SharedMessageConstants.MSG_SHOW_ERRQST);
                    }




                    if (File.Exists(Path.Combine(newDataFolder, model.Code))) 
                        return;
                    
                    //Delete NEWDATA folders
                    DirExt.ForceDeleteFolder(newDataFolder);
                    //Copy directories of franchise 
                    DirExt.ForceCopyFolder(Path.Combine(SettingsData.AlohaPath, newDataFolderFranchise.ToString()), newDataFolder);
                }

            });
        }

        /*private void WaitForTopMostToDisable(Process process)
        {
            while (true)
            {
                if (GetForegroundWindow() == process.MainWindowHandle)
                {
                    //MessageBox.Show("Prueba");
                    break;
                }
                Thread.Sleep(200);
            }
        }*/
    }
}
