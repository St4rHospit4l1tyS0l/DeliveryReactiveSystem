using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Infrastructure.Extensions.Proc;
using Drs.Model.Catalog;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Service.Client;
using Drs.Service.ReactiveDelivery;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public class SendOrderVm : UcViewModelBase, ISendOrderVm
    {
        private PosCheck _posCheck;
        private string _eventsMsg;
        private Visibility _hasError;
        private Visibility _hasSuccess;
        private string _errorMsg;
        private string _successMsg;
        private Visibility _isSending;
        private Visibility _isReadyToSend;
        private string _sendOrderTitleBtn;
        private bool _immediateDelivery;
        private bool _futureDelivery;
        private Visibility _futureOrderVisibility;
        private string _extraNotes;
        private string _promiseTimeTx;
        private DateTime _promiseTime;
        private DateTime _minDateTime;
        private ItemCatalog _payment;
        private ItemCatalog _pickUpStore;
        private string _currentFranchiseCode;
        private bool _hasPickUpInStore;
        private bool _hasSendFromStore;
        private bool _hasEnableStores;

        public SendOrderVm(IReactiveDeliveryClient client)
        {
            client.HubListeners.SendToStoreEventChanged += OnSendOrderToStoreEventChanged;
            EventsMsg = "Enviando el pedido al servidor. \nEspere por favor...";
            CultureSystem = SettingsData.CultureSystem;

            LstPayments = new ReactiveList<ItemCatalog>();
            LstPayments.ClearAndAddRange(CatalogsClientModel.CatPayments);

            LstStores = new ReactiveList<ItemCatalog>();
            ResetValues();
            SendOrderToStore = ReactiveCommand.CreateAsyncTask(Observable.Return(true), async _ => await SendOrderTask());
            MessageBus.Current.Listen<PropagateOrderModel>(SharedMessageConstants.PROPAGATE_LASTORDER_POSCHECK).Subscribe(OnPropagate);
        }

        private async Task<Unit> SendOrderTask()
        {
            EventsMsg = "Enviando el pedido al servidor. \nEspere por favor...";
            HasError = Visibility.Collapsed;
            ErrorMsg = SuccessMsg = String.Empty;
            HasSuccess = Visibility.Collapsed;

            var response = ValidateOrderDelivery();
            if (response.IsSuccess == false)
            {
                MessageBus.Current.SendMessage(new MessageBoxSettings
                {
                    Message = response.Message,
                    Title = "Envío de la orden",
                }, SharedMessageConstants.MSG_SHOW_ERRQST);
                return new Unit();
            }

            response = ValidateModel(ClientFlags.ValidateOrder.Phone | ClientFlags.ValidateOrder.Franchise |
                              ClientFlags.ValidateOrder.Client
                              | ClientFlags.ValidateOrder.Address | ClientFlags.ValidateOrder.Order 
                              | ClientFlags.ValidateOrder.StoreAvailable 
                              | ClientFlags.ValidateOrder.OrderSaved);

            if (response.IsSuccess == false)
            {
                MessageBus.Current.SendMessage(new MessageBoxSettings
                {
                    Message = response.Message,
                    Title = "Información faltante",
                }, SharedMessageConstants.MSG_SHOW_ERRQST);
                return new Unit();
            }

            IsReadyToSend = Visibility.Collapsed;
            IsSending = Visibility.Visible;
            SendOrderTitleBtn = "Reenviar pedido a la sucursal";

            var posOrderStatus = ExtractPosOrderStatus();
            var posOrderMode = ExtractPosOrderMode();

            var orderDetails = new OrderDetails
            {
                PosOrderMode = posOrderMode,
                PosOrderStatus = posOrderStatus,
                ExtraNotes = ExtraNotes,
                PromiseTime = _promiseTime,
                Payment = Payment
            };

            await Task.Run(() => OrderService.SendOrderToStore(orderDetails));
            return new Unit();
        }

        private string ExtractPosOrderMode()
        {
            return HasPickUpInStore ? SettingsData.Constants.StoreConst.SENDING_MODE_WALK_IN : SettingsData.Constants.StoreConst.SENDING_MODE_DELIVERY;
        }


        private void OnPropagate(PropagateOrderModel model)
        {
            model.PosCheck.GuidId = Guid.NewGuid();
            model.PosCheck.OrderDateTime = DateTime.Now;
            MessageBus.Current.SendMessage(model.PosCheck, SharedMessageConstants.ORDER_SEND_POSORDER);
        }


        private ResponseMessage ValidateOrderDelivery()
        {
            var response = new ResponseMessage();
            if (ImmediateDelivery)
            {
                response.IsSuccess = true;
                return response;
            }

            if (String.IsNullOrWhiteSpace(PromiseTimeTx))
            {
                response.IsSuccess = false;
                response.Message = "Es necesario definir la fecha y hora futura de entrega";
                return response;
            }

            try
            {
                _promiseTime = DateTime.ParseExact(PromiseTimeTx,
                    SettingsData.Constants.SystemConst.FORMAT_DATETIME_V1, CultureInfo.CreateSpecificCulture(SettingsData.CultureSystem));
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = "El formato de la fecha y hora son incorrectos";
                return response;
            }

            if (DateTime.Now.AddMinutes(SettingsData.MinutesToBeFutureOrder) > _promiseTime)
            {
                response.IsSuccess = false;
                response.Message = String.Format("La fecha y hora del pedido a futuro no puede ser menor a {0} minutos del tiempo actual", SettingsData.MinutesToBeFutureOrder);
                return response;
            }

            response.IsSuccess = true;
            return response;
        }

        private int ExtractPosOrderStatus()
        {
            if (ImmediateDelivery)
                return SettingsData.Constants.StoreConst.MODE_DELIVERY_IMMEDIATE;

            if (FutureDelivery)
                return SettingsData.Constants.StoreConst.MODE_DELIVERY_FUTURE;

            return SettingsData.Constants.StoreConst.MODE_DELIVERY_IMMEDIATE;
        }

        private void ResetValues()
        {
            SendOrderTitleBtn = "Enviar pedido a la sucursal";

            IsSending = Visibility.Collapsed;
            IsReadyToSend = Visibility.Collapsed;
            HasError = Visibility.Collapsed;
            HasSuccess = Visibility.Collapsed;

            ImmediateDelivery = true;
            FutureDelivery = false;
            FutureOrderVisibility = Visibility.Collapsed;
            ExtraNotes = String.Empty;

            MinDateTime = DateTime.Now;

            EventsMsg = String.Empty;
            ErrorMsg = String.Empty;
            SuccessMsg = String.Empty;

            if (LstPayments.Count > 0)
                Payment = LstPayments[0];

            _currentFranchiseCode = String.Empty;

            HasPickUpInStore = false;
            HasSendFromStore = false;
        }

        public override bool Initialize(bool bForceToInit = false, string parameters = null)
        {
            ResetValues();
            return base.Initialize(bForceToInit);
        }

        private void OnSendOrderToStoreEventChanged(ResponseMessage resMsg)
        {
            if (resMsg.IsSuccess)
            {
                switch (resMsg.Code)
                {
                    case SettingsData.Constants.StoreConst.STORE_RESPONSE_ORDER_OK:
                        MessageBus.Current.SendMessage(new MessageBoxSettings
                        {
                            Message = resMsg.Message,
                            Title = "Estado de la orden",
                            Callback = (_ => EndOrder())
                        }, SharedMessageConstants.MSG_SHOW_SUCCESS);
                        return;
                    case SettingsData.Constants.StoreConst.STORE_RESPONSE_FAILURE:
                        MessageBus.Current.SendMessage(new MessageBoxSettings
                        {
                            Message = resMsg.Message,
                            Title = "Estado de la orden",
                        }, SharedMessageConstants.MSG_SHOW_ERRQST);
                        OnSendOrderToStoreStatusChanged(new OrderModelDto { HasError = true });
                        return;
                }
                EventsMsg = resMsg.Message;
            }
            else
            {
                switch (resMsg.Code)
                {
                    case SettingsData.Constants.StoreConst.STORE_RESPONSE_PING_ERROR:
                    case SettingsData.Constants.StoreConst.STORE_RESPONSE_FAILURE:
                        MessageBus.Current.SendMessage(new MessageBoxSettings
                        {
                            Message = resMsg.Message,
                            Title = "Estado de la orden",
                        }, SharedMessageConstants.MSG_SHOW_ERRQST);
                        OnSendOrderToStoreStatusChanged(new OrderModelDto { HasError = true });
                        break;
                }
                EventsMsg = resMsg.Message;
            }
        }


        public override ResponseMessage OnViewSelected(int iSelectedTab)
        {
            IsReadyToSend = Visibility.Collapsed;

            var response = ValidateModel(ClientFlags.ValidateOrder.Phone | ClientFlags.ValidateOrder.Franchise | ClientFlags.ValidateOrder.Client
                | ClientFlags.ValidateOrder.Address | ClientFlags.ValidateOrder.Order);

            if (response.IsSuccess)
            {
                ProcessExt.ForceSetFocusThisApp();
                IsReadyToSend = Visibility.Visible;
            }

            return response;
        }

        public event Action EndOrder;

        protected virtual void OnEndOrder()
        {
            var handler = EndOrder;
            if (handler != null) handler();
        }

        public event Action<ItemCatalog, bool> ChangeStore;

        protected virtual void OnChangeStore(ItemCatalog item, bool bIsLastStore)
        {
            var handler = ChangeStore;
            if (handler != null) handler(item, false);
        }

        public event Action UndoPickUpInStore;

        protected virtual void OnUndoPickUpInStore()
        {
            var handler = UndoPickUpInStore;
            if (handler != null) handler();
        }

        public void OnSendOrderToStoreStatusChanged(OrderModelDto model)
        {
            if (model.HasError)
            {
                ErrorMsg = model.Message;
                HasSuccess = Visibility.Collapsed;
                HasError = Visibility.Visible;
                IsReadyToSend = Visibility.Visible;
                IsSending = Visibility.Collapsed;
            }
            else
            {
                if (model.IsAlreadyOnStore)
                {
                    HasSuccess = Visibility.Visible;
                    SuccessMsg = model.Message;
                    IsSending = Visibility.Collapsed;
                }
                else
                {
                    HasSuccess = Visibility.Collapsed;
                    EventsMsg = model.Message;
                    IsSending = Visibility.Visible;
                }

                HasError = Visibility.Collapsed;
                IsReadyToSend = Visibility.Collapsed;
            }
        }

        public void OnPosOrderChanged(PosCheck posCheck)
        {
            PosCheck = posCheck;

            if (_currentFranchiseCode == OrderService.OrderModel.Franchise.Code)
                return;

            _currentFranchiseCode = OrderService.OrderModel.Franchise.Code;

            List<ItemCatalog> lstCatalogs;
            CatalogsClientModel.DicFranchiseStore.TryGetValue(_currentFranchiseCode, out lstCatalogs);

            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                LstStores.ClearAndAddRange(lstCatalogs);
                if (OrderService.OrderModel.StoreModel != null && OrderService.OrderModel.StoreModel.IdKey.HasValue)
                    PickUpStore = LstStores.FirstOrDefault(e => e.Id == OrderService.OrderModel.StoreModel.IdKey.Value);
            });
        }

        private ItemCatalog SelectLastStoreByAddress()
        {
            if (OrderService == null || OrderService.OrderModel == null || OrderService.OrderModel.LastStoreModelByClientAddress == null)
                return null;

            return LstStores.FirstOrDefault(e => e.Id == OrderService.OrderModel.LastStoreModelByClientAddress.IdKey);
        }

        public Func<ClientFlags.ValidateOrder, ResponseMessage> ValidateModel { get; set; }
        public IReactiveCommand<Unit> SendOrderToStore { get; set; }
        public IMainOrderService OrderService { get; set; }

        public ItemCatalog Payment
        {
            get { return _payment; }
            set { this.RaiseAndSetIfChanged(ref _payment, value); }
        }

        public string SendOrderTitleBtn
        {
            get { return _sendOrderTitleBtn; }
            set { this.RaiseAndSetIfChanged(ref _sendOrderTitleBtn, value); }
        }

        public Visibility IsSending
        {
            get { return _isSending; }
            set { this.RaiseAndSetIfChanged(ref _isSending, value); }
        }

        public Visibility IsReadyToSend
        {
            get { return _isReadyToSend; }
            set { this.RaiseAndSetIfChanged(ref _isReadyToSend, value); }
        }

        public string EventsMsg
        {
            get { return _eventsMsg; }
            set { this.RaiseAndSetIfChanged(ref _eventsMsg, value); }
        }

        public Visibility HasError
        {
            get { return _hasError; }
            set { this.RaiseAndSetIfChanged(ref _hasError, value); }
        }

        public Visibility HasSuccess
        {
            get { return _hasSuccess; }
            set { this.RaiseAndSetIfChanged(ref _hasSuccess, value); }
        }

        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { this.RaiseAndSetIfChanged(ref _errorMsg, value); }
        }

        public string SuccessMsg
        {
            get { return _successMsg; }
            set { this.RaiseAndSetIfChanged(ref _successMsg, value); }
        }

        public bool ImmediateDelivery
        {
            get { return _immediateDelivery; }
            set
            {
                this.RaiseAndSetIfChanged(ref _immediateDelivery, value);
                if (value)
                    FutureOrderVisibility = Visibility.Collapsed;
            }
        }

        public bool FutureDelivery
        {
            get { return _futureDelivery; }
            set
            {
                this.RaiseAndSetIfChanged(ref _futureDelivery, value);
                if (value)
                    FutureOrderVisibility = Visibility.Visible;
            }
        }

        public Visibility FutureOrderVisibility
        {
            get { return _futureOrderVisibility; }
            set
            {
                this.RaiseAndSetIfChanged(ref _futureOrderVisibility, value);
            }
        }

        public string ExtraNotes
        {
            get { return _extraNotes; }
            set
            {
                this.RaiseAndSetIfChanged(ref _extraNotes, value);
            }
        }

        public string PromiseTimeTx
        {
            get { return _promiseTimeTx; }
            set
            {
                this.RaiseAndSetIfChanged(ref _promiseTimeTx, value);
            }
        }

        public DateTime MinDateTime
        {
            get { return _minDateTime; }
            set
            {
                this.RaiseAndSetIfChanged(ref _minDateTime, value);
            }
        }

        public string CultureSystem { get; set; }

        public PosCheck PosCheck
        {
            get { return _posCheck; }
            set
            {
                this.RaiseAndSetIfChanged(ref _posCheck, value);
            }
        }
        public IReactiveList<ItemCatalog> LstPayments { get; set; }

        public IReactiveList<ItemCatalog> LstStores { get; set; }

        public bool HasEnableStores
        {
            get { return _hasEnableStores; }
            private set
            {
                this.RaiseAndSetIfChanged(ref _hasEnableStores, value);
            }
        }

        public bool HasPickUpInStore
        {
            get { return _hasPickUpInStore; }
            private set
            {
                this.RaiseAndSetIfChanged(ref _hasPickUpInStore, value);

                if (_hasPickUpInStore)
                    HasSendFromStore = false;

                GetStoreLastSelected();
            }
        }

        public bool HasSendFromStore
        {
            get { return _hasSendFromStore; }
            private set
            {
                this.RaiseAndSetIfChanged(ref _hasSendFromStore, value);

                if (_hasSendFromStore)
                    HasPickUpInStore = false;

                GetStoreLastSelected();
            }
        }

        private void GetStoreLastSelected()
        {
            if (_hasPickUpInStore == false && _hasSendFromStore == false)
                PickUpStore = SelectLastStoreByAddress();
            
            HasEnableStores = _hasPickUpInStore || _hasSendFromStore;
        }

        public ItemCatalog PickUpStore
        {
            get
            {
                return _pickUpStore;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _pickUpStore, value);
                OnChangeStore(_pickUpStore, false);
            }
        }
    }
}
