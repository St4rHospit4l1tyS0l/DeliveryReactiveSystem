using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AdmInterceptActivity;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using LasaFOHLib67;

namespace Drs.PosService.BsLogic
{
    public class PosActService : IPosActService
    {
        public const int MAX_TRIES = 3;
        public const int INTERNAL_CHECKS = 540;
        public const int INTERNAL_CHECKS_ENTRIES = 542;
        public const int INTERNAL_ENTRIES_ITEM_DATA = 562;
        private int _iTries;

        //public static int GetCheckNumber(int iCheckId)
        //{
        //    long lDecodeTerm = iCheckId >> 20;
        //    long iDecodeRel = iCheckId & 0xFFFFF;
        //    return Convert.ToInt32(lDecodeTerm * 10000 + iDecodeRel);
        //}

        private void GetInternalItems(int iCheckId)
        {
            var pDepot = new IberDepot();
            try
            {
                _iTries = 0;
                var posCheck = new PosCheck();
                //var sItems = String.Empty;
                var lstItems = new List<ItemModel>();
                ItemModel lastItemModelLvl0 = null;
                ItemModel lastItemModelLvl1 = null;

                foreach (IIberObject chkObject in pDepot.FindObjectFromId(INTERNAL_CHECKS, iCheckId))
                {
                    foreach (IIberObject objItem in chkObject.GetEnum(INTERNAL_CHECKS_ENTRIES))
                    {
                        //foreach (IIberObject objItem in entryObject.GetEnum(INTERNAL_ENTRIES_ITEM_DATA))
                        //{
                        var idItem = objItem.GetLongVal("ID");
                        var itemName = objItem.GetStringVal("DISP_NAME");
                        var price = objItem.GetDoubleVal("PRICE");
                        var level = objItem.GetLongVal("LEVEL");
                        var item = new ItemModel { ItemId = idItem, Name = itemName, IsIdSpecified = true, Price = price, Level = level };
                        switch (level)
                        {
                            case 0:
                                lastItemModelLvl0 = item;
                                break;
                            case 1:
                                lastItemModelLvl1 = item;
                                item.Parent = lastItemModelLvl0;
                                break;
                            default:
                                item.Parent = lastItemModelLvl1;
                                break;
                        }
                        lstItems.Add(item);
                        //sItems = sItems + String.Format(" *** Id: {0} Name: {1}", idItem, itemName);
                        //}
                    }

                    posCheck.CheckId = iCheckId;
                    posCheck.GuidId = Guid.NewGuid();
                    var dValue = chkObject.GetDoubleVal("SUBTOTAL");
                    posCheck.SubTotal = dValue;
                    dValue = chkObject.GetDoubleVal("TAX");
                    posCheck.Tax = dValue;
                    dValue = chkObject.GetDoubleVal("COMPLETETOTAL");
                    posCheck.Total = dValue;
                    posCheck.LstItems = lstItems;

                    SendPosCheckInfo(posCheck);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.StackTrace);
            }
        }

        private void SendPosCheckInfo(PosCheck posCheck)
        {
            Client.ExecutionProxy.ExecuteRequest<PosCheck, PosCheck, ResponseMessageData<bool>, ResponseMessageData<bool>>
                (posCheck, TransferDto.SameType, SharedConstants.Server.POS_RECEIVER_HUB, SharedConstants.Server.ORDER_POS_RECEIVER_HUB_METHOD, TransferDto.SameType)
                .Subscribe(x => OnSendPosOk(x, posCheck), x => OnSendPosError(x, posCheck));
        }

        private void OnSendPosError(Exception ex, PosCheck posCheck)
        {
            OnSendPosError(ex.Message, posCheck);
        }

        private void OnSendPosError(string msgError, PosCheck posCheck)
        {
            MessageBox.Show("Error pos: " + msgError);

            if (_iTries++ > MAX_TRIES)
                return;

            SendPosCheckInfo(posCheck);
        }

        private void OnSendPosOk(IStale<ResponseMessageData<bool>> obj, PosCheck posCheck)
        {
            if (obj.IsStale)
            {
                OnSendPosError("Error de red", posCheck);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnSendPosError(obj.Data.Message, posCheck);
                return;
            }
        }

        public void LogOut(int iEmployeeId, string sName)
        {

        }

        public void ClockIn(int iEmployeeId, string sEmpName, int iJobcodeId, string sJobName)
        {

        }

        public void ClockOut(int iEmployeeId, string sEmpName)
        {

        }

        public void OpenTable(int iEmployeeId, int iQueueId, int iTableId, int iTableDefId, string sName)
        {
        }

        public void CloseTable(int iemployeeId, int iqueueId, int itableId)
        {

        }

        public void OpenCheck(int iemployeeId, int iqueueId, int itableId, int icheckId)
        {

        }

        public void CloseCheck(int iemployeeId, int iqueueId, int itableId, int icheckId)
        {
            GetInternalItems(icheckId);
        }

        public void TransferTable(int ifromEmployeeId, int itoEmployeeId, int itableId, string sNewName, int iIsGetCheck)
        {

        }

        public void AcceptTable(int iemployeeId, int ifromTableId, int itoTableId)
        {

        }

