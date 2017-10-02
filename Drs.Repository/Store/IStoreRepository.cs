using System;
using System.Collections.Generic;
using Drs.Model.Address;
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
        StoreModel GetStoreById(int id);
        int GetFranchiseIdByStoreId(int franchiseStoreId);
        List<CoverageStoreModel> GetAvailableCoverageByFrachiseCode(string franchiseCode);
        List<StoreModel> GetStoresByIds(List<int> storesIds);
        IEnumerable<StoreNotificationCategoryModel> GetNotificationsByStore(int storeId);
        bool OrderExists(long orderId, string referenceId);
        List<OrderStoreModel> GetAllInProgressOrdersByStore(int storeId);
        void SaveOrderToStoreEmail(long orderToStoreId);

    }
}
