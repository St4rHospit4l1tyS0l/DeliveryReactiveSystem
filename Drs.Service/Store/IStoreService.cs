using System.Collections.Generic;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Model.Store;
using Microsoft.AspNet.SignalR.Hubs;

namespace Drs.Service.Store
{
    public interface IStoreService
    {
        ResponseMessageData<OrderModelDto> SendOrderToStore(OrderModelDto model, IHubCallerConnectionContext<dynamic> clients);
        ResponseMessage CancelOrder(long orderToStoreId);
        StoreModel StoreAvailableForAddress(StoreAvailableModel model, ResponseMessageData<StoreModel> response);
        void GetPreparationTime(string wsAddress, ResponseMessageData<StoreModel> response);
        StoreModel StoreAvailableByStore(ItemCatalog item, ResponseMessageData<StoreModel> response);
        StoreModel StoreAvailableForAddressMap(StoreAvailableModel model, ResponseMessageData<StoreModel> response);
        IEnumerable<StoreNotificationCategoryModel> GetNotificationsByStore(int storeId);
    }
}
