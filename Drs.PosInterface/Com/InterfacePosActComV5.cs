using AdmInterceptActivity;

namespace Drs.PosInterface.Com
{
    public partial class InterfacePosActCom
    {

        void IInterceptAlohaActivity5.LogOut(int iEmployeeId, string sName)
        {
            _service.LogOut(iEmployeeId, sName);
        }

        void IInterceptAlohaActivity5.ClockIn(int iEmployeeId, string sEmpName, int iJobcodeId, string sJobName)
        {
            _service.ClockIn(iEmployeeId, sEmpName, iJobcodeId, sJobName);
        }

        void IInterceptAlohaActivity5.ClockOut(int iEmployeeId, string sEmpName)
        {
            _service.ClockOut(iEmployeeId, sEmpName);
        }

        void IInterceptAlohaActivity5.OpenTable(int iEmployeeId, int iQueueId, int iTableId, int iTableDefId, string sName)
        {
            _service.OpenTable(iEmployeeId, iQueueId, iTableId, iTableDefId, sName);
        }

        void IInterceptAlohaActivity5.CloseTable(int iEmployeeId, int iQueueId, int iTableId)
        {
            _service.CloseTable(iEmployeeId, iQueueId, iTableId);
        }

        void IInterceptAlohaActivity5.OpenCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.OpenCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity5.CloseCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.CloseCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity5.TransferTable(int iFromEmployeeId, int iToEmployeeId, int iTableId, string sNewName, int iIsGetCheck)
        {
            _service.TransferTable(iFromEmployeeId, iToEmployeeId, iTableId, sNewName, iIsGetCheck);
        }

