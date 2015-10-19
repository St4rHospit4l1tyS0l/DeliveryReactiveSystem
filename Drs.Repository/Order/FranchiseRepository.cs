using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Drs.Infrastructure.Resources;
using Drs.Model.Franchise;
using Drs.Model.Menu;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

namespace Drs.Repository.Order
{
    public class FranchiseRepository : BaseOneRepository, IFranchiseRepository
    {
        public FranchiseRepository()
        {
            
        }

        public FranchiseRepository(CallCenterEntities db)
            :base(db)
        {
            
        }

        public IEnumerable<ButtonItemModel> GetFranchiseButtons()
        {
            return DbEntities.Franchise.Where(e => e.IsObsolete == false).Select(e => new ButtonItemModel
                    {
                        Color = e.FranchiseButton.Color,
                        Title = e.ShortName,
                        Position = e.FranchiseButton.Position,
                        Image = e.FranchiseButton.Image,
                        Code = e.Code,
                        Description = e.FranchiseButton.Products,
                        DataInfo = new FranchiseDataModel{DataFolder = e.FranchiseData.DataFolder, NewDataFolder = e.FranchiseData.NewDataFolder}
                    }).OrderBy(e => e.Position).ToList();

        }

        public bool IsPositionAlreadyUsed(int position, int franchiseId)
        {
            return DbEntities.Franchise.Any(e => e.IsObsolete == false && e.FranchiseButton.Position == position && e.FranchiseId != franchiseId);
        }

        public void Add(FranchiseUpModel model)
        {
            var modelNew = new Franchise
            {
                Code = model.Code,
                DatetimeIns = DateTime.Now,
                FranchiseButton = new FranchiseButton
                {
                    Color = model.Color,
                    Image = model.Image,
                    Position = model.Position,
                    Products = model.Products
                },
                FranchiseData = new FranchiseData
                {
                    DataFolder = model.DataFolder,
                    LastVersion = null,
                    NewDataFolder = model.NewDataFolder,
                    WsAddress = model.WsAddress
                },
                IsObsolete = false,
                Name = model.Name,
                ShortName = model.ShortName,
                UserIdIns = model.UserInsUpId,
            };

            DbEntities.Franchise.Add(modelNew);
            DbEntities.SaveChanges();
        }

        public void Update(FranchiseUpModel model)
        {
            var modelOld = DbEntities.Franchise.Single(e => e.FranchiseId == model.FranchiseId);

            modelOld.Code = model.Code;
            
            modelOld.FranchiseButton.Color = model.Color;
            modelOld.FranchiseButton.Image = model.Image;
            modelOld.FranchiseButton.Position = model.Position;
            modelOld.FranchiseButton.Products = model.Products;

            modelOld.FranchiseData.DataFolder = model.DataFolder;
            modelOld.FranchiseData.NewDataFolder = model.NewDataFolder;
            modelOld.FranchiseData.WsAddress = model.WsAddress;

            modelOld.Name = model.Name;
            modelOld.ShortName = model.ShortName;
            modelOld.UserIdUpd = model.UserInsUpId;
            modelOld.DatetimeUpd = DateTime.Now;

            DbEntities.SaveChanges();

        }

        public Franchise FindById(int id)
        {
            return DbEntities.Franchise.Single(e => e.FranchiseId == id);
        }

        public void DoObsolete(Franchise model, string userId)
        {
            model.IsObsolete = true;
            model.UserIdUpd = userId;
            model.DatetimeUpd = DateTime.Now;
            
            DbEntities.SaveChanges();
        }

        public bool IsCodeAlreadyUsed(string code, int franchiseId)
        {
            return DbEntities.Franchise.Any(e => e.IsObsolete == false && e.Code == code && e.FranchiseId != franchiseId);
        }

        public void SaveFranchiseDataVersion(FranchiseDataVersion model)
        {
            DbEntities.FranchiseDataVersion.Add(model);
            DbEntities.SaveChanges();
        }

        public string GetUrlSyncWsByFranchiseId(int franchiseId)
        {
            return DbEntities.Franchise.Where(e => e.FranchiseId == franchiseId).Select(e => e.FranchiseData.WsAddress).Single();
        }

