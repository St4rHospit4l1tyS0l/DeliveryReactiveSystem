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
        private IDisposable _subscription;

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

        public void OnChangeStore(ItemCatalog item)
        {
            if(item == null)
                return;

            var orderModel = OrderService.OrderModel;

            if (orderModel.StoreModel != null && orderModel.StoreModel.IdKey == item.Id)
                return;

            LookingAvailability();
            DisposeSubscription();

            _subscription = _client.ExecutionProxy.ExecuteRequest<ItemCatalog, ItemCatalog, ResponseMessageData<StoreModel>,
                ResponseMessageData<StoreModel>>(item, TransferDto.TransferDto.SameType, SharedConstants.Server.STORE_HUB,
                    SharedConstants.Server.AVAILABLE_BY_STORE_STORE_HUB_METHOD, TransferDto.TransferDto.SameType).Subscribe(obj => OnResultStoreAvailableOk(obj), OnResultStoreAvailableError);            
        }

        private void DisposeSubscription()
        {
            if (_subscription != null)
            {
                try
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + @" - ST - " + ex.StackTrace);
                }
            }
        }

        private void LookingAvailability()
        {
            OrderService.OrderModel.StoreModel = null;
            OnStoreSelected(null, "Buscando disponiblidad...");
        }

        public void OnUndoPickUpInStore()
        {
            var orderModel = OrderService.OrderModel;

            if (orderModel == null || orderModel.LastStoreModelByClientAddress == null || orderModel.LastStoreModelByClientAddress.IdKey.HasValue == false ||
                (orderModel.StoreModel != null && orderModel.LastStoreModelByClientAddress.IdKey == orderModel.StoreModel.IdKey))
                return;

            LookingAvailability();
            DisposeSubscription();

            var item = new ItemCatalog
            {
                Id = orderModel.LastStoreModelByClientAddress.IdKey.Value
            };

            _client.ExecutionProxy.ExecuteRequest<ItemCatalog, ItemCatalog, ResponseMessageData<StoreModel>,
                ResponseMessageData<StoreModel>>(item, TransferDto.TransferDto.SameType, SharedConstants.Server.STORE_HUB,
                    SharedConstants.Server.AVAILABLE_BY_STORE_STORE_HUB_METHOD, TransferDto.TransferDto.SameType)
                .Subscribe(obj => OnResultStoreAvailableOk(obj), OnResultStoreAvailableError);            

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

            DisposeSubscription();

            var model = new StoreAvailableModel
            {
                FranchiseCode = OrderService.OrderModel.Franchise.Code,
                AddressInfo = address.AddressInfo
            };

            _client.ExecutionProxy.ExecuteRequest<StoreAvailableModel, StoreAvailableModel, ResponseMessageData<StoreModel>,
                ResponseMessageData<StoreModel>>(model, TransferDto.TransferDto.SameType, SharedConstants.Server.STORE_HUB,
                    SharedConstants.Server.AVAILABLE_FOR_ADDRESS_STORE_HUB_METHOD, TransferDto.TransferDto.SameType)
                .Subscribe(e => OnResultStoreAvailableOk(e, true), OnResultStoreAvailableError);
        }

        private void OnResultStoreAvailableError(Exception ex)
        {
            OnResultStoreAvailableError(ex.Message);
        }

        private void OnResultStoreAvailableError(string sMsg)
        {
            OrderService.OrderModel.StoreModel = null;
            OnStoreSelected(null, sMsg);
        }

        private void OnResultStoreAvailableOk(IStale<ResponseMessageData<StoreModel>> obj, bool bIsForAddress = false)
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
            if (bIsForAddress)
                OrderService.OrderModel.LastStoreModelByClientAddress = dataResp;
            OrderService.OrderModel.StoreModel.ExtraMsg = obj.Data.Message;

            OnStoreSelected(dataResp, null);
        }
    }
}
