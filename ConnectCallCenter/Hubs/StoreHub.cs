using System;
using Autofac;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Properties;
using Drs.Model.Shared;
using Drs.Model.Store;
using Drs.Repository.Log;
using Drs.Service.Store;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ConnectCallCenter.Hubs
{

    [HubName(SharedConstants.Server.STORE_HUB), UsedImplicitly]
    public class StoreHub : Hub
    {
        [HubMethodName(SharedConstants.Server.SEND_ORDER_TO_STORE_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<OrderModelDto> SendOrderToStore(OrderModelDto model)
        {
            try
            {
                return AppInit.Container.Resolve<IStoreService>().SendOrderToStore(model, Clients);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<OrderModelDto>.CreateCriticalMessage("No fue posible enviar la orden a la sucursal");
            }
        }


        [HubMethodName(SharedConstants.Server.CANCEL_ORDER_STORE_HUB_METHOD), UsedImplicitly]
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
                SharedLogger.LogError(ex);
                return ResponseMessageData<ResponseMessage>.CreateCriticalMessage("No fue posible cancelar la orden");
            }
        }


        [HubMethodName(SharedConstants.Server.AVAILABLE_FOR_ADDRESS_STORE_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<StoreModel> StoreAvailableForAddress(StoreAvailableModel model)
        {
            try
            {
                var response = new ResponseMessageData<StoreModel>();

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

                return response;
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<StoreModel>.CreateCriticalMessage("No fue posible obtener una sucursal disponible para esa dirección");
            }
        }



        [HubMethodName(SharedConstants.Server.AVAILABLE_BY_STORE_STORE_HUB_METHOD), UsedImplicitly]
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
                SharedLogger.LogError(ex);
                return ResponseMessageData<StoreModel>.CreateCriticalMessage("No fue posible obtener una sucursal disponible para esa dirección o selección");
            }
        }


        [HubMethodName(SharedConstants.Server.GET_NOTIFICATIONS_BY_STORE_STORE_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<StoreNotificationCategoryModel> GetNotificationsByStore(int storeId)
        {
            try
            {
                var response = new ResponseMessageData<StoreNotificationCategoryModel>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<IStoreService>().GetNotificationsByStore(storeId)
                };
                return response;
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<StoreNotificationCategoryModel>.CreateCriticalMessage("No fue posible obtener las notificaciones para la sucursal seleccionada");
            }
        }
    }
}

