using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Drs.Infrastructure.Resources;
using Drs.Model.Address;
using Drs.Model.Catalog;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Model.Store;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

namespace Drs.Repository.Store
{
    public class StoreRepository : BaseOneRepository, IStoreRepository
    {
        public StoreRepository()
        {
            
        }
        public StoreRepository(CallCenterEntities callCenter)
            :base(callCenter)
        {
        }

        public CallCenterEntities InnerDbEntities {
            get
            {
                return DbEntities;
            }
        }

        public void SaveOrderToStore(OrderModelDto model)
        {
            var now = DateTime.Now;

            var orderToStore = new OrderToStore
            {
                LastStatus = OrderStatus.NEW_READY_TO_SEND,
                AddressId = model.AddressInfo.AddressId ?? SettingsData.Constants.Entities.NULL_ID_INT,
                LastUpdate = now,
                ClientPhoneId = model.PhoneId,
                ClientId = model.ClientId ?? SettingsData.Constants.Entities.NULL_ID_INT,
                FranchiseId = model.FranchiseId,
                FranchiseStoreId = (int)(model.Store.IdKey ?? SettingsData.Constants.Entities.NULL_ID_INT),
                PosOrderId = model.PosOrder.Id ?? SettingsData.Constants.Entities.NULL_ID_INT,
                StartDatetime = now,
                UserInsId = model.UserId,
                PaymentId = model.OrderDetails.Payment.Id,
                PosOrderStatus = model.OrderDetails.PosOrderStatus,
                ExtraNotes = model.OrderDetails.ExtraNotes,
                InputType =  SettingsData.Constants.StoreOrder.INPUT_TYPE_DELIVERY
            };

            DbEntities.OrderToStore.Add(orderToStore);
            SaveLogOrderToStore(orderToStore, "Se almacenó la información del pedido. Listo para el envío a la sucursal " + model.Store.Value, orderToStore.LastStatus, now);
            DbEntities.SaveChanges();
            model.OrderToStoreId = orderToStore.OrderToStoreId;
            model.Status = orderToStore.LastStatus;
        }

        public void UpdateOrderMode(long orderToStoreId, string sOrderId, string sStatus, string sMode, string sModeCharge, DateTime sPromiseTime)
        {
            var orderToStore = new OrderToStore
            {
                OrderToStoreId = orderToStoreId,
                OrderAtoId = sOrderId,
                LastStatus = sStatus,
                OrderMode = sMode,
                OrderModeCharge = sModeCharge,
                PromiseTime = sPromiseTime
            };
            DbEntities.OrderToStore.Attach(orderToStore);

            var entry = DbEntities.Entry(orderToStore);
            entry.Property(e => e.OrderAtoId).IsModified = true;
            entry.Property(e => e.LastStatus).IsModified = true;
            entry.Property(e => e.OrderMode).IsModified = true;
            entry.Property(e => e.PromiseTime).IsModified = true;
            entry.Property(e => e.OrderModeCharge).IsModified = true;

            DbEntities.SaveChanges();
            SaveLogOrderToStore(orderToStore, "Se consulta el histórico de la orden", sStatus, DateTime.Now, true);
        }

        public void UpdateOrderStatusFailedRetrieve(long orderToStoreId, int failedStatusCounter)
        {
            var orderToStore = new OrderToStore
            {
                OrderToStoreId = orderToStoreId,
                FailedStatusCounter = failedStatusCounter + 1
            };
            
            DbEntities.OrderToStore.Attach(orderToStore);
            var entry = DbEntities.Entry(orderToStore);
            entry.Property(e => e.FailedStatusCounter).IsModified = true;
            DbEntities.SaveChanges();
        }

        public void UpdateOrderStatus(long orderToStoreId, string sStatus, DateTime sPromiseTime)
        {
            var orderToStore = new OrderToStore
            {
                OrderToStoreId = orderToStoreId,
                LastStatus = sStatus,
                PromiseTime = sPromiseTime,
                FailedStatusCounter = 0
            };


            DbEntities.OrderToStore.Attach(orderToStore);

            var entry = DbEntities.Entry(orderToStore);
            entry.Property(e => e.LastStatus).IsModified = true;
            entry.Property(e => e.PromiseTime).IsModified = true;
            entry.Property(e => e.FailedStatusCounter).IsModified = true;

            if (SettingsData.Constants.TrackConst.OrderStatusEnd.Contains(orderToStore.LastStatus))
            {
                orderToStore.EndDatetime = DateTime.Now;
                entry.Property(e => e.EndDatetime).IsModified = true;
            }

            DbEntities.SaveChanges();
            SaveLogOrderToStore(orderToStore, "Se consulta el histórico de la orden", sStatus, DateTime.Now, true);
        }