        public void DoObsoleteVersion(int id, string userId, ResponseMessageModel response)
        {
            var model = DbEntities.FranchiseDataVersion.FirstOrDefault(e => e.FranchiseDataVersionId == id);

            if (model == null)
            {
                response.HasError = true;
                response.Message = "No existe registro para actualizar";
                return;
            }

            model.UserUpdId = userId;
            model.IsObsolete = true;

            DbEntities.SaveChanges();
        }

        public IEnumerable<SyncFranchiseModel> GetListSyncFiles(string eInfo)
        {
            var lstFranchiseVersionList = DbEntities.Franchise.Where(e => e.IsObsolete == false 
                && e.FranchiseDataVersion.Any(i => i.IsObsolete == false && i.IsCompleted))
                .Select(e => new
                {
                    e.FranchiseId,
                    e.Code,
                    LastVersion = e.FranchiseDataVersion.Where(i => i.IsObsolete == false && i.IsCompleted).OrderByDescending(i => i.FranchiseDataVersionId)
                        .Select(i => new
                        {
                            i.Version,
                            i.FranchiseDataVersionId,
                            i.FranchiseDataVersionUid
                        }).FirstOrDefault()
                }).ToList();


            var lstFranchiseByTerminal = DbEntities.InfoClientTerminalVersion.Where(e => e.InfoClientTerminal.Host == eInfo).
                Select(e => new
                {
                    e.FranchiseId,
                    e.Version
                }).ToList();


            var lstSyncFranchise = new List<SyncFranchiseModel>();
            foreach (var franchise in lstFranchiseVersionList)
            {
                if (lstFranchiseByTerminal.Any(e => e.FranchiseId == franchise.FranchiseId && e.Version == franchise.LastVersion.Version))
                    continue;

                lstSyncFranchise.Add(new SyncFranchiseModel
                {
                    FranchiseId = franchise.FranchiseId,
                    Code = franchise.Code,
                    Version = franchise.LastVersion.Version,
                    FranchiseDataVersionId = franchise.LastVersion.FranchiseDataVersionId,
                    FranchiseDataVersionUid = franchise.LastVersion.FranchiseDataVersionUid
                });
            }

            foreach (var syncFranchise in lstSyncFranchise)
            {
                syncFranchise.LstFiles = DbEntities.FranchiseDataFile.Where(e => e.FranchiseDataVersionId == syncFranchise.FranchiseDataVersionId)
                    .Select(e => new SyncFileModel
                    {
                        CheckSum = e.CheckSum,
                        FileName = e.FileName,
                        FranchiseDataFileId = e.FranchiseDataFileId,
                        FileType = SettingsData.Constants.FranchiseConst.SYNC_FILE_TYPE_DATA
                    }).ToList();
            }

            return lstSyncFranchise;
        }

        public OptionModel GetFranchiseByCode(string franchiseCode)
        {
            return DbEntities.Franchise.Where(e => e.Code == franchiseCode).Select(e => new OptionModel
            {
                StKey = e.FranchiseId.ToString(),
                Name = e.Name
            }).FirstOrDefault();
        }

        public FranchiseUpModel FindModelById(int id)
        {
            return DbEntities.Franchise.Where(e => e.FranchiseId == id)
                .Select(e => new FranchiseUpModel
                {
                    FranchiseId = e.FranchiseId,
                    ShortName = e.ShortName,
                    Name = e.Name,
                    Code = e.Code,
                    DataFolder = e.FranchiseData.DataFolder,
                    NewDataFolder = e.FranchiseData.NewDataFolder,
                    WsAddress = e.FranchiseData.WsAddress,
                    Position = e.FranchiseButton.Position,
                    Color = e.FranchiseButton.Color,
                    Image = e.FranchiseButton.Image,
                    Products = e.FranchiseButton.Products
                }).FirstOrDefault();
        }

        public IQueryable<UnSyncListModel> GetUnSyncListOfFiles()
        {
            return DbEntities.FranchiseDataVersion.Where(e => e.Franchise.IsObsolete == false 
                && e.IsObsolete == false && e.IsListOfFilesReceived == false)
                .Select(e => new UnSyncListModel
                {
                    FranchiseId = e.FranchiseId,
                    FranchiseDataVersionId = e.FranchiseDataVersionId,
                    WsAddress = e.Franchise.FranchiseData.WsAddress,
                    FranchiseDataVersionUid = e.FranchiseDataVersionUid
                });
        }


