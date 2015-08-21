using System;
using System.Linq;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Service.ReactiveDelivery;
using Drs.Model.Address;
using Drs.Model.Store;

namespace Drs.Service.Client
{
    public class StoreAddressService : IStoreAddressService
    {
        private readonly IReactiveDeliveryClient _client;

        public StoreAddressService(IReactiveDeliveryClient client)
        {
            _client = client;
        }

        public IMainOrderService OrderService { get; set; }
        public event Action<StoreModel, string> StoreSelected;
        
        protected virtual void OnStoreSelected(StoreModel obj, string sMsg)
        {
            var handler = StoreSelected;
            if (handler != null) handler(obj, sMsg);
        }

        public void OnAddressSelected(AddressInfoGrid obj)
        {
            ValidateStore();
        }

        public void OnFranchiseChanged(FranchiseInfoModel obj)
        {
            ValidateStore();
        }

        private void ValidateStore()
        {
            var orderModel = OrderService.OrderModel;

            if (orderModel == null)
                return;

            var franchise = orderModel.Franchise;

            if (franchise == null || String.IsNullOrEmpty(franchise.Code))
                return;

            var address = orderModel.LstAddressInfo.FirstOrDefault(e => e.IsSelected);

            if (address == null)
                return;

            var model = new StoreAvailableModel
            {
                FranchiseCode = OrderService.OrderModel.Franchise.Code,
                AddressInfo = address.AddressInfo
            };

            _client.ExecutionProxy.ExecuteRequest<StoreAvailableModel, StoreAvailableModel, ResponseMessageData<StoreModel>,
                ResponseMessageData<StoreModel>>(model, TransferDto.TransferDto.SameType, SharedConstants.Server.STORE_HUB,
                    SharedConstants.Server.AVAILABLE_FOR_ADDRESS_STORE_HUB_METHOD, TransferDto.TransferDto.SameType)
                .Subscribe(OnResultStoreAvailableOk, OnResultStoreAvailableError);
        }

        private void OnResultStoreAvailableError(Exception ex)
        {
            OnResultStoreAvailableError(ex.Message);
        }

        private void OnResultStoreAvailableError(string sMsg)
        {
            OnStoreSelected(null, sMsg);
        }

        private void OnResultStoreAvailableOk(IStale<ResponseMessageData<StoreModel>> obj)
        {
            if (obj.IsStale)
            {
                OnResultStoreAvailableError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnResultStoreAvailableError(obj.Data.Message);
                return;
            }

            var dataResp = obj.Data.Data;

            if (dataResp == null)
            {
                OnResultStoreAvailableError("No hay una tienda disponible en la dirección que seleccionó");
                return;
            }

            OrderService.OrderModel.StoreModel = dataResp;

            OnStoreSelected(dataResp, null);
        }
    }
}