        public OrderToStoreLog SaveLogOrderToStore(OrderToStore orderToStore, string comments, string status, DateTime timestamp, bool bHasToSave = false)
        {
            var orderToStoreLog = new OrderToStoreLog
            {
                Comments = comments,
                OrderToStore = orderToStore,
                Status = status,
                Timestamp = timestamp
            };
            DbEntities.OrderToStoreLog.Add(orderToStoreLog);
            if (bHasToSave)
                DbEntities.SaveChanges();

            return orderToStoreLog;
        }


        public OrderToStoreLog SaveLogOrderToStore(long orderId, string comments, string status, DateTime timestamp, bool bHasToSave = false)
        {
            var orderToStoreLog = new OrderToStoreLog
            {
                Comments = comments,
                OrderToStoreId = orderId,
                Status = status,
                Timestamp = timestamp
            };

            DbEntities.OrderToStoreLog.Add(orderToStoreLog);
            if (bHasToSave)
                DbEntities.SaveChanges();

            var orderToStore = new OrderToStore
            {
                OrderToStoreId = orderId,
                LastStatus = status
            };
            
            DbEntities.OrderToStore.Attach(orderToStore);

            var entry = DbEntities.Entry(orderToStore);
            entry.Property(e => e.LastStatus).IsModified = true;
            DbEntities.SaveChanges(); 

            return orderToStoreLog;
        }

        public FranchiseStoreWsInfo GetWsAddresInfoByOrderToStoreId(long orderToStoreId)
        {
            return DbEntities.OrderToStore.Where(e => e.OrderToStoreId == orderToStoreId)
                    .Select(e => new FranchiseStoreWsInfo{
                            AtoOrderId = e.OrderAtoId,
                            WsAddress = e.FranchiseStore.WsAddress,
                            Name = e.FranchiseStore.Name
                    }).FirstOrDefault();
        }

        public void SetCancelOrderToStore(long orderToStoreId)
        {
            var now = DateTime.Now;
            var orderToStore = new OrderToStore
            {
                OrderToStoreId = orderToStoreId,
                IsCanceled = true,
                DateTimeCanceled = now
            };

            DbEntities.OrderToStore.Attach(orderToStore);
            DbEntities.Entry(orderToStore).Property(e => e.IsCanceled).IsModified = true;
            DbEntities.Entry(orderToStore).Property(e => e.DateTimeCanceled).IsModified = true;
            DbEntities.SaveChanges();

            SaveLogOrderToStore(orderToStoreId, "Cancelado por el cliente", SettingsData.Constants.TrackConst.CANCELED, now, true);
        }

        public bool IsValidToCancel(long orderToStoreId)
        {
            var lastStatus = DbEntities.OrderToStore.Where(e => e.OrderToStoreId == orderToStoreId).Select(e => e.LastStatus).FirstOrDefault();

            if (String.IsNullOrWhiteSpace(lastStatus))
                return false;

            if (CatalogsClientModel.LstStatusCannotCancel.Any(e => e == lastStatus))
                return false;

            return true;
        }

        public bool IsFranchiseValidById(int franchiseId)
        {
            return DbEntities.Franchise.Any(e => e.FranchiseId == franchiseId && e.IsObsolete == false);
        }

        public bool IsValidManagerStoreUserId(string manUserId)
        {
            return DbEntities.AspNetUsers.Any(e => e.Id == manUserId && e.UserDetail.IsObsolete == false && e.AspNetRoles.Any(i => i.Name == RoleConstants.STORE_MANAGER));
        }

        public AddressModel IsValidRegionA(int? regionArId)
        {
            return DbEntities.RegionA.Where(e => e.RegionId == regionArId).Select(
                    e => new AddressModel{
                        CountryId = e.CountryId,
                        RegionArId = e.RegionId,
                        ZipCodeId = e.ZipCodeId
                    }).FirstOrDefault();
        }

