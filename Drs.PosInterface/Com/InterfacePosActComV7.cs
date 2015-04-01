using AdmInterceptActivity;

namespace Drs.PosInterface.Com
{
    public partial class InterfacePosActCom
    {

        void IInterceptAlohaActivity7.LogOut(int iEmployeeId, string sName)
        {
            _service.LogOut(iEmployeeId, sName);
        }

        void IInterceptAlohaActivity7.ClockIn(int iEmployeeId, string sEmpName, int iJobcodeId, string sJobName)
        {
            _service.ClockIn(iEmployeeId, sEmpName, iJobcodeId, sJobName);
        }

        void IInterceptAlohaActivity7.ClockOut(int iEmployeeId, string sEmpName)
        {
            _service.ClockOut(iEmployeeId, sEmpName);
        }

        void IInterceptAlohaActivity7.OpenTable(int iEmployeeId, int iQueueId, int iTableId, int iTableDefId, string sName)
        {
            _service.OpenTable(iEmployeeId, iQueueId, iTableId, iTableDefId, sName);
        }

        void IInterceptAlohaActivity7.CloseTable(int iEmployeeId, int iQueueId, int iTableId)
        {
            _service.CloseTable(iEmployeeId, iQueueId, iTableId);
        }

        void IInterceptAlohaActivity7.OpenCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.OpenCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity7.CloseCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.CloseCheck(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity7.TransferTable(int iFromEmployeeId, int iToEmployeeId, int iTableId, string sNewName, int iIsGetCheck)
        {
            _service.TransferTable(iFromEmployeeId, iToEmployeeId, iTableId, sNewName, iIsGetCheck);
        }

        void IInterceptAlohaActivity7.AcceptTable(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AcceptTable(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity7.SaveTab(int iEmployeeId, int iTableId, string sName)
        {
            _service.SaveTab(iEmployeeId, iTableId, sName);
        }

        void IInterceptAlohaActivity7.AddTab(int iEmployeeId, int iFromTableId, int iToTableId)
        {
            _service.AddTab(iEmployeeId, iFromTableId, iToTableId);
        }

        void IInterceptAlohaActivity7.NameOrder(int iEmployeeId, int iQueueId, int iTableId, string sName)
        {
            _service.NameOrder(iEmployeeId, iQueueId, iTableId, sName);
        }

        void IInterceptAlohaActivity7.Bump(int iTableId)
        {
            _service.Bump(iTableId);
        }

        void IInterceptAlohaActivity7.AddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.AddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity7.ModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.ModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity7.OrderItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iModeId)
        {
            _service.OrderItems(iEmployeeId, iQueueId, iTableId, iCheckId, iModeId);
        }

        void IInterceptAlohaActivity7.HoldItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.HoldItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity7.OpenItem(int iEmployeeId, int iEntryId, int iItemId, string sDescription, double dPrice)
        {
            _service.OpenItem(iEmployeeId, iEntryId, iItemId, sDescription, dPrice);
        }

        void IInterceptAlohaActivity7.SpecialMessage(int iEmployeeId, int iMessageId, string sMessage)
        {
            _service.SpecialMessage(iEmployeeId, iMessageId, sMessage);
        }

        void IInterceptAlohaActivity7.DeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {
            _service.DeleteItems(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iReasonId);
        }

        void IInterceptAlohaActivity7.UpdateItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {
            _service.UpdateItems(iEmployeeId, iQueueId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity7.ApplyPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.ApplyPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity7.AdjustPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.AdjustPayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity7.DeletePayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {
            _service.DeletePayment(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iTenderId, iPaymentId);
        }

        void IInterceptAlohaActivity7.ApplyComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.ApplyComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity7.DeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.DeleteComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity7.ApplyPromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.ApplyPromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity7.DeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.DeletePromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity7.Custom(string sName)
        {
            _service.Custom(sName);
        }

        void IInterceptAlohaActivity7.Startup(int hMainWnd)
        {
            _service.Startup(hMainWnd);
        }

        void IInterceptAlohaActivity7.InitializationComplete()
        {
            _service.InitializationComplete();
        }

        void IInterceptAlohaActivity7.Shutdown()
        {
            _service.Shutdown();
        }

        void IInterceptAlohaActivity7.CarryoverId(int iType, int iOldId, int iNewId)
        {
            _service.CarryoverId(iType, iOldId, iNewId);
        }

        void IInterceptAlohaActivity7.EndOfDay(int iIsMaster)
        {
            _service.EndOfDay(iIsMaster);
        }

        void IInterceptAlohaActivity7.CombineOrder(int iEmployeeId, int iSrcQueueId, int iSrcTableId, int iSrcCheckId, int iDstQueueId, int iDstTableId, int iDstCheckId)
        {
            _service.CombineOrder(iEmployeeId, iSrcQueueId, iSrcTableId, iSrcCheckId, iDstQueueId, iDstTableId, iDstCheckId);
        }

        void IInterceptAlohaActivity7.OnClockTick()
        {
            _service.OnClockTick();
        }

        void IInterceptAlohaActivity7.PreModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.PreModifyItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity7.LockOrder(int iTableId)
        {
            _service.LockOrder(iTableId);
        }

        void IInterceptAlohaActivity7.UnlockOrder(int iTableId)
        {
            _service.UnlockOrder(iTableId);
        }

        void IInterceptAlohaActivity7.SetMasterTerminal(int iTerminalId)
        {
            _service.SetMasterTerminal(iTerminalId);
        }

        void IInterceptAlohaActivity7.SetQuickComboLevel(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId, int nLevel, int nContext)
        {
            _service.SetQuickComboLevel(iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId, nLevel, nContext);
        }

        void IInterceptAlohaActivity7.TableToShowOnDispBChanged(int iNTermId, int iTableId)
        {
            _service.TableToShowOnDispBChanged(iNTermId, iTableId);
        }

        void IInterceptAlohaActivity7.StartAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId, int iParentEntryId, int iModCodeId, int iItemId, string sItemName, double dItemPrice)
        {
            _service.StartAddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId, iParentEntryId, iModCodeId, iItemId, sItemName, dItemPrice);
        }

        void IInterceptAlohaActivity7.CancelAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.CancelAddItem(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity7.PostDeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {
            _service.PostDeleteItems(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iReasonId);
        }

        void IInterceptAlohaActivity7.PostDeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {
            _service.PostDeleteComp(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iCompTypeId, iCompId);
        }

        void IInterceptAlohaActivity7.PostDeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {
            _service.PostDeletePromo(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iPromotionId, iPromoId);
        }

        void IInterceptAlohaActivity7.OrderScreen_TableCheckSeatChanged(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iSeatNum)
        {
            _service.OrderScreenTableCheckSeatChanged(iManagerId, iEmployeeId, iQueueId, iTableId, iCheckId, iSeatNum);
        }

        void IInterceptAlohaActivity7.EventNotification(int iEmployeeId, int iTableId, ALOHA_ACTIVITY_EVENT_NOTIFICATION_TYPES eventNotification)
        {
            _service.EventNotification(iEmployeeId, iTableId, eventNotification);
        }

        void IInterceptAlohaActivity7.RerouteDisplayBoard(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iDisplayBoardId, int iControllingTerminalId, int iDefaultOrderModeOverride, int iCurrentOrderOnly)
        {
            _service.RerouteDisplayBoard(iEmployeeId, iQueueId, iTableId, iCheckId, iDisplayBoardId, iControllingTerminalId, iDefaultOrderModeOverride, iCurrentOrderOnly);
        }

        void IInterceptAlohaActivity7.ChangeItemSize(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            _service.ChangeItemSize(iEmployeeId, iQueueId, iTableId, iCheckId, iEntryId);
        }

        void IInterceptAlohaActivity7.AdvanceOrder(int iEmployeeId, int iQueueId, int iTableId)
        {
            _service.AdvanceOrder(iEmployeeId, iQueueId, iTableId);
        }

        void IInterceptAlohaActivity7.EnrollEmployee(int iEmployeeId, int numTries)
        {
            _service.EnrollEmployee(iEmployeeId, numTries);
        }

        void IInterceptAlohaActivity7.MasterDown()
        {
            _service.MasterDown();
        }

        void IInterceptAlohaActivity7.IAmMaster()
        {
            _service.IamMaster();
        }

        void IInterceptAlohaActivity7.FileServerDown()
        {
            _service.FileServerDown();
        }

        void IInterceptAlohaActivity7.FileServer(string sServerName)
        {
            _service.FileServer(sServerName);
        }

        void IInterceptAlohaActivity7.SettleInfoChanged(string sSettleInfo)
        {
            _service.SettleInfoChanged(sSettleInfo);
        }

        void IInterceptAlohaActivity7.SplitCheck(int iCheckId, int iTableId, int iQueueId, int iEmployeeNumber, int iNumberOfSplits, int iSplitType)
        {
            _service.SplitCheck(iCheckId, iTableId, iQueueId, iEmployeeNumber, iNumberOfSplits, iSplitType);
        }

        void IInterceptAlohaActivity7.AuthorizePayment(int iTableId, int iCheckId, int iPaymentId, int iTransactionType, int iTransactionResult)
        {
            _service.AuthorizePayment(iTableId, iCheckId, iPaymentId, iTransactionType, iTransactionResult);
        }

        void IInterceptAlohaActivity7.CurrentCheckChanged(int iTermId, int iTableId, int iCheckId)
        {
            _service.CurrentCheckChanged(iTermId, iTableId, iCheckId);
        }

        void IInterceptAlohaActivity7.LogIn(int iEmployeeId, string sName)
        {
            _service.LogIn(iEmployeeId, sName);
        }



    }
}
