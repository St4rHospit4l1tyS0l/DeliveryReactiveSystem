using AdmInterceptActivity;

namespace Drs.PosInterface.Com
{
    public partial class InterfacePosActCom
    {
        void IInterceptAlohaActivity9.LogOut(int iEmployeeId, string sName)
        {
            _service.LogOut(iEmployeeId, sName);
        }

        void IInterceptAlohaActivity9.ClockIn(int iEmployeeId, string sEmpName, int iJobcodeId, string sJobName)
        {
            _service.ClockIn(iEmployeeId, sEmpName, iJobcodeId, sJobName);
        }

        void IInterceptAlohaActivity9.ClockOut(int iEmployeeId, string sEmpName)
        {
            _service.ClockOut(iEmployeeId, sEmpName);
        }

        void IInterceptAlohaActivity9.OpenTable(int iEmployeeId, int iQueueId, int iTableId, int iTableDefId, string sName)
        {
            _service.OpenTable(iEmployeeId, iQueueId, iTableId, iTableDefId, sName);
        }

        void IInterceptAlohaActivity9.CloseTable(int iEmployeeId, int iQueueId, int iTableId)
        {
            _service.CloseTable(iEmployeeId, iQueueId, iTableId);
        }

        void IInterceptAlohaActivity9.OpenCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.XOpenCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity9.CloseCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.CloseCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity9.TransferTable(int iFromEmployeeId, int iToEmployeeId, int iTableId, string sNewName, int iIsGetCheck)
        {
            _service.TransferTable(iFromEmployeeId, iToEmployeeId, iTableId, sNewName, iIsGetCheck);
        }