        public AddressModel IsValidRegionB(int? regionBrId)
        {
            return DbEntities.RegionB.Where(e => e.RegionId == regionBrId).Select(
                    e => new AddressModel
                    {
                        CountryId = e.CountryId,
                        RegionArId = e.RegionArId,
                        RegionBrId = e.RegionId,
                        ZipCodeId = e.ZipCodeId
                    }).FirstOrDefault();
        }

        public AddressModel IsValidRegionC(int? regionCrId)
        {
            return DbEntities.RegionC.Where(e => e.RegionId == regionCrId).Select(
                    e => new AddressModel
                    {
                        CountryId = e.CountryId,
                        RegionArId = e.RegionArId,
                        RegionBrId = e.RegionBrId,
                        RegionCrId = e.RegionId,
                        ZipCodeId = e.ZipCodeId
                    }).FirstOrDefault();
        }

        public AddressModel IsValidRegionD(int? regionDrId)
        {
            return DbEntities.RegionD.Where(e => e.RegionId == regionDrId).Select(
                    e => new AddressModel
                    {
                        CountryId = e.CountryId,
                        RegionArId = e.RegionArId,
                        RegionBrId = e.RegionBrId,
                        RegionCrId = e.RegionCrId,
                        RegionDrId = e.RegionId,
                        ZipCodeId = e.ZipCodeId
                    }).FirstOrDefault();
        }

        public void Add(StoreUpModel model)
        {
            var franchiseStore = new FranchiseStore
            {
                AddressId = model.AddressId,
                FranchiseId = model.FranchiseId,
                Name = model.Name,
                UserIdIns = model.UserInsUpId,
                DatetimeIns = DateTime.Today,
                IsObsolete = false,
                WsAddress = model.WsAddress,
                ManageUserId = model.ManUserId
            };
            DbEntities.FranchiseStore.Add(franchiseStore);
            DbEntities.SaveChanges();
        }

        public void Update(StoreUpModel model)
        {
            var franchiseStore = DbEntities.FranchiseStore.Single(e => e.FranchiseStoreId == model.FranchiseStoreId);
            franchiseStore.AddressId = model.AddressId;
            franchiseStore.FranchiseId = model.FranchiseId;
            franchiseStore.Name = model.Name;
            franchiseStore.UserIdUpd = model.UserInsUpId;
            franchiseStore.DatetimeUpd = DateTime.Today;
            franchiseStore.IsObsolete = false;
            franchiseStore.WsAddress = model.WsAddress;
            franchiseStore.ManageUserId = model.ManUserId;
            DbEntities.SaveChanges();
        }

        public FranchiseStore FindById(int id)
        {
            return DbEntities.FranchiseStore.FirstOrDefault(e => e.FranchiseStoreId == id);
        }

        public void DoObsoleteStore(FranchiseStore store, string userId)
        {
            store.IsObsolete = true;
            store.UserIdUpd = userId;
            store.DatetimeUpd = DateTime.Now;
            DbEntities.SaveChanges();
        }

        public StoreOfflineDto IsStoreOnline(int idKey, DateTime utcDateTime)
        {
            return DbEntities.FranchiseStoreOffLine.Where(e => e.FranchiseStoreId == idKey && e.IsObsolete == false
                && ((utcDateTime >= e.DateTimeStart && utcDateTime <= e.DateTimeEnd) || (utcDateTime >= e.DateTimeStart && e.IsUndefinedOfflineTime)))
                .Select(e => new StoreOfflineDto
                {
                    DateTimeEnd = e.DateTimeEnd,
                    IsUndefinedOfflineTime = e.IsUndefinedOfflineTime
                }).FirstOrDefault();
        }

        public StoreModel GetStoreById(int id)
        {
            return DbEntities.FranchiseStore.Where(e => e.FranchiseStoreId == id && e.IsObsolete == false)
                .Select(e => new StoreModel
                {
                    Key = e.FranchiseStoreId.ToString(),
                    IdKey = e.FranchiseStoreId, 
                    Value = e.Name, 
                    MainAddress = e.Address.MainAddress,
                    LstPhones = e.FranchisePhone.Select(i => i.Phone).ToList(),
                    WsAddress = e.WsAddress
                }).FirstOrDefault();
        }


