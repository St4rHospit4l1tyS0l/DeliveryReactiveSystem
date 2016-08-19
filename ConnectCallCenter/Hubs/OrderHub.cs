using System;
using Autofac;
using Drs.Model.Constants;
using Drs.Model.Menu;
using Drs.Model.Order;
using Drs.Model.Properties;
using Drs.Model.Shared;
using Drs.Repository.Log;
using Drs.Service.Franchise;
using Drs.Service.Order;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ConnectCallCenter.Hubs
{

    [HubName(SharedConstants.Server.ORDER_HUB), UsedImplicitly]
    public class OrderHub : Hub
    {
        [HubMethodName(SharedConstants.Server.LST_FRANCHISE_ORDER_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<ButtonItemModel> GetFranchiseButtons()
        {
            try
            {
                var lstData = AppInit.Container.Resolve<IFranchiseService>().GetFranchiseButtons();

                return new ResponseMessageData<ButtonItemModel>
                {
                    IsSuccess = true,
                    LstData = lstData
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<ButtonItemModel>.CreateCriticalMessage("No fue posible obtener las franquicias a emplear");
            }
        }

        [HubMethodName(SharedConstants.Server.SAVE_PHONE_ORDER_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<PhoneModel> SavePhone(PhoneModel model)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().SavePhone(model);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<PhoneModel>.CreateCriticalMessage("No fue posible almacenar el número telefónico");
            }
        }


        [HubMethodName(SharedConstants.Server.SAVE_CLIENT_ORDER_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<ClientInfoModel> SaveClient(ClientInfoModel model)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().SaveClient(model);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<ClientInfoModel>.CreateCriticalMessage("No fue posible almacenar los datos del cliente");
            }
        }

        [HubMethodName(SharedConstants.Server.SAVE_POS_CHECK_ORDER_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<PosCheck> SavePosCheck(PosCheck model)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().SavePosCheck(model);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<PosCheck>.CreateCriticalMessage("No fue posible almacenar la orden del POS");
            }
        }


        [HubMethodName(SharedConstants.Server.POS_ORDER_BYID_ORDER_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<PropagateOrderModel> PosOrderByOrderToStoreId(long orderToStoreId)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().PosOrderByOrderToStoreId(orderToStoreId);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<PropagateOrderModel>.CreateCriticalMessage("No fue posible obtener la última orden");
            }
        }


        [HubMethodName(SharedConstants.Server.LAST_N_ORDERS_ORDER_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<LastOrderInfoModel> LastNthOrdersByPhone(String phone)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().LastNthOrdersByPhone(phone);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<LastOrderInfoModel>.CreateCriticalMessage("No fue posible obtener la última orden");
            }
        }


        [HubMethodName(SharedConstants.Server.CALCULATE_PRICES_ORDER_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<PosCheck> CalculatePrices(String phone)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().CalculatePrices(phone);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<PosCheck>.CreateCriticalMessage("No fue posible calcular el costo del pedido");
            }
        }
    }
}
