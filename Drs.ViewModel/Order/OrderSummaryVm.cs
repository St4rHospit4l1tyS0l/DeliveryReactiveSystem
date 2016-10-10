using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Drs.Model.Address;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Store;
using Drs.Service.Client;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public class OrderSummaryVm : UcViewModelBase, IOrderSummaryVm
    {

        private readonly OrderSummaryItem _phoneView;
        private readonly OrderSummaryItem _franchiseView;
        private readonly OrderSummaryItem _clientView;
        private readonly OrderSummaryItem _addressView;
        private readonly OrderSummaryItem _posCheckView;
        private readonly OrderSummaryItem _storeView;
        private readonly List<OrderSummaryItem> _lstOrderSum;

        public OrderSummaryVm()
        {
            _phoneView = new OrderSummaryItem();
            _franchiseView = new OrderSummaryItem();
            _clientView = new OrderSummaryItem();
            _addressView = new OrderSummaryItem();
            _storeView = new OrderSummaryItem();
            _posCheckView = new OrderSummaryItem();
            _lstOrderSum = new List<OrderSummaryItem> { _phoneView, _franchiseView, _clientView, _addressView, _storeView, _posCheckView };

            _phoneView.RetrySave = ReactiveCommand.CreateAsyncTask(Observable.Return(true), async _ =>
            {
                await Task.Run(() => OrderService.SavePhoneInformation());
                return new Unit();
            });

            _posCheckView.RetrySave = ReactiveCommand.CreateAsyncTask(Observable.Return(true), async _ =>
            {
                await Task.Run(() => OrderService.SavePosOrder());
                return new Unit();
            });

        }

        public override bool Initialize(bool bForceToInit = false, string parameters = null)
        {
            foreach (var sumItem in _lstOrderSum)
            {
                sumItem.ResetItem();
            }

            return base.Initialize(bForceToInit);
        }

        public void OnPhoneChanged(PhoneModel modelPhone)
        {
            OnItemChanged(PhoneView, modelPhone.Status, modelPhone.Phone, modelPhone.Message);
        }

        public void OnFranchiseChanged(FranchiseInfoModel franchiseInfo)
        {
            OnItemChanged(FranchiseView, SharedConstants.Client.RECORD_SAVED, franchiseInfo.Title);
        }

        public void OnClientSelected(ClientInfoGrid clientInfo)
        {
            if (clientInfo == null)
                OnItemChanged(ClientView, SharedConstants.Client.RECORD_ERROR_SAVED, String.Empty, String.Empty);
            else
                OnItemChanged(ClientView, SharedConstants.Client.RECORD_SAVED,
                    String.Format("{0} {1}", clientInfo.ClientInfo.FirstName, clientInfo.ClientInfo.LastName), String.Empty);
        }

        public void OnAddressSelected(AddressInfoGrid info)
        {
            if (info == null)
                OnItemChanged(AddressView, SharedConstants.Client.RECORD_ERROR_SAVED, String.Empty, String.Empty);
            else
                OnItemChanged(AddressView, SharedConstants.Client.RECORD_SAVED,
                    String.Format("{0} {1}", info.AddressInfo.MainAddress, info.AddressInfo.ExtIntNumber), String.Empty);
        }

        public void OnPosOrderChanged(PosCheck posCheck)
        {
            var checkInfo = String.Empty;
            var checkTotal = String.Empty;

            if (posCheck.Status == SharedConstants.Client.RECORD_SAVED)
            {
                checkInfo = String.Format("ID Orden POS: {0}", posCheck.CheckId);
                checkTotal = String.Format("Total: {0}", posCheck.TotalTx);
            }

            OnItemChanged(PosCheckView, posCheck.Status, checkInfo, posCheck.Message, checkTotal);
        }

        public void OnStoreSelected(StoreModel obj, string sMsg, bool pendingStatus)
        {
            if (obj == null)
            {
                OnItemChanged(StoreView, SharedConstants.Client.RECORD_ERROR_SAVED, String.Empty, sMsg);
            }
            else
            {
                if (pendingStatus)
                {
                    var stData = String.IsNullOrWhiteSpace(obj.ExtraMsg) ? obj.MainAddress : String.Format("{0}{1}{2}", obj.MainAddress, Environment.NewLine, obj.ExtraMsg);

                    OnItemChanged(StoreView, SharedConstants.Client.RECORD_ONPROGRESS_TO_PROCESS,
                        String.Format("{0} - {1}", obj.Value, stData), sMsg);
                }
                else
                {
                    OnItemChanged(StoreView, SharedConstants.Client.RECORD_SAVED,
                        String.Format("{0} - {1}{2}{3}", obj.Value, obj.MainAddress, Environment.NewLine, obj.ExtraMsg));
                }

            }
        }


        private void OnItemChanged(OrderSummaryItem sumItem, int status, string value, string message = "", string secValue = "")
        {
            switch (status)
            {
                case SharedConstants.Client.RECORD_ERROR_SAVED:
                    sumItem.IsOk = Visibility.Collapsed;
                    sumItem.IsInProgress = Visibility.Collapsed;
                    sumItem.IsError = Visibility.Visible;
                    sumItem.IsSaveInProgress = Visibility.Collapsed;
                    sumItem.FirstValue = String.Empty;
                    sumItem.SecondValue = String.Empty;
                    sumItem.MsgErr = message;
                    break;
                case SharedConstants.Client.RECORD_ONPROGRESS_TO_SAVED:
                    sumItem.IsOk = Visibility.Hidden;
                    sumItem.IsInProgress = Visibility.Collapsed;
                    sumItem.IsError = Visibility.Collapsed;
                    sumItem.IsSaveInProgress = Visibility.Visible;
                    break;
                case SharedConstants.Client.RECORD_SAVED:
                    sumItem.IsOk = Visibility.Visible;
                    sumItem.IsInProgress = Visibility.Collapsed;
                    sumItem.IsError = Visibility.Collapsed;
                    sumItem.IsSaveInProgress = Visibility.Collapsed;
                    sumItem.FirstValue = value;
                    sumItem.SecondValue = secValue;
                    sumItem.MsgErr = String.Empty;
                    break;
                case SharedConstants.Client.RECORD_ONPROGRESS_TO_PROCESS:
                    sumItem.IsOk = Visibility.Collapsed;
                    sumItem.IsInProgress = Visibility.Visible;
                    sumItem.IsError = Visibility.Collapsed;
                    sumItem.IsSaveInProgress = Visibility.Collapsed;
                    sumItem.FirstValue = value;
                    sumItem.SecondValue = secValue;
                    sumItem.MsgErr = message;
                    break;
            }
        }

        public IMainOrderService OrderService { get; set; }

        public OrderSummaryItem PhoneView
        {
            get { return _phoneView; }
        }


        public OrderSummaryItem FranchiseView
        {
            get { return _franchiseView; }
        }

        public OrderSummaryItem ClientView
        {
            get { return _clientView; }
        }

        public OrderSummaryItem AddressView
        {
            get { return _addressView; }
        }

        public OrderSummaryItem PosCheckView
        {
            get { return _posCheckView; }
        }

        public OrderSummaryItem StoreView
        {
            get { return _storeView; }
        }
    }
}
