using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Drs.Infrastructure.Extensions;
using Drs.Infrastructure.Extensions.Io;
using Drs.Infrastructure.Extensions.Proc;
using Drs.Infrastructure.Logging;
using Drs.Infrastructure.Ui;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Repository.Log;
using ReactiveUI;

namespace Drs.Service.Franchise
{
    public class PosService : IPosService
    {
        private static readonly object Lock = new object();

        public void OnFranchiseChanged(FranchiseInfoModel model)
        {
            Task.Run(() => StarPosEmbedded(model, true)).ContinueWith(_ => DoInjectPosDataByPropagate(model));
        }

        public void StarPosEmbedded(FranchiseInfoModel model, bool bHasToSetThisWndTopMost)
        {
            var dataFolderFranchise = model.DataInfo[StaticReflection.GetMemberName<FranchiseDataModel>(x => x.DataFolder)];
            var newDataFolderFranchise =
                model.DataInfo[StaticReflection.GetMemberName<FranchiseDataModel>(x => x.NewDataFolder)];
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
                var fileCode = Path.Combine(dataFolder, model.Code);

                //Check if DATA y NEWDATA has already franchise selected
                ReinitPosIfNotCurrentDobOrDifferentFranchise(isUpdated, fileCode, dataFolder, dataFolderFranchise, tmpFolder);

                //Start Iber
                var process =
                    ProcessExt.ForceStartProcess(
                        Path.Combine(SettingsData.AlohaPath, SettingsData.Constants.Franchise.BIN_FOLDER),
                        SettingsData.AlohaIberToInit,
                        SettingsData.AlohaIber.Replace(SettingsData.Constants.EXTENSION_EXE, String.Empty),
                            ScreenSizeResponsive.CalculatePosEnviromentVariables(SettingsData.Store.EnableStoreNotifications));

                if (process == null)
                {
                    MessageBus.Current.SendMessage(new MessageBoxSettings
                    {
                        Message =
                            "No fue posible ejecutar el proceso del POS, por favor reporte a soporte técnico.",
                        Title = "Error al ejecutar la aplicación",
                    }, SharedMessageConstants.MSG_SHOW_ERRQST);
                }

                if(bHasToSetThisWndTopMost)
                    SetOnTopMostWindowForNotShowingPosWindow();

                if (File.Exists(Path.Combine(newDataFolder, model.Code)))
                    return;

                //Delete NEWDATA folders
                DirExt.ForceDeleteFolder(newDataFolder);
                //Copy directories of franchise 
                DirExt.ForceCopyFolder(Path.Combine(SettingsData.AlohaPath, newDataFolderFranchise.ToString()), newDataFolder);
            }
        }

        private void ReinitPosIfNotCurrentDobOrDifferentFranchise(bool isUpdated, string fileCode, string dataFolder,
            dynamic dataFolderFranchise, string tmpFolder)
        {
            if (isUpdated == false || !File.Exists(fileCode))
            {
                //Kill Iber process if exists
                ProcessExt.ForceKillProcess(SettingsData.AlohaIber.Replace(SettingsData.Constants.EXTENSION_EXE, String.Empty));

                //Delete DATA folders
                DirExt.ForceDeleteFolder(dataFolder);
                //Copy directories of franchise 
                DirExt.ForceCopyFolder(Path.Combine(SettingsData.AlohaPath, dataFolderFranchise.ToString()),
                    dataFolder);
                try
                {
                    DirExt.ForceToCreateFile(fileCode);
                }
                catch (Exception ex)
                {
                    SharedLogger.LogErrorToFile(ex, fileCode);
                }
                //WaitForTopMostToDisable(process);

                ChangeAlohaIniDate(dataFolder);
                DeleteTransLog(dataFolder);
                DeleteTmpFiles(tmpFolder);
            }
        }

        private void DoInjectPosDataByPropagate(FranchiseInfoModel model)
        {
            if (model.PropagateOrder == null || model.PropagateOrder.HasEdit == false)
                return;

            bool isExecuteOk;
            var iCount = 0;
            var randomTime = new Random();
            do
            {
                if (iCount != 0)
                    Thread.Sleep(randomTime.Next(100, 500));
                isExecuteOk = StartInjectPosData(model.PropagateOrder);

                if (
                    Process.GetProcessesByName(SettingsData.AlohaIber.Replace(SettingsData.Constants.EXTENSION_EXE, String.Empty))
                        .Any() == false)
                    break;
            } while (!isExecuteOk && iCount++ < SharedConstants.Client.TRIES_INJECT_POS_DATA);
        }

