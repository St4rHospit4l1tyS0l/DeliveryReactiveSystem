using AdmInterceptActivity;
using Drs.Service.ReactiveDelivery;

namespace Drs.PosService.BsLogic
{
    public interface IPosActService
    {
        void LogOut(int iEmployeeId, string sName);
        void ClockIn(int iEmployeeId, string sEmpName, int iJobcodeId, string sJobName);
        void ClockOut(int iEmployeeId, string sEmpName);
        void OpenTable(int iemployeeId, int iqueueId, int itableId, int itableDefId, string sName);
        void CloseTable(int iemployeeId, int iqueueId, int itableId);
        void OpenCheck(int iemployeeId, int iqueueId, int itableId, int icheckId);
        void CloseCheck(int iemployeeId, int iqueueId, int itableId, int icheckId);
        void TransferTable(int ifromEmployeeId, int itoEmployeeId, int itableId, string sNewName, int iIsGetCheck);
        void AcceptTable(int iemployeeId, int ifromTableId, int itoTableId);
        void SaveTab(int iemployeeId, int itableId, string sName);
        void AddTab(int iemployeeId, int ifromTableId, int itoTableId);
        void NameOrder(int iemployeeId, int iqueueId, int itableId, string sName);
        void NameOrder(int iEmployeeId, int iQueueId, int iTableId, string sName, int iCheckId);
        void Bump(int iTableId);
        void AddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId);
        void ModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId);
        void OrderItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iModeId);
        void HoldItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId);
        void OpenItem(int iEmployeeId, int iEntryId, int iItemId, string sDescription, double dPrice);
        void SpecialMessage(int iEmployeeId, int iMessageId, string sMessage);
        void DeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId);
        void UpdateItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId);
        void ApplyPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId);
        void AdjustPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId);
        void DeletePayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId);
        void ApplyComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId);
        void DeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId);
        void ApplyPromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId);
        void DeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId);
        void Custom(string sName);
        void Startup(int iHMainWnd);
        void InitializationComplete();
        void Shutdown();
        void CarryoverId(int iType, int iOldId, int iNewId);
        void EndOfDay(int iIsMaster);
        void CombineOrder(int iEmployeeId, int iSrcQueueId, int iSrcTableId, int iSrcCheckId, int iDstQueueId, int iDstTableId, int iDstCheckId);
        void OnClockTick();
        void PreModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId);
        void LockOrder(int iTableId);
        void UnlockOrder(int iTableId);
        void SetMasterTerminal(int iTerminalId);
        void SetQuickComboLevel(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId, int iLevel, int iContext);
        void TableToShowOnDispBChanged(int iTermId, int iTableId);
        void StartAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId, int iParentEntryId, int iModCodeId, int iItemId, string sItemName, double dItemPrice);
        void CancelAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId);
        void PostDeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId);
        void PostDeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId);
        void PostDeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId);
        void EventNotification(int iEmployeeId, int iTableId, ALOHA_ACTIVITY_EVENT_NOTIFICATION_TYPES eventNotification);
        void RerouteDisplayBoard(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iDisplayBoardId, int iControllingTerminalId, int iDefaultOrderModeOverride, int currentOrderOnly);
        void ChangeItemSize(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId);
        void AdvanceOrder(int iEmployeeId, int iQueueId, int iTableId);
        void EnrollEmployee(int iEmployeeId, int iNumTries);
        void MasterDown();
        void AmMaster();
        void FileServerDown();
        void FileServer(string sServerName);
        void SettleInfoChanged(string sSettleInfo);
        void SplitCheck(int iCheckId, int iTableId, int iQueueId, int iEmployeeNumber, int iNumberOfSplits, int iSplitType);
        void AuthorizePayment(int iTableId, int iCheckId, int iPaymentId, int iTransactionType, int iTransactionResult);
        void CurrentCheckChanged(int iTermId, int iTableId, int iCheckId);
        void FinalBump(int iTableId);
        void AssignCashDrawer(int iEmployeeId, int iDrawerId, int iIsPublic);
        void ReassignCashDrawer(int iEmployeeId, int iDrawerId);
        void DeassignCashDrawer(int iEmployeeId, int iDrawerId, int iIsPublic);
        void ReopenCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId);
        void EnterIberScreen(int iTermId, int iScreenId);
        void ExitIberScreen(int iTermId, int iScreenId);
        void LogIn(int iEmployeeId, string sName);
        void OrderScreenTableCheckSeatChanged(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iSeatNum);
        void IamMaster();
        void XOpenCheck(int employeeId, int queueId, int tableId, int checkId);
        void KitchenOrderStatus(string sOrders);
        void RenameTab(int iTermId, int iCheckId, string stabName);
        //IReactiveDeliveryClient Client { get; set; }
        IReactiveDeliveryClient Client { get; set; }
    }
}