        public List<StoreModel> GetStoresByIds(List<int> storesIds)
        {
            return DbEntities.FranchiseStore.Where(e => storesIds.Any(i => i == e.FranchiseStoreId) && e.IsObsolete == false)
                .Select(e => new StoreModel
                {
                    Key = e.FranchiseStoreId.ToString(),
                    IdKey = e.FranchiseStoreId,
                    Value = e.Name,
                    MainAddress = e.Address.MainAddress,
                    LstPhones = e.FranchisePhone.Select(i => i.Phone).ToList(),
                    WsAddress = e.WsAddress
                }).ToList();
        }

        public IEnumerable<StoreNotificationCategoryModel> GetNotificationsByStore(int storeId)
        {
            var today = DateTime.Today;
            return DbEntities.CategoryMessage.Select(e => new StoreNotificationCategoryModel
            {
                CategoryName = e.Category,
                Color = e.Color,
                Position = e.Position,
                Notifications = e.StoreMessageDate.Where(i => i.DateApplied == today && i.FranchiseStoreId == storeId).Select(i => i.StoreMessage.Message).ToList()
            }).ToList();
        }

        public void SaveRecurrence(Recurrence recurrence)
        {
            DbEntities.Recurrence.Add(recurrence);
            DbEntities.SaveChanges();
        }


        public StoreUpModel FindModelById(int franchiseStoreId)
        {
            return DbEntities.FranchiseStore.Where(e => e.FranchiseStoreId == franchiseStoreId)
                .Select(e => new StoreUpModel
                {
                    FranchiseStoreId = e.FranchiseStoreId,
                    Name = e.Name,
                    FranchiseId = e.FranchiseId,
                    AddressId = e.AddressId,
                    ManUserId = e.ManageUserId,
                    WsAddress = e.WsAddress,
                    Address = new AddressModel
                    {
                        CountryId = e.Address.CountryId,
                        Country = e.Address.Country.Name,
                        MainAddress = e.Address.MainAddress,
                        NumExt = e.Address.ExtIntNumber,
                        Reference = e.Address.Reference,
                        RegionArId = e.Address.RegionArId,
                        RegionA = e.Address.RegionA.Name,
                        RegionBrId = e.Address.RegionBrId,
                        RegionB = e.Address.RegionB.Name,
                        RegionCrId = e.Address.RegionCrId,
                        RegionC = e.Address.RegionC.Name,
                        RegionDrId = e.Address.RegionDrId,
                        RegionD = e.Address.RegionD.Name,
                        ZipCodeId = e.Address.ZipCodeId,
                        ZipCode = e.Address.ZipCode.Code
                    }
                }).FirstOrDefault();
        }

        public List<OptionModel> GetFranchises()
        {
            return DbEntities.Franchise.Where(e => e.IsObsolete == false)
                .Select(e => new OptionModel
                {
                    Name = e.Name,
                    StKey = e.FranchiseId.ToString()
                }).ToList();
        }


        public List<ListItemModel> GetFranchisesStores()
        {
            return DbEntities.FranchiseStore.Where(e => e.IsObsolete == false && e.Franchise.IsObsolete == false)
                .Select(e => new ListItemModel
                {
                    IdKey = e.FranchiseId,
                    Key = e.FranchiseStoreId.ToString(),
                    Value = e.Name
                }).ToList();
        }

        public StoreOfflineModel FindStoreOfflineModelById(int storeId, int? id)
        {
            if (id.HasValue)
            {
                return DbEntities.FranchiseStoreOffLine.Where(e => e.FranchiseStoreId == storeId && e.FranchiseStoreOffLineId == id.Value)
                    .Select(e => new StoreOfflineModel
                    {
                        StoreName = e.FranchiseStore.Name,
                        FranchiseStoreId = e.FranchiseStoreId,
                        FranchiseStoreOffLineId = e.FranchiseStoreOffLineId,
                        UtcStartDateTimeSaved = e.DateTimeStart,
                        Duration = e.Duration,
                        IsUndefinedOfflineTime = e.IsUndefinedOfflineTime
                    }).FirstOrDefault();
            }
            
            
            return DbEntities.FranchiseStore.Where(e => e.FranchiseStoreId == storeId)
                .Select(e => new StoreOfflineModel
                {
                    StoreName = e.Name,
                    FranchiseStoreId = e.FranchiseStoreId
                }).FirstOrDefault();
        }

