using System;
using Autofac;
using Drs.Model.Constants;
using Drs.Model.Menu;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Repository.Log;
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
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<IAccountService>().GetMenuByUser(username)
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<ButtonItemModel>.CreateCriticalMessage("No hay menú disponible");
            }
        }


        [HubMethodName(SharedConstants.Server.ACCOUNT_INFO_ACCOUNT_HUB_METHOD)]
        public ResponseMessageData<string> GetAccountInfo()
        {
            try
            {
                return new ResponseMessageData<string>
                {
                    IsSuccess = true,
                    Data = AppInit.Container.Resolve<IAccountService>()
                        .GetAccountInfo(Context.Headers[SharedConstants.Server.USERNAME_HEADER], Context.Headers[SharedConstants.Server.CONNECTION_ID_HEADER])
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<string>.CreateCriticalMessage("No hay información disponible del usuario");
            }
        }
    }
}
