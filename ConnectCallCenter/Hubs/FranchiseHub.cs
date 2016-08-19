using System;
using Autofac;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Model.Properties;
using Drs.Model.Shared;
using Drs.Repository.Log;
using Drs.Service.Franchise;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ConnectCallCenter.Hubs
{

    [HubName(SharedConstants.Server.FRANCHISE_HUB), UsedImplicitly]
    public class FranchiseHub : Hub
    {
        [HubMethodName(SharedConstants.Server.LIST_SYNC_FILES_FRANCHISE_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<SyncFranchiseModel> GetListSyncFiles()
        {
            try
            {
                return new ResponseMessageData<SyncFranchiseModel>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<IFranchiseService>().GetListSyncFiles(Context.Headers[SharedConstants.Server.USERNAME_HEADER])
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<SyncFranchiseModel>.CreateCriticalMessage("No fue posible obtener los archivos a sincronizar");
            }
        }
    }
}
