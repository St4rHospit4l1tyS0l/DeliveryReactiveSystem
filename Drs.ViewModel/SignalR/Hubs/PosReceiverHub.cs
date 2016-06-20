using System;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Repository.Log;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ReactiveUI;

namespace Drs.ViewModel.SignalR.Hubs
{

    [HubName(SharedConstants.Server.POS_RECEIVER_HUB)]
    public class PosReceiverHub : Hub
    {
        [HubMethodName(SharedConstants.Server.ORDER_POS_RECEIVER_HUB_METHOD)]
        public ResponseMessageData<bool> SendOrder(PosCheck posCheck)
        {
            try
            {
                MessageBus.Current.SendMessage(posCheck, SharedMessageConstants.ORDER_SEND_POSORDER);
                
                return new ResponseMessageData<bool>
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<bool>.CreateCriticalMessage("No fue posible enviar la orden al POS");
            }
        }
    }
}
