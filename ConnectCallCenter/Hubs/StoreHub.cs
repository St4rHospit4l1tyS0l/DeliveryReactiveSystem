using System;
using System.Threading;
using Autofac;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Service.Store;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ConnectCallCenter.Hubs
{

    [HubName(SharedConstants.Server.STORE_HUB)]
    public class StoreHub : Hub
    {
        [HubMethodName(SharedConstants.Server.SEND_ORDER_TO_STORE_HUB_METHOD)]
        public ResponseMessageData<OrderModelDto> SendOrderToStore(OrderModelDto model)
        {
            try
            {
                //this.Context.Headers["UsrHdr"]
                //Thread.Sleep(5000);
                return AppInit.Container.Resolve<IStoreService>().SendOrderToStore(model, Clients);
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<OrderModelDto>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }
    }
}

