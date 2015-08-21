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
        StoreModel StoreAvailableForAddress(StoreAvailableModel model);
        void StoreHasOnlineAndCapacity(StoreModel model);
    }
}