        public void SaveTab(int iemployeeId, int itableId, string sName)
        {

        }

        public void AddTab(int iemployeeId, int ifromTableId, int itoTableId)
        {

        }

        public void NameOrder(int iemployeeId, int iqueueId, int itableId, string sName)
        {

        }
        public void NameOrder(int iEmployeeId, int iQueueId, int iTableId, string sName, int iCheckId)
        {

        }

        public void Bump(int iTableId)
        {

        }

        public void AddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {
            //MessageBox.Show("AddItem(): Estamos enviando los items agregados AddItem10: " + iEntryId);
        }

        public void ModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {

        }

        public void OrderItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iModeId)
        {

        }

        public void HoldItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {

        }

        public void OpenItem(int iEmployeeId, int iEntryId, int iItemId, string sDescription, double dPrice)
        {

        }

        public void SpecialMessage(int iEmployeeId, int iMessageId, string sMessage)
        {

        }

        public void DeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {

        }

        public void UpdateItems(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {

        }

        public void ApplyPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {

        }

        public void AdjustPayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {

        }

        public void DeletePayment(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iTenderId, int iPaymentId)
        {

        }

        public void ApplyComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {

        }

        public void DeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {

        }

        public void ApplyPromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {

        }

        public void DeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {

        }

        public void Custom(string sName)
        {

        }

        public void Startup(int iHMainWnd)
        {

        }

        public void InitializationComplete()
        {

        }

        public void Shutdown()
        {

        }

        public void CarryoverId(int iType, int iOldId, int iNewId)
        {

        }

        public void EndOfDay(int iIsMaster)
        {

        }

        public void CombineOrder(int iEmployeeId, int iSrcQueueId, int iSrcTableId, int iSrcCheckId, int iDstQueueId, int iDstTableId, int iDstCheckId)
        {

        }

        public void OnClockTick()
        {

        }

        public void PreModifyItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {

        }

        public void LockOrder(int iTableId)
        {

        }

        public void UnlockOrder(int iTableId)
        {

        }

        public void SetMasterTerminal(int iTerminalId)
        {

        }

        public void SetQuickComboLevel(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId, int iLevel, int iContext)
        {

        }

        public void TableToShowOnDispBChanged(int iTermId, int iTableId)
        {

        }

        public void StartAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId, int iParentEntryId, int iModCodeId, int iItemId, string sItemName, double dItemPrice)
        {

        }

        public void CancelAddItem(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {

        }

        public void PostDeleteItems(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iReasonId)
        {

        }

        public void PostDeleteComp(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iCompTypeId, int iCompId)
        {

        }

        public void PostDeletePromo(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iPromotionId, int iPromoId)
        {

        }

        public void OrderScreenTableCheckSeatChanged(int iManagerId, int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iSeatNum)
        {

        }

        public void EventNotification(int iEmployeeId, int iTableId, ALOHA_ACTIVITY_EVENT_NOTIFICATION_TYPES eventNotification)
        {

        }

        public void RerouteDisplayBoard(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iDisplayBoardId, int iControllingTerminalId, int iDefaultOrderModeOverride, int currentOrderOnly)
        {

        }

        public void ChangeItemSize(int iEmployeeId, int iQueueId, int iTableId, int iCheckId, int iEntryId)
        {

        }

        public void AdvanceOrder(int iEmployeeId, int iQueueId, int iTableId)
        {

        }

        public void EnrollEmployee(int iEmployeeId, int iNumTries)
        {

        }

        public void MasterDown()
        {

        }

        public void AmMaster()
        {

        }

        public void FileServerDown()
        {

        }

        public void FileServer(string sServerName)
        {

        }

        public void SettleInfoChanged(string sSettleInfo)
        {

        }

        public void SplitCheck(int iCheckId, int iTableId, int iQueueId, int iEmployeeNumber, int iNumberOfSplits, int iSplitType)
        {

        }

        public void AuthorizePayment(int iTableId, int iCheckId, int iPaymentId, int iTransactionType, int iTransactionResult)
        {

        }

        public void CurrentCheckChanged(int iTermId, int iTableId, int iCheckId)
        {

        }

        public void FinalBump(int iTableId)
        {

        }

        public void AssignCashDrawer(int iEmployeeId, int iDrawerId, int iIsPublic)
        {

        }

        public void ReassignCashDrawer(int iEmployeeId, int iDrawerId)
        {

        }

        public void DeassignCashDrawer(int iEmployeeId, int iDrawerId, int iIsPublic)
        {

        }

        public void ReopenCheck(int iEmployeeId, int iQueueId, int iTableId, int iCheckId)
        {

        }

        public void EnterIberScreen(int iTermId, int iScreenId)
        {

        }

        public void ExitIberScreen(int iTermId, int iScreenId)
        {

        }

        public void LogIn(int iEmployeeId, string sName)
        {

        }

        public void IamMaster()
        {

        }

        public void XOpenCheck(int employeeId, int queueId, int tableId, int checkId)
        {

        }

        public void KitchenOrderStatus(string sOrders)
        {

        }

        public void RenameTab(int iTermId, int iCheckId, string stabName)
        {

        }

        public IReactiveDeliveryClient Client { get; set; }
    }
}
