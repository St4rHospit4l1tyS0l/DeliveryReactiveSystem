using System;
using Drs.Model.Order;
using Drs.Repository.Entities;

namespace Drs.Repository.Store
{
    public interface IStoreRepository : IDisposable
    {
        CallCenterEntities InnerDbEntities { get; }
        void SaveOrderToStore(OrderModelDto model);
        void UpdateOrderMode(long orderToStoreId, string sOrderId, string sStatus, string sMode, string sModeCharge, string sPromiseTime);
        OrderToStoreLog SaveLogOrderToStore(OrderToStore orderToStore, string comments, string status, DateTime timestamp, bool bHasToSave = false);
        OrderToStoreLog SaveLogOrderToStore(long orderId, string comments, string status, DateTime timestamp, bool bHasToSave = false);
    }
}