        public void SaveListOfFranchiseDataFile(UnSyncListModel syncListModel, IEnumerable<FranchiseDataFile> lstFiles)
        {
            DbEntities.Configuration.ValidateOnSaveEnabled = false;
            using (var dbTrans = DbEntities.Database.BeginTransaction(IsolationLevel.Snapshot))
            {
                DbEntities.FranchiseDataFile.AddRange(lstFiles);
                DbEntities.SaveChanges();

                var franchiseDataVersion = new FranchiseDataVersion
                {
                    FranchiseDataVersionId = syncListModel.FranchiseDataVersionId,
                    IsListOfFilesReceived = true
                };

                DbEntities.FranchiseDataVersion.Attach(franchiseDataVersion);
                DbEntities.Entry(franchiseDataVersion).Property(e => e.IsListOfFilesReceived).IsModified = true;
                DbEntities.SaveChanges();

                dbTrans.Commit();
            }
        }

        public IQueryable<SyncFileModel> GetFilesToSyncByVersionId(int franchiseDataVersionId)
        {
            return DbEntities.FranchiseDataFile.Where(
                e => e.IsSync == false && e.FranchiseDataVersion.IsObsolete == false
                     && e.FranchiseDataVersion.Franchise.IsObsolete == false).Select(e => new SyncFileModel
                     {
                         CheckSum = e.CheckSum,
                         FileName = e.FileName,
                         FranchiseDataFileId = e.FranchiseDataFileId
                     });
        }

        public void UpdateSyncOkFile(int franchiseDataFileId)
        {
            DbEntities.Configuration.ValidateOnSaveEnabled = false;
            var model = new FranchiseDataFile {FranchiseDataFileId = franchiseDataFileId, IsSync = true};
            DbEntities.FranchiseDataFile.Attach(model);
            DbEntities.Entry(model).Property(e => e.IsSync).IsModified = true;
            DbEntities.SaveChanges();
        }

        public List<UnSyncListModel> GetDataVersionsIdsReadyToDownload()
        {
            return DbEntities.FranchiseDataVersion.Where(e => e.IsObsolete == false && e.IsCompleted == false 
                && e.IsListOfFilesReceived && e.Franchise.IsObsolete == false)
                .Select(e => new UnSyncListModel
                {
                    FranchiseDataVersionId = e.FranchiseDataVersionId,
                    FranchiseDataVersionUid = e.FranchiseDataVersionUid,
                    FranchiseId = e.FranchiseId,
                    WsAddress = e.Franchise.FranchiseData.WsAddress
                }).ToList();
        }

        public void TrySetFranchiseSyncFilesCompleted(int franchiseDataVersionId)
        {

            //Si existe al menos un archivo no sincronizado, no es posible completar la sincronización
            if (DbEntities.FranchiseDataFile.Any(e => e.FranchiseDataVersionId == franchiseDataVersionId && e.IsSync == false))
                return;

            DbEntities.Configuration.ValidateOnSaveEnabled = false;
            var model = new FranchiseDataVersion { FranchiseDataVersionId = franchiseDataVersionId, IsCompleted = true };
            DbEntities.FranchiseDataVersion.Attach(model);
            DbEntities.Entry(model).Property(e => e.IsCompleted).IsModified = true;
            DbEntities.SaveChanges(); 
        }

        public void SetFranchiseVersionTerminalOk(int franchiseId, string sVersion, string eHost)
        {
            var infoClientTerminalId = DbEntities.InfoClientTerminal.Where(e => e.Host == eHost).Select(e => e.InfoClientTerminalId).Single();

            var model = new InfoClientTerminalVersion
            {
                FranchiseId = franchiseId,
                InfoClientTerminalId = infoClientTerminalId,
                Version = sVersion                
            };

            DbEntities.InfoClientTerminalVersion.Add(model);
            DbEntities.SaveChanges();
        }
    }
}
