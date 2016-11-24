using System;
using System.Collections.Generic;
using Autofac;
using Drs.Model.Client;
using Drs.Model.Client.Recurrence;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Properties;
using Drs.Model.Shared;
using Drs.Repository.Log;
using Drs.Service.Client;
using Drs.Service.Order;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ConnectCallCenter.Hubs
{
    [HubName(SharedConstants.Server.CLIENT_HUB), UsedImplicitly]
    public class ClientHub : Hub
    {
        [HubMethodName(SharedConstants.Server.SEARCH_BY_PHONE_CLIENT_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<ListItemModel> SearchByPhone(String phone)
        {
            try
            {
                return new ResponseMessageData<ListItemModel>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<IClientService>().SearchByPhone(phone)
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<ListItemModel>.CreateCriticalMessage("No fue posible buscar por el teléfono");
            }
        }
        
        [HubMethodName(SharedConstants.Server.SEARCH_BY_COMPANY_CLIENT_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<ListItemModel> SearchByCompany(String company)
        {
            try
            {
                return new ResponseMessageData<ListItemModel>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<IClientService>().SearchByCompany(company)
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<ListItemModel>.CreateCriticalMessage("No fue posible buscar por compañía");
            }
        }

        
        [HubMethodName(SharedConstants.Server.SEARCH_CLIENTS_BY_PHONE_CLIENT_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<ClientInfoModel> SearchClientsByPhone(String phone)
        {
            try
            {
                return new ResponseMessageData<ClientInfoModel>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<IClientService>().SearchClientsByPhone(phone)
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<ClientInfoModel>.CreateCriticalMessage("No fue posible listar los clientes por el teléfono");
            }
        }


        [HubMethodName(SharedConstants.Server.SEARCH_BY_CLIENTNAME_CLIENT_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<ListItemModel> SearchClientsByClientName(String clientName)
        {
            try
            {
                return new ResponseMessageData<ListItemModel>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<IClientService>().SearchClientsByClientName(clientName)
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<ListItemModel>.CreateCriticalMessage("No fue posible listar los clientes por su nombre");
            }
        }


        [HubMethodName(SharedConstants.Server.REMOVE_REL_PHONECLIENT_CLIENT_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<bool> RemoveRelPhoneClient(ClientPhoneModel model)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().RemoveRelPhoneClient(model);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<bool>.CreateCriticalMessage("No fue posible eliminar la relación teléfono-cliente");
            }
        }

        [HubMethodName(SharedConstants.Server.CALCULATE_RECURRENCE_CLIENT_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<RecurrenceResponseModel> CalculateRecurrence(List<long> lstClientId)
        {
            try
            {
                return AppInit.Container.Resolve<IClientService>().CalculateRecurrence(lstClientId);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<RecurrenceResponseModel>.CreateCriticalMessage("No fue posible calcular la recurrencia del cliente");
            }
        }
    }
}
