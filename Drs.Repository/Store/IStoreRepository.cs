using System;
using Drs.Model.Franchise;
using Drs.Model.Order;
using Drs.Model.Store;
using Drs.Repository.Entities;

namespace Drs.Repository.Store
{
    public interface IStoreRepository : IDisposable
    {
        CallCenterEntities InnerDbEntities { get; }
        void SaveOrderToStore(OrderModelDto model);
        void UpdateOrderMode(long orderToStoreId, string sOrderId, string sStatus, string sMode, string sModeCharge, DateTime sPromiseTime);
        OrderToStoreLog SaveLogOrderToStore(OrderToStore orderToStore, string comments, string status, DateTime timestamp, bool bHasToSave = false);
        OrderToStoreLog SaveLogOrderToStore(long orderId, string comments, string status, DateTime timestamp, bool bHasToSave = false);
        FranchiseStoreWsInfo GetWsAddresInfoByOrderToStoreId(long orderToStoreId);
        void SetCancelOrderToStore(long orderToStoreId);
        bool IsValidToCancel(long orderToStoreId);
        bool IsFranchiseValidById(int franchiseId);
        bool IsValidManagerStoreUserId(string manUserId);
        AddressModel IsValidRegionA(int? regionArId);
        AddressModel IsValidRegionB(int? regionBrId);
        AddressModel IsValidRegionC(int? regionCrId);
        AddressModel IsValidRegionD(int? regionDrId);
        void Add(StoreUpModel model);
        void Update(StoreUpModel model);
        FranchiseStore FindById(int id);
        void DoObsoleteStore(FranchiseStore store, string userId);
        StoreOfflineDto IsStoreOnline(int idKey, DateTime utcDateTime);
    }
}
