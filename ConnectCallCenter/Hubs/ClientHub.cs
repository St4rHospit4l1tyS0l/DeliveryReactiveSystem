using System;
using System.Collections.Generic;
using Autofac;
using Drs.Model.Client;
using Drs.Model.Client.Recurrence;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Service.Client;
using Drs.Service.Order;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ConnectCallCenter.Hubs
{
    [HubName(SharedConstants.Server.CLIENT_HUB)]
    public class ClientHub : Hub
    {
        [HubMethodName(SharedConstants.Server.SEARCH_BY_PHONE_CLIENT_HUB_METHOD)]
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
                return new ResponseMessageData<ListItemModel>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }
        
        [HubMethodName(SharedConstants.Server.SEARCH_BY_COMPANY_CLIENT_HUB_METHOD)]
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
                return new ResponseMessageData<ListItemModel>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }

        
        [HubMethodName(SharedConstants.Server.SEARCH_CLIENTS_BY_PHONE_CLIENT_HUB_METHOD)]
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
                return new ResponseMessageData<ClientInfoModel>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }


        [HubMethodName(SharedConstants.Server.SEARCH_BY_CLIENTNAME_CLIENT_HUB_METHOD)]
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
                return new ResponseMessageData<ListItemModel>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }


        [HubMethodName(SharedConstants.Server.REMOVE_REL_PHONECLIENT_CLIENT_HUB_METHOD)]
        public ResponseMessageData<bool> RemoveRelPhoneClient(ClientPhoneModel model)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().RemoveRelPhoneClient(model);
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<bool>
                {
                    IsSuccess = false,
                    Message = ex.Message// + ex.StackTrace
                };
            }
        }

        [HubMethodName(SharedConstants.Server.CALCULATE_RECURRENCE_CLIENT_HUB_METHOD)]
        public ResponseMessageData<RecurrenceResponseModel> CalculateRecurrence(List<int> lstClientId)
        {
            try
            {
                return AppInit.Container.Resolve<IClientService>().CalculateRecurrence(lstClientId);
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<RecurrenceResponseModel>
                {
                    IsSuccess = false,
                    Message = ex.Message// + ex.StackTrace
                };
            }
        }
    }
}
