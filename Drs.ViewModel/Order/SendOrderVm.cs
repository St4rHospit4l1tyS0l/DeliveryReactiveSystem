using System;
using System.Globalization;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Drs.Infrastructure.Extensions.Proc;
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

        public SendOrderVm(IReactiveDeliveryClient client)
        {
            client.HubListeners.SendToStoreEventChanged += OnSendOrderToStoreEventChanged;
            EventsMsg = "Enviando el pedido al servidor. \nEspere por favor...";
            CultureSystem = SettingsData.CultureSystem;

            ResetValues();

            SendOrderToStore = ReactiveCommand.CreateAsyncTask(Observable.Return(true), async _ =>
            {
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

                response = ValidateModel(ClientFlags.ValidateOrder.Phone | ClientFlags.ValidateOrder.Franchise | ClientFlags.ValidateOrder.Client
                | ClientFlags.ValidateOrder.Address | ClientFlags.ValidateOrder.Order |  ClientFlags.ValidateOrder.OrderSaved);
                
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
                SendOrderTitleBtn = "Reenviar pedido a la tienda";

                var orderMode = ExtractOrderMode();

                await Task.Run(() => OrderService.SendOrderToStore(orderMode, ExtraNotes, _promiseTime));
                return new Unit();
            });

            MessageBus.Current.Listen<PropagateOrderModel>(SharedMessageConstants.PROPAGATE_LASTORDER_POSCHECK).Subscribe(OnPropagate);
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
            if (ImmediateDelivery){
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

        private int ExtractOrderMode()
        {
            if (ImmediateDelivery)
                return SettingsData.Constants.StoreConst.MODE_DELIVERY_IMMEDIATE;
            
            if(FutureDelivery)
                return SettingsData.Constants.StoreConst.MODE_DELIVERY_FUTURE;
            
            return SettingsData.Constants.StoreConst.MODE_DELIVERY_IMMEDIATE;
        }

        private void ResetValues()
        {
            SendOrderTitleBtn = "Enviar pedido a la tienda";

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
        }

        public override bool Initialize(bool bForceToInit = false)
        {
            ResetValues();
            return base.Initialize(bForceToInit);
        }

        private void OnSendOrderToStoreEventChanged(ResponseMessage resMsg)
        {
            if (resMsg.IsSuccess)
            {
                if (resMsg.Code == SettingsData.Constants.StoreConst.STORE_RESPONSE_ORDER_OK)
                {
                    MessageBus.Current.SendMessage(new MessageBoxSettings
                    {
                        Message = resMsg.Message,
                        Title = "Estado de la orden",
                        Callback = (_ => EndOrder())
                    }, SharedMessageConstants.MSG_SHOW_SUCCESS);
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

        public Func<ClientFlags.ValidateOrder, ResponseMessage> ValidateModel { get; set; }
        public IReactiveCommand<Unit> SendOrderToStore { get; set; }
        public IMainOrderService OrderService { get; set; }

        public string SendOrderTitleBtn
        {
            get { return _sendOrderTitleBtn; }
            set {  this.RaiseAndSetIfChanged(ref _sendOrderTitleBtn, value); }
        }

        public Visibility IsSending
        {
            get { return _isSending; }
            set { this.RaiseAndSetIfChanged(ref _isSending,  value); }
        }

        public Visibility IsReadyToSend
        {
            get { return _isReadyToSend; }
            set { this.RaiseAndSetIfChanged(ref _isReadyToSend,  value); }
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

        public event Action EndOrder;

        protected virtual void OnEndOrder()
        {
            var handler = EndOrder;
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

        public void OnPosOrderChanged(PosCheck posCheck)
        {
            PosCheck = posCheck;
        }

        public PosCheck PosCheck
        {
            get { return _posCheck; }
            set
            {
                this.RaiseAndSetIfChanged(ref _posCheck, value);
            }
        }
    }
}
