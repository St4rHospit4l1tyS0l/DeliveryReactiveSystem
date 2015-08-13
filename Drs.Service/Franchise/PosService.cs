using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions;
using Drs.Infrastructure.Extensions.Io;
using Drs.Infrastructure.Extensions.Proc;
using Drs.Infrastructure.Logging;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Repository.Log;
using Drs.Repository.Shared;
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
                    var tmpFolder = Path.Combine(SettingsData.AlohaPath, SettingsData.Constants.Franchise.TMP_FOLDER);
                    var stopFile = Path.Combine(tmpFolder, SettingsData.Constants.Franchise.STOP_FILE);
                    if (File.Exists(stopFile))
                        FileExt.ForceDeleteFile(stopFile);

                    var isUpdated = IsUpdatedUpToDay(dataFolder);

                    //Check if DATA y NEWDATA has already franchise selected
                    if (isUpdated == false || !File.Exists(Path.Combine(dataFolder, model.Code)))
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

                    if (isUpdated == false)
                    {
                        ChangeAlohaIniDate(dataFolder);
                        DeleteTransLog(dataFolder);
                        DeleteTmpFiles(tmpFolder);
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

            }).ContinueWith(_ =>
            {
                if (model.PropagateOrder == null)
                    return;

                bool isExecuteOk;
                var iCount = 0;
                do
                {
                    Thread.Sleep(1000);
                    isExecuteOk = StartInjectPosData(model.PropagateOrder);

                    if(Process.GetProcessesByName(SettingsData.AlohaIber.Replace(SettingsData.Constants.EXTENSION_EXE, String.Empty)).Any() == false)
                        break;

                } while (!isExecuteOk && iCount++ < SharedConstants.Client.TRIES_INJECT_POS_DATA);

            });
        }

        private bool StartInjectPosData(PropagateOrderModel propagateOrder)
        {
            LasaFOHLib67.IberFuncs funcs;
            int termId, checkId;
            try
            {
                LasaFOHLib67.IberDepot depot = new LasaFOHLib67.IberDepotClass();
                LasaFOHLib67.IIberObject localState = depot.GetEnum(720).First();
                termId = localState.GetLongVal("TERMINAL_NUM");
                funcs = new LasaFOHLib67.IberFuncsClass();
            }
            catch (Exception ex)
            {
                SharedLogger.LogErrorToFile(ex, ex.AlohaError());
                return false;
            }


            try
            {
                funcs.LogOut(termId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                funcs.LogIn(termId, SettingsData.Client.UserAlohaPosId, String.Empty, String.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            try
            {
                funcs.ClockIn(termId, 4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            bool isTableService;
            try
            {
                funcs.RefreshCheckDisplay();
                isTableService = funcs.IsTableService();
            }
            catch (Exception ex)
            {
                SharedLogger.LogErrorToFile(ex, ex.AlohaError());
                return false;
            }

            try
            {
                var tableId = funcs.AddTable(termId, (isTableService ? 0 : 1), 0, "TbCc", 1);
                checkId = funcs.AddCheck(termId, tableId);
                funcs.RefreshCheckDisplay();
                //funcs.DisplayMessage(resp.ToString(CultureInfo.InvariantCulture));
                //Console.WriteLine(resp);
            }
            catch (Exception ex)
            {
                SharedLogger.LogErrorToFile(ex, ex.AlohaError());
                return false;
            }

            return InjectPosData(propagateOrder, termId, checkId);
        }

        private bool InjectPosData(PropagateOrderModel propagateOrder, int termId, int checkId)
        {
            try
            {
                LasaFOHLib67.IberFuncs funcs = new LasaFOHLib67.IberFuncsClass();
                int lastParentEntry = EntityConstants.NULL_VALUE;

                foreach (var itemModel in propagateOrder.PosCheck.LstItems)
                {
                    if (itemModel.ParentId == null)
                    {
                        if (lastParentEntry != EntityConstants.NULL_VALUE)
                            funcs.EndItem(termId);

                        lastParentEntry = funcs.BeginItem(termId, checkId, (int)itemModel.ItemId, "", -999999999);
                        continue;
                    }

                    funcs.ModItem(termId, lastParentEntry, (int)itemModel.ItemId, "", -999999999, 0);
                }

                if (lastParentEntry != EntityConstants.NULL_VALUE)
                    funcs.EndItem(termId);

                funcs.RefreshCheckDisplay();
                return true;
            }
            catch (Exception ex)
            {
                SharedLogger.LogErrorToFile(ex, ex.AlohaError());
                return false;
            }
        }

        private void DeleteTmpFiles(String tmpFolder)
        {
            DirExt.ForceDeleteFolder(tmpFolder);
        }

        private void DeleteTransLog(String dataFolder)
        {
            DirExt.ForceDeleteFile(Path.Combine(dataFolder, SettingsData.Constants.SystemConst.TRANS_LOG));
        }

        private void ChangeAlohaIniDate(string dataFolder)
        {
            try
            {
                var alohaIniFile = Path.Combine(dataFolder, SettingsData.Constants.SystemConst.ALOHA_INI);
                FileHelperExt.ReplaceDataInFile(alohaIniFile, "DOB=", String.Format("DOB={0}", DateTime.Today.ToString("MM dd yyyy")));
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
            }
        }

        private bool IsUpdatedUpToDay(string dataFolder)
        {
            var alohaIniFile = Path.Combine(dataFolder, SettingsData.Constants.SystemConst.ALOHA_INI);
            var today = DateTime.Today;
            var fileInfo = new FileInfo(alohaIniFile);

            return fileInfo.LastWriteTime > today;
        }

        //public bool ValidatePrices(PosCheck posCheck)
        //{
        //    var newDataFolderFranchise = posCheck.Franchise.Name;
        //    var pathNewData = Path.Combine(SettingsData.AlohaPath, newDataFolderFranchise, SettingsData.);
        //    if(File.Exists(pathNewData) == false)
        //}

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
