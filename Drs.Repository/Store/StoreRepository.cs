using System;
using System.Linq;
using System.Linq.Dynamic;
using Drs.Model.Catalog;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Repository.Entities;
using Drs.Repository.Shared;
using Microsoft.Owin.Security.Provider;

namespace Drs.Repository.Store
{
    public class StoreRepository : BaseOneRepository, IStoreRepository
    {
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
                FranchiseStoreId = model.Store.IdKey ?? SettingsData.Constants.Entities.NULL_ID_INT,
                PosOrderId = model.PosOrder.Id ?? SettingsData.Constants.Entities.NULL_ID_INT,
                StartDatetime = now,
                UserInsId = model.UserId,
                PaymentId = model.OrderDetails.Payment.Id
            };

            DbEntities.OrderToStore.Add(orderToStore);
            SaveLogOrderToStore(orderToStore, "Se almacenó la información del pedido. Listo para el envío a la tienda " + model.Store.Value, orderToStore.LastStatus, now);
            DbEntities.SaveChanges();
            model.OrderToStoreId = orderToStore.OrderToStoreId;
            model.Status = orderToStore.LastStatus;
        }

        public void UpdateOrderMode(long orderToStoreId, string sOrderId, string sStatus, string sMode, string sModeCharge, string sPromiseTime)
        {
            var orderToStore = new OrderToStore {OrderToStoreId = orderToStoreId, OrderAtoId = sOrderId, LastStatus = sStatus, OrderMode = sMode, 
                OrderModeCharge = sModeCharge, PromiseTime = sPromiseTime};
            DbEntities.OrderToStore.Attach(orderToStore);

            var entry = DbEntities.Entry(orderToStore);
            entry.Property(e => e.OrderAtoId).IsModified = true;
            entry.Property(e => e.LastStatus).IsModified = true;
            entry.Property(e => e.OrderMode).IsModified = true;
            entry.Property(e => e.PromiseTime).IsModified = true;
            entry.Property(e => e.OrderModeCharge).IsModified = true;

            DbEntities.SaveChanges(); 

            SaveLogOrderToStore(orderToStore, String.Empty, sStatus, DateTime.Now, bHasToSave: true);

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

        public void SaveRecurrence(Recurrence recurrence)
        {
            DbEntities.Recurrence.Add(recurrence);
            DbEntities.SaveChanges();
        }
    }
}
