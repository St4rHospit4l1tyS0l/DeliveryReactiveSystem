using AdmInterceptActivity;

namespace Drs.PosInterface.Com
{
    public partial class InterfacePosActCom
    {

        void IInterceptAlohaActivity3.LogOut(int iEmployeeId, string sName)
        {
            _service.LogOut(iEmployeeId, sName);
        }

        void IInterceptAlohaActivity3.ClockIn(int iEmployeeId, string sEmpName, int iJobcodeId, string sJobName)
        {
            _service.ClockIn(iEmployeeId, sEmpName, iJobcodeId, sJobName);
        }

        void IInterceptAlohaActivity3.ClockOut(int iEmployeeId, string sEmpName)
        {
            _service.ClockOut(iEmployeeId, sEmpName);
        }

        void IInterceptAlohaActivity3.OpenTable(int iEmployeeId, int iQueueId, int iTableId, int iTableDefId, string sName)
        {
            _service.OpenTable(iEmployeeId, iQueueId, iTableId, iTableDefId, sName);
        }

        void IInterceptAlohaActivity3.CloseTable(int iEmployeeId, int iQueueId, int iTableId)
        {
            _service.CloseTable(iEmployeeId, iQueueId, iTableId);
        }

        void IInterceptAlohaActivity3.OpenCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.OpenCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity3.CloseCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.CloseCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity3.TransferTable(int iFromEmployeeId, int iToEmployeeId, int iTableId, string sNewName, int iIsGetCheck)
        {
            _service.TransferTable(iFromEmployeeId, iToEmployeeId, iTableId, sNewName, iIsGetCheck);
        }

        void IInterceptAlohaActivity3.AcceptTable(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AcceptTable(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity3.SaveTab(int iEmployeeId, int iTableId, string sName)
        {
            _service.SaveTab(iEmployeeId, iTableId, sName);
        }

        void IInterceptAlohaActivity3.AddTab(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AddTab(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity3.NameOrder(int iEmployeeId, int iQueueId, int iTableId, string sName)
        {
            _service.NameOrder(iEmployeeId, iQueueId, iTableId, sName);
        }

        void IInterceptAlohaActivity3.Bump(int iTableId)
        {
            _service.Bump(iTableId);
        }

        void IInterceptAlohaActivity3.AddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.AddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity3.ModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.ModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity3.OrderItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iModeId)
        {
            _service.OrderItems(iEmployeeId, iQueueId, iTableId, iCheckId, iModeId);
        }

        void IInterceptAlohaActivity3.HoldItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.HoldItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity3.OpenItem(int iEmployeeId, int iEntryId, int iItemId, string sDescription, double dPrice)
        {
            _service.OpenItem(iEmployeeId, iEntryId, iItemId, sDescription, dPrice);
        }

        void IInterceptAlohaActivity3.SpecialMessage(int iEmployeeId, int iMessageId, string sMessage)
        {
            _service.SpecialMessage(iEmployeeId, iMessageId, sMessage);
        }

        void IInterceptAlohaActivity3.DeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {
            _service.DeleteItems(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iReasonId);
        }

        void IInterceptAlohaActivity3.UpdateItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.UpdateItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity3.ApplyPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.ApplyPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity3.AdjustPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.AdjustPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity3.DeletePayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.DeletePayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity3.ApplyComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.ApplyComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity3.DeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.DeleteComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity3.ApplyPromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.ApplyPromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity3.DeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.DeletePromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity3.Custom(string sName)
        {
            _service.Custom(sName);
        }

        void IInterceptAlohaActivity3.Startup(int hMainWnd)
        {
            _service.Startup(hMainWnd);
        }

        void IInterceptAlohaActivity3.InitializationComplete()
        {
            _service.InitializationComplete();
        }

        void IInterceptAlohaActivity3.Shutdown()
        {
            _service.Shutdown();
        }

        void IInterceptAlohaActivity3.CarryoverId(int iType, int iOldId, int iNewId)
        {
            _service.CarryoverId(iType, iOldId, iNewId);
        }

        void IInterceptAlohaActivity3.EndOfDay(int iIsMaster)
        {
            _service.EndOfDay(iIsMaster);
        }

        void IInterceptAlohaActivity3.CombineOrder(int iEmployeeId, int iSrcQueueId, int iSrcTableId, int iSrcCheckId, int iDstQueueId, int iDstTableId, int iDstCheckId)
        {
            _service.CombineOrder(iEmployeeId, iSrcQueueId, iSrcTableId, iSrcCheckId, iDstQueueId, iDstTableId, iDstCheckId);
        }

        void IInterceptAlohaActivity3.OnClockTick()
        {
            _service.OnClockTick();
        }

        void IInterceptAlohaActivity3.PreModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.PreModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity3.LockOrder(int iTableId)
        {
            _service.LockOrder(iTableId);
        }

        void IInterceptAlohaActivity3.UnlockOrder(int iTableId)
        {
            _service.UnlockOrder(iTableId);
        }

        void IInterceptAlohaActivity3.SetMasterTerminal(int iTerminalId)
        {
            _service.SetMasterTerminal(iTerminalId);
        }

        void IInterceptAlohaActivity3.SetQuickComboLevel(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId, int nLevel, int nContext)
        {
            _service.SetQuickComboLevel(iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId, nLevel, nContext);
        }

        void IInterceptAlohaActivity3.TableToShowOnDispBChanged(int iNTermId, int iTableId)
        {
            _service.TableToShowOnDispBChanged(iNTermId, iTableId);
        }

        void IInterceptAlohaActivity3.StartAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId, int iParentEntryId, int iModCodeId, int iItemId, string sItemName, double dItemPrice)
        {
            _service.StartAddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId, iParentEntryId, iModCodeId, iItemId, sItemName, dItemPrice);
        }

        void IInterceptAlohaActivity3.CancelAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.CancelAddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity3.PostDeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {
            _service.PostDeleteItems(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iReasonId);
        }

        void IInterceptAlohaActivity3.PostDeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.PostDeleteComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity3.PostDeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.PostDeletePromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity3.OrderScreen_TableCheckSeatChanged(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int sSeatNum)
        {
            _service.OrderScreenTableCheckSeatChanged(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, sSeatNum);
        }

        void IInterceptAlohaActivity3.EventNotification(int iEmployeeId, int iTableId, ALOHA_ACTIVITY_EVENT_NOTIFICATION_TYPES eventNotification)
        {
            _service.EventNotification(iEmployeeId, iTableId, eventNotification);
        }

        void IInterceptAlohaActivity3.RerouteDisplayBoard(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iDisplayBoardId, int iControllingTerminalId, int iDefaultOrderModeOverride, int iCurrentOrderOnly)
        {
            _service.RerouteDisplayBoard(iEmployeeId, iQueueId, iTableId, iCheckId, iDisplayBoardId, iControllingTerminalId, iDefaultOrderModeOverride, iCurrentOrderOnly);
        }

        void IInterceptAlohaActivity3.ChangeItemSize(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.ChangeItemSize(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity3.LogIn(int iEmployeeId, string sName)
        {
            _service.LogIn(iEmployeeId, sName);
        }




    }
}
