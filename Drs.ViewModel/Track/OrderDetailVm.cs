using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Model.Track;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Track
{
    public class OrderDetailVm : UcViewModelBase, IOrderDetailVm
    {
        private readonly IReactiveDeliveryClient _client;
        private TrackOrderDetailDto _orderDetail;
        private Visibility _visiblityStoreErrMsg;

        public IReactiveCommand<Unit> CopyIdOrder { get; set; }

        public OrderDetailVm(IReactiveDeliveryClient client)
        {
            _client = client;
            VisiblityStoreErrMsg = Visibility.Collapsed;
            CopyIdOrder = ReactiveCommand.CreateAsyncTask(Observable.Return(true), _ => OnCopyIdOrder());
        }

        private async Task<Unit> OnCopyIdOrder()
        {
            await Task.Run(() =>
            {
                if (OrderDetail == null) return;
                RxApp.MainThreadScheduler.Schedule(_ => Clipboard.SetText(OrderDetail.OrderAtoId));
            });

            return new Unit();
        }

        public void OnShowDetail(long orderToStoreId)
        {
            OnStatusChanged(SettingsData.Constants.TrackConst.SEARCH_SHOWDETAIL_ON_PROGRESS, "Obteniendo el detalle del pedido...");

            _client.ExecutionProxy.ExecuteRequest<long, long, ResponseMessageData<TrackOrderDetailDto>, ResponseMessageData<TrackOrderDetailDto>>
                (orderToStoreId, TransferDto.SameType, SharedConstants.Server.TRACK_HUB,
                    SharedConstants.Server.SHOW_DETAIL_TRACK_HUB_METHOD, TransferDto.SameType)
                    .Subscribe(OnResultShowDetailOk, OnResultShowDetailError);    
        }

        private void OnResultShowDetailError(Exception obj)
        {
            OnResultShowDetailError(obj.Message);
        }

        private void OnResultShowDetailError(string message)
        {
            OnStatusChanged(SettingsData.Constants.TrackConst.SEARCH_SHOWDETAIL_ERROR, message);
        }

        private void OnResultShowDetailOk(IStale<ResponseMessageData<TrackOrderDetailDto>> obj)
        {
            if (obj.IsStale)
            {
                OnResultShowDetailError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnResultShowDetailError(obj.Data.Message);
                return;
            }

            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                obj.Data.Data.LstOrderLog = obj.Data.Data.LstOrderLog.OrderByDescending(e => e.Id).ToList();
                OrderDetail = obj.Data.Data;
                VisiblityStoreErrMsg = String.IsNullOrWhiteSpace(OrderDetail.StoreErrMsg) ? Visibility.Collapsed : Visibility.Visible;
            });

            OnStatusChanged(SettingsData.Constants.TrackConst.SEARCH_SHOWDETAIL_OK, String.Empty);
        }

        public event Action<int, string> StatusChanged;

        public TrackOrderDetailDto OrderDetail
        {
            get { return _orderDetail; }
            set
            {
                this.RaiseAndSetIfChanged(ref _orderDetail, value);
            }
        }

        public Visibility VisiblityStoreErrMsg
        {
            get { return _visiblityStoreErrMsg; }
            set
            {
                this.RaiseAndSetIfChanged(ref _visiblityStoreErrMsg, value);
            }
        }

        protected virtual void OnStatusChanged(int iStatus, string sMsg)
        {
            var handler = StatusChanged;
            if (handler != null) handler(iStatus, sMsg);
        }
    }
}
