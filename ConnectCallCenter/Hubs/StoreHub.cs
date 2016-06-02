using System;
using Autofac;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Model.Store;
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


        [HubMethodName(SharedConstants.Server.CANCEL_ORDER_STORE_HUB_METHOD)]
        public ResponseMessageData<ResponseMessage> CancelOrder(long orderToStoreId)
        {
            try
            {
                return new ResponseMessageData<ResponseMessage>
                {
                    IsSuccess = true,
                    Data = AppInit.Container.Resolve<IStoreService>().CancelOrder(orderToStoreId),
                };
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<ResponseMessage>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }


        [HubMethodName(SharedConstants.Server.AVAILABLE_FOR_ADDRESS_STORE_HUB_METHOD)]
        public ResponseMessageData<StoreModel> StoreAvailableForAddress(StoreAvailableModel model)
        {
            try
            {
                var response = new ResponseMessageData<StoreModel>();


                //var store = 
                if (model.AddressInfo.IsMap == false)
                {
                    AppInit.Container.Resolve<IStoreService>().StoreAvailableForAddress(model, response);
                }
                else
                {
                    AppInit.Container.Resolve<IStoreService>().StoreAvailableForAddressMap(model, response);
                }


                if (response.IsSuccess == false)
                    return response;

                //AppInit.Container.Resolve<IStoreService>().GetPreparationTime(store.WsAddress, response);

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<StoreModel>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }



        [HubMethodName(SharedConstants.Server.AVAILABLE_BY_STORE_STORE_HUB_METHOD)]
        public ResponseMessageData<StoreModel> StoreAvailableByStore(ItemCatalog item)
        {
            try
            {
                var response = new ResponseMessageData<StoreModel>();


                var store = AppInit.Container.Resolve<IStoreService>().StoreAvailableByStore(item, response);

                if (response.IsSuccess == false)
                    return response;

                AppInit.Container.Resolve<IStoreService>().GetPreparationTime(store.WsAddress, response);

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<StoreModel>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }
    }
}

