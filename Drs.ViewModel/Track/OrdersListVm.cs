using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Model.Track;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.ViewModel.Shared;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;

namespace Drs.ViewModel.Track
{
    public class OrdersListVm : UcViewModelBase, IOrdersListVm
    {
        private readonly IReactiveDeliveryClient _client;
        private string _totalFound;

        public OrdersListVm(IReactiveDeliveryClient client)
        {
            _client = client;
            LstItems = new ReactiveList<TrackOrderDto>();
            CmdShowDetail = ReactiveCommand.CreateAsyncTask(Observable.Return(true), DoShowDetail);
            CmdCancelOrder = ReactiveCommand.CreateAsyncTask(Observable.Return(true), DoCancelOrder);
        }

        private async Task<Unit> DoCancelOrder(object o)
        {
            var lstItemSelected = o as ObservableCollection<object>;

            if (lstItemSelected != null && lstItemSelected.Count > 0)
            {
                var model = lstItemSelected[0] as TrackOrderDto;
                if (model != null)
                    await Task.Run(() => OnCancelOrder(model));
            }

            return new Unit();
        }

        private async Task<Unit> DoShowDetail(object o)
        {
            var lstItemSelected = o as ObservableCollection<object>;

            if (lstItemSelected != null && lstItemSelected.Count > 0)
            {
                var model = lstItemSelected[0] as TrackOrderDto;
                if (model != null)
                    await Task.Run(() => OnShowDetail(model.OrderToStoreId));
            }

            return new Unit();
        }

        public event Action<long> ShowDetail;

        protected virtual void OnShowDetail(long orderToStoreId)
        {
            var handler = ShowDetail;
            if (handler != null) handler(orderToStoreId);
        }


        protected virtual void OnCancelOrder(TrackOrderDto order)
        {
            MessageBus.Current.SendMessage(new MessageBoxSettings
            {
                Message = "¿Desea cancelar la orden seleccionada?",
                Title = "Cancelar la orden",
                Settings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Si",
                    NegativeButtonText = "No"
                },
                Style = MessageDialogStyle.AffirmativeAndNegative,
                Callback = (x => ExecuteCancelOrder(x, order))

            }, SharedMessageConstants.MSG_SHOW_ERRQST);
        }

        private void ExecuteCancelOrder(MessageDialogResult result, TrackOrderDto order)
        {
            if (result != MessageDialogResult.Affirmative)
                return;

            _client.ExecutionProxy.ExecuteRequest<long, long, ResponseMessageData<ResponseMessage>, ResponseMessageData<ResponseMessage>>
                    (order.OrderToStoreId, TransferDto.SameType, SharedConstants.Server.STORE_HUB,
                        SharedConstants.Server.CANCEL_ORDER_STORE_HUB_METHOD, TransferDto.SameType)
                        .Subscribe(e => OnResultCancelOrderOk(e, order), OnResultCancelOrderError);
        }

        private void OnResultCancelOrderError(Exception obj)
        {
            OnResultCancelOrderError(obj.Message);
        }

        private void OnResultCancelOrderError(string message)
        {
            MessageBus.Current.SendMessage(new MessageBoxSettings
            {
                Message = message,
                Title = "Cancelar la orden",
            }, SharedMessageConstants.MSG_SHOW_ERRQST);
        }

        private void OnResultCancelOrderOk(IStale<ResponseMessageData<ResponseMessage>> obj, TrackOrderDto order)
        {
            if (obj.IsStale)
            {
                OnResultCancelOrderError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnResultCancelOrderError(obj.Data.Message);
                return;
            }

            var response = obj.Data.Data;

            if (response.IsSuccess == false)
            {
                OnResultCancelOrderError(response.Message);
                return;
            }

            order.LastStatus = SettingsData.Constants.TrackConst.CANCELED;
            order.ChangeCancel = true;
            MessageBus.Current.SendMessage(new MessageBoxSettings
            {
                Message = response.Message,
                Title = "Cancelar la orden"
                //, Callback = (_ => EndOrder())
            }, SharedMessageConstants.MSG_SHOW_SUCCESS);
        }

        public void OnPhoneChanged(String phone)
        {
            OnStatusChanged(SettingsData.Constants.TrackConst.SEARCH_ORDERLIST_ON_PROGRESS, "Buscando pedidos...");

            _client.ExecutionProxy.ExecuteRequest<String, String, ResponseMessageData<TrackOrderDto>, ResponseMessageData<TrackOrderDto>>
                            (phone, TransferDto.SameType, SharedConstants.Server.TRACK_HUB,
                                SharedConstants.Server.SEARCH_BY_PHONE_TRACK_HUB_METHOD, TransferDto.SameType)
                                .Subscribe(OnResultSearchOk, OnResultSearchError);

        }


        public void OnClientNameChanged(string clientName)
        {
            OnStatusChanged(SettingsData.Constants.TrackConst.SEARCH_ORDERLIST_ON_PROGRESS, "Buscando pedidos...");

            _client.ExecutionProxy.ExecuteRequest<String, String, ResponseMessageData<TrackOrderDto>, ResponseMessageData<TrackOrderDto>>
                            (clientName, TransferDto.SameType, SharedConstants.Server.TRACK_HUB,
                                SharedConstants.Server.SEARCH_BY_CLIENTNAME_TRACK_HUB_METHOD, TransferDto.SameType)
                                .Subscribe(OnResultSearchOk, OnResultSearchError);

        }

        private void OnResultSearchError(Exception ex)
        {
            OnResultSearchError(ex.Message);
        }

        private void OnResultSearchError(string sMsg)
        {
            OnStatusChanged(SettingsData.Constants.TrackConst.SEARCH_ERROR_PHONE, sMsg);
        }

        private void OnResultSearchOk(IStale<ResponseMessageData<TrackOrderDto>> obj)
        {
            if (obj.IsStale)
            {
                OnResultSearchError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnResultSearchError(obj.Data.Message);
                return;
            }

            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                LstItems.Clear();
                TotalFound = String.Format("Se ha(n) encontrado {0} registro(s)", obj.Data.LstData.Count());

                foreach (var dto in obj.Data.LstData)
                {
                    LstItems.Add(dto);
                }
            });

            OnStatusChanged(SettingsData.Constants.TrackConst.SEARCH_ORDERLIST_OK, String.Empty);
        }

        public IReactiveList<TrackOrderDto> LstItems { get; set; }

        public event Action<int, string> StatusChanged;

        public IReactiveCommand<Unit> CmdShowDetail { get; set; }
        public IReactiveCommand<Unit> CmdCancelOrder { get; set; }

        public string TotalFound
        {
            get { return _totalFound; }
            set
            {
                this.RaiseAndSetIfChanged(ref _totalFound, value);
            }
        }

        protected virtual void OnStatusChanged(int iStatus, string sMsg)
        {
            var handler = StatusChanged;
            if (handler != null) handler(iStatus, sMsg);
        }
    }
}