        void IInterceptAlohaActivity9.AcceptTable(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AcceptTable(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity9.SaveTab(int iEmployeeId, int iTableId, string sName)
        {
            _service.SaveTab(iEmployeeId, iTableId, sName);
        }

        void IInterceptAlohaActivity9.AddTab(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AddTab(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity9.NameOrder(int iEmployeeId, int iQueueId, int iTableId, string sName)
        {
            _service.NameOrder(iEmployeeId, iQueueId, iTableId, sName);
        }

        void IInterceptAlohaActivity9.Bump(int iTableId)
        {
            _service.Bump(iTableId);
        }

        void IInterceptAlohaActivity9.AddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.AddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity9.ModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.ModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity9.OrderItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iModeId)
        {
            _service.OrderItems(iEmployeeId, iQueueId, iTableId, iCheckId, iModeId);
        }

        void IInterceptAlohaActivity9.HoldItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.HoldItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity9.OpenItem(int iEmployeeId, int iEntryId, int iItemId, string sDescription, double dPrice)
        {
            _service.OpenItem(iEmployeeId, iEntryId, iItemId, sDescription, dPrice);
        }

        void IInterceptAlohaActivity9.SpecialMessage(int iEmployeeId, int iMessageId, string sMessage)
        {
            _service.SpecialMessage(iEmployeeId, iMessageId, sMessage);
        }

        void IInterceptAlohaActivity9.DeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {
            _service.DeleteItems(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iReasonId);
        }

        void IInterceptAlohaActivity9.UpdateItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.UpdateItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity9.ApplyPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.ApplyPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity9.AdjustPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.AdjustPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity9.DeletePayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.DeletePayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity9.ApplyComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.ApplyComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity9.DeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.DeleteComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity9.ApplyPromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.ApplyPromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity9.DeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.DeletePromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity9.Custom(string sName)
        {
            _service.Custom(sName);
        }

        void IInterceptAlohaActivity9.Startup(int iHMainWnd)
        {
            _service.Startup(iHMainWnd);
        }

        void IInterceptAlohaActivity9.InitializationComplete()
        {
            _service.InitializationComplete();
        }

        void IInterceptAlohaActivity9.Shutdown()
        {
            _service.Shutdown();
        }

        void IInterceptAlohaActivity9.CarryoverId(int iType, int iOldId, int iNewId)
        {
            _service.CarryoverId(iType, iOldId, iNewId);
        }

        void IInterceptAlohaActivity9.EndOfDay(int iIsMaster)
        {
            _service.EndOfDay(iIsMaster);
        }

        void IInterceptAlohaActivity9.CombineOrder(int iEmployeeId, int iSrcQueueId, int iSrcTableId, int iSrcCheckId, int iDstQueueId, int iDstTableId, int iDstCheckId)
        {
            _service.CombineOrder(iEmployeeId, iSrcQueueId, iSrcTableId, iSrcCheckId, iDstQueueId, iDstTableId, iDstCheckId);
        }

        void IInterceptAlohaActivity9.OnClockTick()
        {
            _service.OnClockTick();
        }

        void IInterceptAlohaActivity9.PreModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.PreModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity9.LockOrder(int iTableId)
        {
            _service.LockOrder(iTableId);
        }

        void IInterceptAlohaActivity9.UnlockOrder(int iTableId)
        {
            _service.UnlockOrder(iTableId);
        }

        void IInterceptAlohaActivity9.SetMasterTerminal(int iTerminalId)
        {
            _service.SetMasterTerminal(iTerminalId);
        }

        void IInterceptAlohaActivity9.SetQuickComboLevel(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId, int iNLevel, int iNContext)
        {
            _service.SetQuickComboLevel(iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId, iNLevel, iNContext);
        }

        void IInterceptAlohaActivity9.TableToShowOnDispBChanged(int inTermId, int iTableId)
        {
            _service.TableToShowOnDispBChanged(inTermId, iTableId);
        }

        void IInterceptAlohaActivity9.StartAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId, int iParentEntryId, int iModCodeId, int iItemId, string sItemName, double dItemPrice)
        {
            _service.StartAddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId, iParentEntryId, iModCodeId, iItemId, sItemName, dItemPrice);
        }

        void IInterceptAlohaActivity9.CancelAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.CancelAddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity9.PostDeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {
            _service.PostDeleteItems(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iReasonId);
        }

        void IInterceptAlohaActivity9.PostDeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.PostDeleteComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity9.PostDeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.PostDeletePromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity9.OrderScreen_TableCheckSeatChanged(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iSeatNum)
        {
            _service.OrderScreenTableCheckSeatChanged(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iSeatNum);
        }

        void IInterceptAlohaActivity9.EventNotification(int iEmployeeId, int iTableId, ALOHA_ACTIVITY_EVENT_NOTIFICATION_TYPES eventNotification)
        {
            _service.EventNotification(iEmployeeId, iTableId, eventNotification);
        }

        void IInterceptAlohaActivity9.RerouteDisplayBoard(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iDisplayBoardId, int iControllingTerminalId, int iDefaultOrderModeOverride, int iCurrentOrderOnly)
        {
            _service.RerouteDisplayBoard(iEmployeeId, iQueueId, iTableId, iCheckId, iDisplayBoardId, iControllingTerminalId, iDefaultOrderModeOverride, iCurrentOrderOnly);
        }

        void IInterceptAlohaActivity9.ChangeItemSize(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.ChangeItemSize(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity9.AdvanceOrder(int iEmployeeId, int iQueueId, int iTableId)
        {
            _service.AdvanceOrder(iEmployeeId, iQueueId, iTableId);
        }

        void IInterceptAlohaActivity9.EnrollEmployee(int iEmployeeId, int iNumTries)
        {
            _service.EnrollEmployee(iEmployeeId, iNumTries);
        }

        void IInterceptAlohaActivity9.MasterDown()
        {
            _service.MasterDown();
        }

        void IInterceptAlohaActivity9.IAmMaster()
        {
            _service.IamMaster();
        }

        void IInterceptAlohaActivity9.FileServerDown()
        {
            _service.FileServerDown();
        }

        void IInterceptAlohaActivity9.FileServer(string sServerName)
        {
            _service.FileServer(sServerName);
        }

        void IInterceptAlohaActivity9.SettleInfoChanged(string sSettleInfo)
        {
            _service.SettleInfoChanged(sSettleInfo);
        }

        void IInterceptAlohaActivity9.SplitCheck(int iCheckId, int iTableId, int iQueueId, int iEmployeeNumber, int iNumberOfSplits, int iSplitType)
        {
            _service.SplitCheck(iCheckId, iTableId, iQueueId, iEmployeeNumber, iNumberOfSplits, iSplitType);
        }

        void IInterceptAlohaActivity9.AuthorizePayment(int iTableId, int iCheckId, int iPaymentId, int iTransactionType, int iTransactionResult)
        {
            _service.AuthorizePayment(iTableId, iCheckId, iPaymentId, iTransactionType, iTransactionResult);
        }

        void IInterceptAlohaActivity9.CurrentCheckChanged(int iTermId, int iTableId, int iCheckId)
        {
            _service.CurrentCheckChanged(iTermId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity9.FinalBump(int iTableId)
        {
            _service.FinalBump(iTableId);
        }

        void IInterceptAlohaActivity9.AssignCashDrawer(int iEmployeeId, int iDrawerId, int iIsPublic)
        {
            _service.AssignCashDrawer(iEmployeeId, iDrawerId, iIsPublic);
        }

        void IInterceptAlohaActivity9.ReassignCashDrawer(int iEmployeeId, int iDrawerId)
        {
            _service.ReassignCashDrawer(iEmployeeId, iDrawerId);
        }

        void IInterceptAlohaActivity9.DeassignCashDrawer(int iEmployeeId, int iDrawerId, int iIsPublic)
        {
            _service.DeassignCashDrawer(iEmployeeId, iDrawerId, iIsPublic);
        }

        void IInterceptAlohaActivity9.ReopenCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.ReopenCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity9.LogIn(int iEmployeeId, string sName)
        {
            _service.LogIn(iEmployeeId, sName);
        }


    }
}
