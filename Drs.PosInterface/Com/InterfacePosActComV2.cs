using AdmInterceptActivity;

namespace Drs.PosInterface.Com
{
    public partial class InterfacePosActCom
    {


        void IInterceptAlohaActivity2.LogOut(int iEmployeeId, string sName)
        {
            _service.LogOut(iEmployeeId, sName);
        }

        void IInterceptAlohaActivity2.ClockIn(int iEmployeeId, string sEmpName, int iJobcodeId, string sJobName)
        {
            _service.ClockIn(iEmployeeId, sEmpName, iJobcodeId, sJobName);
        }

        void IInterceptAlohaActivity2.ClockOut(int iEmployeeId, string sEmpName)
        {
            _service.ClockOut(iEmployeeId, sEmpName);
        }

        void IInterceptAlohaActivity2.OpenTable(int iEmployeeId, int iQueueId, int iTableId, int iTableDefId, string sName)
        {
            _service.OpenTable(iEmployeeId, iQueueId, iTableId, iTableDefId, sName);
        }

        void IInterceptAlohaActivity2.CloseTable(int iEmployeeId, int iQueueId, int iTableId)
        {
            _service.CloseTable(iEmployeeId, iQueueId, iTableId);
        }

        void IInterceptAlohaActivity2.OpenCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.OpenCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity2.CloseCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.CloseCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity2.TransferTable(int iFromEmployeeId, int iToEmployeeId, int iTableId, string sNewName, int iIsGetCheck)
        {
            _service.TransferTable(iFromEmployeeId, iToEmployeeId, iTableId, sNewName, iIsGetCheck);
        }

        void IInterceptAlohaActivity2.AcceptTable(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AcceptTable(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity2.SaveTab(int iEmployeeId, int iTableId, string sName)
        {
            _service.SaveTab(iEmployeeId, iTableId, sName);
        }

        void IInterceptAlohaActivity2.AddTab(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AddTab(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity2.NameOrder(int iEmployeeId, int iQueueId, int iTableId, string sName)
        {
            _service.NameOrder(iEmployeeId, iQueueId, iTableId, sName);
        }

        void IInterceptAlohaActivity2.Bump(int iTableId)
        {
            _service.Bump(iTableId);
        }

        void IInterceptAlohaActivity2.AddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.AddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity2.ModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.ModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity2.OrderItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iModeId)
        {
            _service.OrderItems(iEmployeeId, iQueueId, iTableId, iCheckId, iModeId);
        }

        void IInterceptAlohaActivity2.HoldItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.HoldItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity2.OpenItem(int iEmployeeId, int iEntryId, int iItemId, string sDescription, double dPrice)
        {
            _service.OpenItem(iEmployeeId, iEntryId, iItemId, sDescription, dPrice);
        }

        void IInterceptAlohaActivity2.SpecialMessage(int iEmployeeId, int iMessageId, string sMessage)
        {
            _service.SpecialMessage(iEmployeeId, iMessageId, sMessage);
        }

        void IInterceptAlohaActivity2.DeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {
            _service.DeleteItems(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iReasonId);
        }

        void IInterceptAlohaActivity2.UpdateItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.UpdateItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity2.ApplyPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.ApplyPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity2.AdjustPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.AdjustPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity2.DeletePayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.DeletePayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity2.ApplyComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.ApplyComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity2.DeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.DeleteComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity2.ApplyPromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.ApplyPromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity2.DeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.DeletePromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity2.Custom(string sName)
        {
            _service.Custom(sName);
        }

        void IInterceptAlohaActivity2.Startup(int hMainWnd)
        {
            _service.Startup(hMainWnd);
        }

        void IInterceptAlohaActivity2.InitializationComplete()
        {
            _service.InitializationComplete();
        }

        void IInterceptAlohaActivity2.Shutdown()
        {
            _service.Shutdown();
        }

        void IInterceptAlohaActivity2.CarryoverId(int iType, int iOldId, int iNewId)
        {
            _service.CarryoverId(iType, iOldId, iNewId);
        }

        void IInterceptAlohaActivity2.EndOfDay(int iIsMaster)
        {
            _service.EndOfDay(iIsMaster);
        }

        void IInterceptAlohaActivity2.CombineOrder(int iEmployeeId, int iSrcQueueId, int iSrcTableId, int iSrcCheckId, int iDstQueueId, int iDstTableId, int iDstCheckId)
        {
            _service.CombineOrder(iEmployeeId, iSrcQueueId, iSrcTableId, iSrcCheckId, iDstQueueId, iDstTableId, iDstCheckId);
        }

        void IInterceptAlohaActivity2.OnClockTick()
        {
            _service.OnClockTick();
        }

        void IInterceptAlohaActivity2.PreModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.PreModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity2.LockOrder(int iTableId)
        {
            _service.LockOrder(iTableId);
        }

        void IInterceptAlohaActivity2.UnlockOrder(int iTableId)
        {
            _service.UnlockOrder(iTableId);
        }

        void IInterceptAlohaActivity2.SetMasterTerminal(int iTerminalId)
        {
            _service.SetMasterTerminal(iTerminalId);
        }

        void IInterceptAlohaActivity2.SetQuickComboLevel(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId, int nLevel, int nContext)
        {
            _service.SetQuickComboLevel(iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId, nLevel, nContext);
        }

        void IInterceptAlohaActivity2.TableToShowOnDispBChanged(int iNTermId, int iTableId)
        {
            _service.TableToShowOnDispBChanged(iNTermId, iTableId);
        }

        void IInterceptAlohaActivity2.StartAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId, int iParentEntryId, int iModCodeId, int iItemId, string sItemName, double dItemPrice)
        {
            _service.StartAddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId, iParentEntryId, iModCodeId, iItemId, sItemName, dItemPrice);
        }

        void IInterceptAlohaActivity2.CancelAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.CancelAddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity2.PostDeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {
            _service.PostDeleteItems(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iReasonId);
        }

        void IInterceptAlohaActivity2.PostDeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.PostDeleteComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity2.PostDeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.PostDeletePromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity2.OrderScreen_TableCheckSeatChanged(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int sSeatNum)
        {
            _service.OrderScreenTableCheckSeatChanged(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, sSeatNum);
        }

        void IInterceptAlohaActivity2.EventNotification(int iEmployeeId, int iTableId, ALOHA_ACTIVITY_EVENT_NOTIFICATION_TYPES eventNotification)
        {
            _service.EventNotification(iEmployeeId, iTableId, eventNotification);
        }

        void IInterceptAlohaActivity2.RerouteDisplayBoard(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iDisplayBoardId, int iControllingTerminalId, int iDefaultOrderModeOverride, int iCurrentOrderOnly)
        {
            _service.RerouteDisplayBoard(iEmployeeId, iQueueId, iTableId, iCheckId, iDisplayBoardId, iControllingTerminalId, iDefaultOrderModeOverride, iCurrentOrderOnly);
        }

        void IInterceptAlohaActivity2.LogIn(int iEmployeeId, string sName)
        {
            _service.LogIn(iEmployeeId, sName);
        }




    }
}
