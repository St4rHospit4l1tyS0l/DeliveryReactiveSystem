using System;
using System.Threading;
using Autofac;
using Drs.Model.Constants;
using Drs.Model.Menu;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Service.Franchise;
using Drs.Service.Order;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ConnectCallCenter.Hubs
{

    [HubName(SharedConstants.Server.ORDER_HUB)]
    public class OrderHub : Hub
    {
        [HubMethodName(SharedConstants.Server.LST_FRANCHISE_ORDER_HUB_METHOD)]
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
                return new ResponseMessageData<ButtonItemModel>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }

        //public static int i = 0;
        [HubMethodName(SharedConstants.Server.SAVE_PHONE_ORDER_HUB_METHOD)]
        public ResponseMessageData<PhoneModel> SavePhone(PhoneModel model)
        {
            try
            {
                //Thread.Sleep(1000);
                //if(i++ % 2 == 0)
                //throw new Exception("Error al guardar");
                return AppInit.Container.Resolve<IOrderService>().SavePhone(model);
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<PhoneModel>
                {
                    IsSuccess = false,
                    Message = ex.Message// + ex.StackTrace
                };
            }
        }


        //public static int I = 1;
        [HubMethodName(SharedConstants.Server.SAVE_CLIENT_ORDER_HUB_METHOD)]
        public ResponseMessageData<ClientInfoModel> SaveClient(ClientInfoModel model)
        {
            try
            {
                Thread.Sleep(1000);
                ////if (I++ % 5 != 0)
                //    throw new Exception("Error al guardar el cliente");
                return AppInit.Container.Resolve<IOrderService>().SaveClient(model);
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<ClientInfoModel>
                {
                    IsSuccess = false,
                    Message = ex.Message// + ex.StackTrace
                };
            }
        }

        [HubMethodName(SharedConstants.Server.SAVE_POS_CHECK_ORDER_HUB_METHOD)]
        public ResponseMessageData<PosCheck> SavePosCheck(PosCheck model)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().SavePosCheck(model);
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<PosCheck>
                {
                    IsSuccess = false,
                    Message = ex.Message// + ex.StackTrace
                };
            }
        }


        [HubMethodName(SharedConstants.Server.LAST_ORDER_ORDER_HUB_METHOD)]
        public ResponseMessageData<PropagateOrderModel> LastOrderByPhone(String phone)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().LastOrderByPhone(phone);
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<PropagateOrderModel>
                {
                    IsSuccess = false,
                    Message = ex.Message// + ex.StackTrace
                };
            }
        }


        [HubMethodName(SharedConstants.Server.CALCULATE_PRICES_ORDER_HUB_METHOD)]
        public ResponseMessageData<PosCheck> CalculatePrices(String phone)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().CalculatePrices(phone);
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<PosCheck>
                {
                    IsSuccess = false,
                    Message = ex.Message// + ex.StackTrace
                };
            }
        }
    }
}