        public void UpdateOffline(StoreOfflineModel model, ResponseMessageModel response, string userId)
        {
            var offline = DbEntities.FranchiseStoreOffLine.FirstOrDefault(
                    e => e.FranchiseStoreOffLineId == model.FranchiseStoreOffLineId);

            if (offline == null)
            {
                response.HasError = true;
                response.Message = "No existe registro para actualizar";
                return;
            }

            offline.DateTimeStart = model.UtcStartDateTime;
            offline.Duration = model.Duration;
            offline.DateTimeEnd = offline.DateTimeStart.AddMinutes(offline.Duration);
            offline.UserUpdId = userId;
            offline.IsUndefinedOfflineTime = model.IsUndefinedOfflineTime;
            offline.IsObsolete = false;

            DbEntities.SaveChanges();
        }

        public void AddOffline(StoreOfflineModel model, string userId)
        {
            var offline = new FranchiseStoreOffLine
            {
                DateTimeStart = model.UtcStartDateTime,
                Duration = model.Duration,
                UserInsId = userId,
                IsObsolete = false,
                FranchiseStoreId = model.FranchiseStoreId,
                IsUndefinedOfflineTime = model.IsUndefinedOfflineTime
            };

            offline.DateTimeEnd = offline.DateTimeStart.AddMinutes(offline.Duration);

            DbEntities.FranchiseStoreOffLine.Add(offline);
            DbEntities.SaveChanges();
        }

        public void DoObsoleteStoreOffline(int id, String userId, ResponseMessageModel response)
        {
            var offline = DbEntities.FranchiseStoreOffLine.FirstOrDefault(e => e.FranchiseStoreOffLineId == id);

            if (offline == null)
            {
                response.HasError = true;
                response.Message = "No existe registro para actualizar";
                return;
            }

            offline.UserUpdId = userId;
            offline.IsObsolete = true;

            DbEntities.SaveChanges();
        }

        public List<StoreConnection> GetStoreConnection()
        {
            return DbEntities.FranchiseStore.Where(e => e.IsObsolete == false && e.Franchise.IsObsolete == false)
                .Select(e => new StoreConnection
                {
                    StoreId = e.FranchiseStoreId,
                    StoreName = e.Name,
                    WsAddress = e.WsAddress
                }).ToList();
        }

        public IQueryable<TrackOrderModel> GetCommentsQry()
        {
            var afterOnDay = DateTime.Today.AddDays(2);

            return DbEntities.OrderToStore.Where(e => e.OrderAtoId != null 
                && e.PromiseTime != null &&  e.PromiseTime < afterOnDay 
                && e.FailedStatusCounter < SettingsData.Store.MaxFailedStatusCounter 
                && SettingsData.Constants.TrackConst.OrderStatusEnd.Contains(e.LastStatus) == false).Select(e => new TrackOrderModel
                {
                    OrderToStoreId = e.OrderToStoreId,
                    AtoOrderId = e.OrderAtoId,
                    LastStatus = e.LastStatus,
                    PromiseTime = e.PromiseTime,
                    StoreId = e.FranchiseStoreId,
                    FailedStatusCounter = e.FailedStatusCounter
                });
        }

        public int GetFranchiseIdByStoreId(int franchiseStoreId)
        {
            return DbEntities.FranchiseStore.Where(e => e.FranchiseStoreId == franchiseStoreId).Select(e => e.FranchiseId).Single();   
        }

        public List<CoverageStoreModel> GetAvailableCoverageByFrachiseCode(string franchiseCode)
        {
            return DbEntities.FranchiseStoreGeoMap.Where(e => e.FranchiseStore.Franchise.Code == franchiseCode && e.FranchiseStore.IsObsolete == false && e.FranchiseStore.Franchise.IsObsolete == false)
                .Select(e => new CoverageStoreModel
                {
                    Coverage = e.Coverage,
                    StoreId = e.FranchiseStoreId
                }).ToList();
        }

    }
}
