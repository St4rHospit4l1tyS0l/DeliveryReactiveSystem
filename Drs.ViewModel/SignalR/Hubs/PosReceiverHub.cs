using System;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
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
                    IsSuccess = true,
                    //LstData = lstItems.Select(e => e.Name).ToList()
                    //LstData = AppInit.Container.Resolve<IFranchiseService>().GetFranchiseButtons(),
                };
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<bool>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }
    }
}
