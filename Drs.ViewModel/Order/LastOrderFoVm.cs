using System;
using System.ComponentModel;
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
        }

        public void ProcessPhone(ListItemModel model)
        {
            _client.ExecutionProxy.ExecuteRequest<String, String, ResponseMessageData<PosCheck>, ResponseMessageData<PosCheck>>
                    (model.Value, TransferDto.SameType, SharedConstants.Server.ORDER_HUB,
                        SharedConstants.Server.LAST_ORDER_ORDER_HUB_METHOD, TransferDto.SameType)
                    .Subscribe(OnPosCheckReady, OnPosCheckError);
        }


        private void OnPosCheckError(Exception ex)
        {

        }

        private void OnPosCheckReady(IStale<ResponseMessageData<PosCheck>> obj)
        {

            if (obj.IsStale)
                return;

            if (obj.Data.IsSuccess == false || obj.Data.Data == null || obj.Data.Data.LstItems.Count == 0)
                return;

            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                
            });
        }

    }
}
