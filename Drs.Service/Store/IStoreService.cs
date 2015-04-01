using Drs.Model.Order;
using Drs.Model.Shared;
using Microsoft.AspNet.SignalR.Hubs;

namespace Drs.Service.Store
{
    public interface IStoreService
    {
        ResponseMessageData<OrderModelDto> SendOrderToStore(OrderModelDto model, IHubCallerConnectionContext<dynamic> clients);
    }
}