        private static void SetOnTopMostWindowForNotShowingPosWindow()
        {
//#if DEBUG == false
            Observable.Interval(TimeSpan.FromMilliseconds(50))
                .Take(30)
                .ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ =>
                {
                    if (Application.Current.Windows.Count > 0)
                    {
                        var wnd = Application.Current.Windows[0];
                        if (wnd != null)
                            wnd.Topmost = true;
                    }

                    if (Application.Current.Windows.Count > 0)
                    {
                        var wnd = Application.Current.Windows[0];
                        if (wnd != null)
                            wnd.Topmost = false;
                    }
                });
//#endif
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
                var sMsg = ex.AlohaError();
                SharedLogger.LogErrorToFile(ex, sMsg);
                MessageBox.Show("Func error: " + sMsg + " | " + ex.Message);
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
                funcs.ClockIn(termId, SettingsData.Client.JobAlohaPosId);
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
                var sMsg = ex.AlohaError();
                SharedLogger.LogErrorToFile(ex, sMsg);
                //MessageBox.Show("Refresh Error: " + sMsg + " | " + ex.Message);
                return false;
            }

            try
            {
                var tableId = funcs.AddTable(termId, (isTableService ? 0 : 1), (isTableService ? SettingsData.Client.TablePosId : 0), SettingsData.Client.TablePosName, 1);
                checkId = funcs.AddCheck(termId, tableId);
                funcs.RefreshCheckDisplay();
            }
            catch (Exception ex)
            {
                var sMsg = ex.AlohaError();
                SharedLogger.LogErrorToFile(ex, sMsg);
                MessageBox.Show("Add Table Error: " + sMsg + " | " + ex.Message);
                return false;
            }
            return InjectPosData(propagateOrder, termId, checkId);
        }

        private bool InjectPosData(PropagateOrderModel propagateOrder, int termId, int checkId)
        {
            try
            {
                LasaFOHLib67.IberFuncs funcs = new LasaFOHLib67.IberFuncsClass();
                var dicParents = new Dictionary<long, int>();
                var bLastItemIsEndItem = true;
                var hasPromo = propagateOrder.PosCheck.Promos != null && propagateOrder.PosCheck.Promos.Count > 0;
                
                foreach (var itemModel in propagateOrder.PosCheck.LstItems)
                {
                    if(hasPromo && propagateOrder.PosCheck.Promos.ContainsKey(itemModel.ItemId))
                        continue;

                    bLastItemIsEndItem = false;
                    int entryId;
                    if (itemModel.ParentId == null)
                    {
                        if (dicParents.Count != 0)
                        {
                            funcs.EndItem(termId);
                            bLastItemIsEndItem = true;
                        }

                        entryId = funcs.BeginItem(termId, checkId, (int)itemModel.ItemId, "", -999999999);
                        dicParents.Add(itemModel.CheckItemId, entryId);
                        continue;
                    }

                    if (!dicParents.TryGetValue(itemModel.ParentId.Value, out entryId))
                        continue;

                    entryId = funcs.ModItem(termId, entryId, (int)itemModel.ItemId, "", -999999999, itemModel.ModCode);
                    dicParents.Add(itemModel.CheckItemId, entryId);
                }

                if (!bLastItemIsEndItem)
                    funcs.EndItem(termId);

                funcs.RefreshCheckDisplay();
                return true;
            }
            catch (Exception ex)
            {
                var sMsg = ex.AlohaError();
                SharedLogger.LogErrorToFile(ex, sMsg);
                MessageBox.Show("Item Error: " + sMsg + " | " + ex.Message);
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

        private static bool IsUpdatedUpToDay(string dataFolder)
        {
            try
            {
                var alohaIniFile = Path.Combine(dataFolder, SettingsData.Constants.SystemConst.ALOHA_INI);

                if (File.Exists(alohaIniFile) == false)
                    return false;

                var today = DateTime.Today;
                var fileInfo = new FileInfo(alohaIniFile);

                if (fileInfo.LastWriteTime < today)
                    return false;


                var dob = FileHelperExt.ReadFirstValue(alohaIniFile, "DOB=");

                if (dob == null)
                    return false;

                var splitDob = dob.Split(' ');

                if (splitDob.Length != 3)
                    return false;

                var alohaDate = new DateTime(int.Parse(splitDob[2]), int.Parse(splitDob[0]), int.Parse(splitDob[1]));

                return alohaDate >= today;
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return false;
            }
        }


        public static bool DeletePosFoldersDataAndNewDataIfPosIsDown()
        {
            var posProcessName = SettingsData.AlohaIber.Replace(SettingsData.Constants.EXTENSION_EXE, String.Empty);
            var lstGndDirectories = Directory.GetDirectories(SettingsData.AlohaPath).Select(e => new DirectoryInfo(e).Name).Where(e => e.StartsWith("20") && e.Length == 8).ToList();

            if (lstGndDirectories.Any())
            {
                ProcessExt.ForceKillProcess(posProcessName);
                foreach (var gndDirectory in lstGndDirectories)
                {
                    DirExt.ForceDeleteFolder(Path.Combine(SettingsData.AlohaPath, gndDirectory));
                }
            }

            if (ProcessExt.ProcessIsRunning(posProcessName))
                return false;

            var dataFolder = Path.Combine(SettingsData.AlohaPath, SettingsData.Constants.Franchise.DATA_FOLDER);
            DirExt.ForceDeleteFolder(dataFolder);

            var newDataFolder = Path.Combine(SettingsData.AlohaPath, SettingsData.Constants.Franchise.NEWDATA_FOLDER);
            DirExt.ForceDeleteFolder(newDataFolder);

            return true;
        }
    }
}
