using System;
using Autofac;
using Drs.Model.Constants;
using Drs.Model.Menu;
using Drs.Model.Shared;
using Drs.Service.Account;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ConnectCallCenter.Hubs
{

    [HubName(SharedConstants.Server.ACCOUNT_HUB)]
    public class AccountHub : Hub
    {
        [HubMethodName(SharedConstants.Server.MENU_INFO_ACCOUNT_HUB_METHOD)]
        public ResponseMessageData<ButtonItemModel> GetMenuByUser(String username)
        {
            try
            {
                return new ResponseMessageData<ButtonItemModel>
                {
                    IsSuccess    = true,
                    LstData = AppInit.Container.Resolve<IAccountService>().GetMenuByUser(username)
                }; 
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<ButtonItemModel>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace 
                };
            }
        }
    }
}
