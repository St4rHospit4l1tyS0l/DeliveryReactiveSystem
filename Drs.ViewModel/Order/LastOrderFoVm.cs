using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.ViewModel.Shared;
using ReactiveUI;


namespace Drs.ViewModel.Order
{
    public class LastOrderFoVm : FlyoutBaseVm, ILastOrderFoVm, IDataErrorInfo
    {
        private readonly IReactiveDeliveryClient _client;
        private PosCheck _posCheck;
        private string _titleLastOrder;
        private string _franchiseName;

        public string this[string columnName]
        {
            get
            {
                return null;
            }
        }

        public string Error { get; private set; }


        public LastOrderFoVm(IReactiveDeliveryClient client)
        {
            _client = client;
            LstItems = new ReactiveList<QtItemModel>();
        }

        public void ProcessPhone(ListItemModel model)
        {
            _client.ExecutionProxy.ExecuteRequest<String, String, ResponseMessageData<PosCheck>, ResponseMessageData<PosCheck>>
                    (model.Value, TransferDto.SameType, SharedConstants.Server.ORDER_HUB,
                        SharedConstants.Server.LAST_ORDER_ORDER_HUB_METHOD, TransferDto.SameType)
                    .Subscribe(e => OnPosCheckReady(e, model.Value), OnPosCheckError);
        }

        public PosCheck PosCheck
        {
            get { return _posCheck; }
            set { _posCheck = this.RaiseAndSetIfChanged(ref _posCheck, value); }
        }


        private void OnPosCheckError(Exception ex)
        {
            IsOpen = false;
        }

        private void OnPosCheckReady(IStale<ResponseMessageData<PosCheck>> obj, string phone)
        {

            if (obj.IsStale)
                return;

            if (obj.Data.IsSuccess == false || obj.Data.Data == null || obj.Data.Data.LstItems.Count == 0)
                return;

            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                PosCheck = obj.Data.Data;
                LstItems.Clear();
                LstItems.AddRange(obj.Data.Data.LstItems.GroupBy(e => new { e.ItemId, e.Name }).Select(e => new QtItemModel
                {
                    ItemId = e.Key.ItemId,
                    Name = e.Key.Name,
                    Quantity = e.Count()
                }).ToList());

                TitleLastOrder = String.Format("Último pedido de {0}", phone);
                FranchiseName = String.Format("Franquicia: {0}", PosCheck.Franchise.Name);
                IsOpen = true;
            });
        }

        public string FranchiseName
        {
            get { return _franchiseName; }
            set { this.RaiseAndSetIfChanged(ref _franchiseName, value); }
        }

        public IReactiveList<QtItemModel> LstItems { get; set; }

        public string TitleLastOrder
        {
            get { return _titleLastOrder; }
            set { this.RaiseAndSetIfChanged(ref _titleLastOrder, value); }
        }
    }
}
