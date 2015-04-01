using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AdmInterceptActivity;
using Autofac;
using Drs.Infrastructure.Logging;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.PosService.BsLogic;
using Drs.Service.Configuration;
using Drs.Service.ReactiveDelivery;

namespace Drs.PosInterface.Com
{

    [ComVisible(true)]
    public partial class InterfacePosActCom : IInterceptAlohaActivity12
    {

        private readonly IPosActService _service;
        private static IContainer _container;

        public InterfacePosActCom()
        {
            try
            {
                if (_container == null)
                    _container = new Bootstrapper().Build();

                SettingsData.Client.Container = _container;
                var reactiveDeliveryClient = _container.Resolve<IReactiveDeliveryClient>();

                var lstHubProxies = new List<string>
                    {
                        SharedConstants.Server.POS_RECEIVER_HUB
                    };
                reactiveDeliveryClient.Initialize(Environment.MachineName, _container.Resolve<IConfigurationProvider>().Servers, lstHubProxies, 
                    _container.Resolve<ILoggerFactory>());
                _service = _container.Resolve<IPosActService>();
                _service.Client = reactiveDeliveryClient;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void IInterceptAlohaActivity.LogIn(int iEmployeeId, string sName)
        {
            _service.LogIn(iEmployeeId, sName);
        }

        public void EnterIberScreen(int iTermId, int iScreenId)
        {
            _service.EnterIberScreen(iTermId, iScreenId);
        }

        public void ExitIberScreen(int iTermId, int iScreenId)
        {
            _service.ExitIberScreen(iTermId, iScreenId);
        }

        public void KitchenOrderStatus(string sOrders)
        {
            _service.KitchenOrderStatus(sOrders);
        }

        public void RenameTab(int iTermId, int iCheckId, string stabName)
        {
            _service.RenameTab(iTermId, iCheckId, stabName);
        }

        void IInterceptAlohaActivity.LogOut(int iEmployeeId, string sName)
        {
            _service.LogOut(iEmployeeId, sName);
        }

        void IInterceptAlohaActivity.ClockIn(int iEmployeeId, string sEmpName, int iJobcodeId, string sJobName)
        {
            _service.ClockIn(iEmployeeId, sEmpName, iJobcodeId, sJobName);
        }

        void IInterceptAlohaActivity.ClockOut(int iEmployeeId, string sEmpName)
        {
            _service.ClockOut(iEmployeeId, sEmpName);
        }

        void IInterceptAlohaActivity.OpenTable(int iEmployeeId, int iQueueId, int iTableId, int iTableDefId, string sName)
        {
            _service.OpenTable(iEmployeeId, iQueueId, iTableId, iTableDefId, sName);
        }

        void IInterceptAlohaActivity.CloseTable(int iEmployeeId, int iQueueId, int iTableId)
        {
            _service.CloseTable(iEmployeeId, iQueueId, iTableId);
        }

        void IInterceptAlohaActivity.OpenCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.OpenCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity.CloseCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.CloseCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity.TransferTable(int iFromEmployeeId, int iToEmployeeId, int iTableId, string sNewName, int iIsGetCheck)
        {
            _service.TransferTable(iFromEmployeeId, iToEmployeeId, iTableId, sNewName, iIsGetCheck);
        }

        void IInterceptAlohaActivity.AcceptTable(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AcceptTable(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity.SaveTab(int iEmployeeId, int iTableId, string sName)
        {
            _service.SaveTab(iEmployeeId, iTableId, sName);
        }

        void IInterceptAlohaActivity.AddTab(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AddTab(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity.NameOrder(int iEmployeeId, int iQueueId, int iTableId, string sName)
        {
            _service.NameOrder(iEmployeeId, iQueueId, iTableId, sName);
        }

        void IInterceptAlohaActivity.Bump(int iTableId)
        {
            _service.Bump(iTableId);
        }

        void IInterceptAlohaActivity.AddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.AddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity.ModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.ModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity.OrderItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iModeId)
        {
            _service.OrderItems(iEmployeeId, iQueueId, iTableId, iCheckId, iModeId);
        }

        void IInterceptAlohaActivity.HoldItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.HoldItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity.OpenItem(int iEmployeeId, int iEntryId, int iItemId, string sDescription, double dPrice)
        {
            _service.OpenItem(iEmployeeId, iEntryId, iItemId, sDescription, dPrice);
        }

        void IInterceptAlohaActivity.SpecialMessage(int iEmployeeId, int iMessageId, string sMessage)
        {
            _service.SpecialMessage(iEmployeeId, iMessageId, sMessage);
        }

        void IInterceptAlohaActivity.DeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {
            _service.DeleteItems(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iReasonId);
        }

        void IInterceptAlohaActivity.UpdateItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.UpdateItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity.ApplyPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.ApplyPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity.AdjustPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.AdjustPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity.DeletePayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.DeletePayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity.ApplyComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.ApplyComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity.DeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.DeleteComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity.ApplyPromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.ApplyPromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity.DeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.DeletePromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity.Custom(string sName)
        {
            _service.Custom(sName);
        }

        void IInterceptAlohaActivity.Startup(int hMainWnd)
        {
            _service.Startup(hMainWnd);
        }

        void IInterceptAlohaActivity.InitializationComplete()
        {
            _service.InitializationComplete();
        }

        void IInterceptAlohaActivity.Shutdown()
        {
            _service.Shutdown();
        }

        void IInterceptAlohaActivity.CarryoverId(int iType, int iOldId, int iNewId)
        {
            _service.CarryoverId(iType, iOldId, iNewId);
        }

        void IInterceptAlohaActivity.EndOfDay(int iIsMaster)
        {
            _service.EndOfDay(iIsMaster);
        }

        void IInterceptAlohaActivity.CombineOrder(int iEmployeeId, int iSrcQueueId, int iSrcTableId, int iSrcCheckId, int iDstQueueId, int iDstTableId, int iDstCheckId)
        {
            _service.CombineOrder(iEmployeeId, iSrcQueueId, iSrcTableId, iSrcCheckId, iDstQueueId, iDstTableId, iDstCheckId);
        }

        void IInterceptAlohaActivity.OnClockTick()
        {
            _service.OnClockTick();
        }

        void IInterceptAlohaActivity.PreModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.PreModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity.LockOrder(int iTableId)
        {
            _service.LockOrder(iTableId);
        }

        void IInterceptAlohaActivity.UnlockOrder(int iTableId)
        {
            _service.UnlockOrder(iTableId);
        }

        void IInterceptAlohaActivity.SetMasterTerminal(int iTerminalId)
        {
            _service.SetMasterTerminal(iTerminalId);
        }

        void IInterceptAlohaActivity.SetQuickComboLevel(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId, int nLevel, int nContext)
        {
            _service.SetQuickComboLevel(iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId, nLevel, nContext);
        }
    }
}