        void IInterceptAlohaActivity5.AcceptTable(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AcceptTable(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity5.SaveTab(int iEmployeeId, int iTableId, string sName)
        {
            _service.SaveTab(iEmployeeId, iTableId, sName);
        }

        void IInterceptAlohaActivity5.AddTab(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AddTab(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity5.NameOrder(int iEmployeeId, int iQueueId, int iTableId, string sName)
        {
            _service.NameOrder(iEmployeeId, iQueueId, iTableId, sName);
        }

        void IInterceptAlohaActivity5.Bump(int iTableId)
        {
            _service.Bump(iTableId);
        }

        void IInterceptAlohaActivity5.AddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.AddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity5.ModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.ModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity5.OrderItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iModeId)
        {
            _service.OrderItems(iEmployeeId, iQueueId, iTableId, iCheckId, iModeId);
        }

        void IInterceptAlohaActivity5.HoldItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.HoldItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity5.OpenItem(int iEmployeeId, int iEntryId, int iItemId, string sDescription, double dPrice)
        {
            _service.OpenItem(iEmployeeId, iEntryId, iItemId, sDescription, dPrice);
        }

        void IInterceptAlohaActivity5.SpecialMessage(int iEmployeeId, int iMessageId, string sMessage)
        {
            _service.SpecialMessage(iEmployeeId, iMessageId, sMessage);
        }

        void IInterceptAlohaActivity5.DeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {
            _service.DeleteItems(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iReasonId);
        }

        void IInterceptAlohaActivity5.UpdateItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.UpdateItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity5.ApplyPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.ApplyPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity5.AdjustPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.AdjustPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity5.DeletePayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.DeletePayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity5.ApplyComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.ApplyComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity5.DeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.DeleteComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity5.ApplyPromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.ApplyPromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity5.DeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.DeletePromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity5.Custom(string sName)
        {
            _service.Custom(sName);
        }

        void IInterceptAlohaActivity5.Startup(int hMainWnd)
        {
            _service.Startup(hMainWnd);
        }

        void IInterceptAlohaActivity5.InitializationComplete()
        {
            _service.InitializationComplete();
        }

        void IInterceptAlohaActivity5.Shutdown()
        {
            _service.Shutdown();
        }

        void IInterceptAlohaActivity5.CarryoverId(int iType, int iOldId, int iNewId)
        {
            _service.CarryoverId(iType, iOldId, iNewId);
        }

        void IInterceptAlohaActivity5.EndOfDay(int iIsMaster)
        {
            _service.EndOfDay(iIsMaster);
        }

        void IInterceptAlohaActivity5.CombineOrder(int iEmployeeId, int iSrcQueueId, int iSrcTableId, int iSrcCheckId, int iDstQueueId, int iDstTableId, int iDstCheckId)
        {
            _service.CombineOrder(iEmployeeId, iSrcQueueId, iSrcTableId, iSrcCheckId, iDstQueueId, iDstTableId, iDstCheckId);
        }

        void IInterceptAlohaActivity5.OnClockTick()
        {
            _service.OnClockTick();
        }

        void IInterceptAlohaActivity5.PreModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.PreModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity5.LockOrder(int iTableId)
        {
            _service.LockOrder(iTableId);
        }

        void IInterceptAlohaActivity5.UnlockOrder(int iTableId)
        {
            _service.UnlockOrder(iTableId);
        }

        void IInterceptAlohaActivity5.SetMasterTerminal(int iTerminalId)
        {
            _service.SetMasterTerminal(iTerminalId);
        }

        void IInterceptAlohaActivity5.SetQuickComboLevel(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId, int nLevel, int nContext)
        {
            _service.SetQuickComboLevel(iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId, nLevel, nContext);
        }

        void IInterceptAlohaActivity5.TableToShowOnDispBChanged(int iNTermId, int iTableId)
        {
            _service.TableToShowOnDispBChanged(iNTermId, iTableId);
        }

        void IInterceptAlohaActivity5.StartAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId, int iParentEntryId, int iModCodeId, int iItemId, string sItemName, double dItemPrice)
        {
            _service.StartAddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId, iParentEntryId, iModCodeId, iItemId, sItemName, dItemPrice);
        }

        void IInterceptAlohaActivity5.CancelAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.CancelAddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity5.PostDeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {
            _service.PostDeleteItems(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iReasonId);
        }

        void IInterceptAlohaActivity5.PostDeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.PostDeleteComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity5.PostDeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.PostDeletePromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity5.OrderScreen_TableCheckSeatChanged(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int sSeatNum)
        {
            _service.OrderScreenTableCheckSeatChanged(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, sSeatNum);
        }

        void IInterceptAlohaActivity5.EventNotification(int iEmployeeId, int iTableId, ALOHA_ACTIVITY_EVENT_NOTIFICATION_TYPES eventNotification)
        {
            _service.EventNotification(iEmployeeId, iTableId, eventNotification);
        }

        void IInterceptAlohaActivity5.RerouteDisplayBoard(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iDisplayBoardId, int iControllingTerminalId, int iDefaultOrderModeOverride, int iCurrentOrderOnly)
        {
            _service.RerouteDisplayBoard(iEmployeeId, iQueueId, iTableId, iCheckId, iDisplayBoardId, iControllingTerminalId, iDefaultOrderModeOverride, iCurrentOrderOnly);
        }

        void IInterceptAlohaActivity5.ChangeItemSize(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.ChangeItemSize(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity5.AdvanceOrder(int iEmployeeId, int iQueueId, int iTableId)
        {
            _service.AdvanceOrder(iEmployeeId, iQueueId, iTableId);
        }

        void IInterceptAlohaActivity5.EnrollEmployee(int iEmployeeId, int numTries)
        {
            _service.EnrollEmployee(iEmployeeId, numTries);
        }

        void IInterceptAlohaActivity5.MasterDown()
        {
            _service.MasterDown();
        }

        void IInterceptAlohaActivity5.IAmMaster()
        {
            _service.IamMaster();
        }

        void IInterceptAlohaActivity5.FileServerDown()
        {
            _service.FileServerDown();
        }

        void IInterceptAlohaActivity5.FileServer(string sServerName)
        {
            _service.FileServer(sServerName);
        }

        void IInterceptAlohaActivity5.SettleInfoChanged(string sSettleInfo)
        {
            _service.SettleInfoChanged(sSettleInfo);
        }

        void IInterceptAlohaActivity5.LogIn(int iEmployeeId, string sName)
        {
            _service.LogIn(iEmployeeId, sName);
        }



    }
}
