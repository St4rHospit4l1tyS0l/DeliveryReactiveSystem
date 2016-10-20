using System;
using System.Collections.Generic;
using System.Linq;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Repository.Log;
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
        public event Action<StoreModel, string, bool> StoreSelected;
        public event Action<List<ItemCatalog>> StoresReceivedByAddress;

        protected virtual void OnStoresReceivedByAddress(List<ItemCatalog> obj)
        {            
            var handler = StoresReceivedByAddress;
            if (handler != null) handler(obj);
        }


        protected virtual void OnStoreSelected(StoreModel obj, string sMsg, bool pendingStatus)
        {
            var handler = StoreSelected;
            if (handler != null) handler(obj, sMsg, pendingStatus);
        }

        public void OnAddressSelected(AddressInfoGrid obj)
        {
            ValidateStore();
        }

        public void OnFranchiseChanged(FranchiseInfoModel obj)
        {
            ValidateStore();
        }

        public void OnChangeStore(ItemCatalog item, bool bIsLastStore)
        {
            if(item == null)
                return;

            //var orderModel = OrderService.OrderModel;
            //if (orderModel.StoreModel != null && orderModel.StoreModel.IdKey == item.Id)
            //    return;

            LookingAvailability(item);
            DisposeSubscription();

            _subscription = _client.ExecutionProxy.ExecuteRequest<ItemCatalog, ItemCatalog, ResponseMessageData<StoreModel>,
                ResponseMessageData<StoreModel>>(item, TransferDto.TransferDto.SameType, SharedConstants.Server.STORE_HUB,
                    SharedConstants.Server.AVAILABLE_BY_STORE_STORE_HUB_METHOD, TransferDto.TransferDto.SameType)
                    .Subscribe(obj => OnResultStoreAvailableOk(obj, false, bIsLastStore), OnResultStoreAvailableError);            
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
                    SharedLogger.LogError(ex);
                }
            }
        }

        private void LookingAvailability(ItemCatalog item)
        {
            var store = OrderService.OrderModel.StoreModel;
            OrderService.OrderModel.StoreModel = null;
            if (store == null || store.IdKey != item.Id)
                store = null;
            OnStoreSelected(store, "Buscando disponiblidad...", true);
        }

        public void OnUndoPickUpInStore()
        {
            var orderModel = OrderService.OrderModel;

            if (orderModel == null || orderModel.LastStoreModelByClientAddress == null || orderModel.LastStoreModelByClientAddress.IdKey.HasValue == false ||
                (orderModel.StoreModel != null && orderModel.LastStoreModelByClientAddress.IdKey == orderModel.StoreModel.IdKey))
                return;

            var item = new ItemCatalog
            {
                Id = orderModel.LastStoreModelByClientAddress.IdKey.Value,
                Name = String.Format("{0} ({1})", orderModel.LastStoreModelByClientAddress.Value, orderModel.LastStoreModelByClientAddress.MainAddress),
                Value = orderModel.LastStoreModelByClientAddress.MainAddress
            };

            LookingAvailability(item);
            DisposeSubscription();
                

            _subscription = _client.ExecutionProxy.ExecuteRequest<ItemCatalog, ItemCatalog, ResponseMessageData<StoreModel>,
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

            _subscription = _client.ExecutionProxy.ExecuteRequest<StoreAvailableModel, StoreAvailableModel, ResponseMessageData<StoreModel>,
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
            OnStoreSelected(null, sMsg, false);
        }

        private void OnResultStoreAvailableOk(IStale<ResponseMessageData<StoreModel>> obj, bool bIsForAddress = false, bool bIsLastStore = false)
        {
            if (obj.IsStale)
            {
                OnResultStoreAvailableError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnResultStoreAvailableError(obj.Data.Message);
                if (bIsLastStore == false)
                    ExtractStores(obj.Data.LstData);
                return;
            }

            var dataResp = obj.Data.Data;

            if (dataResp == null)
            {
                OnResultStoreAvailableError("No hay una sucursal disponible en la dirección que seleccionó");
                if (bIsLastStore == false)
                    ExtractStores(obj.Data.LstData);
                return;
            }

            OrderService.OrderModel.StoreModel = dataResp;
            OrderService.OrderModel.StoreModel.ExtraMsg = obj.Data.Message;

            if (bIsForAddress || bIsLastStore)
            {
                OrderService.OrderModel.LastStoreModelByClientAddress = dataResp;

                if (bIsLastStore == false)
                {
                    ExtractStores(obj.Data.LstData);
                    return;
                }
            }

            OnStoreSelected(dataResp, null, false);
        }

        private void ExtractStores(IEnumerable<StoreModel> lstData)
        {
            if (lstData == null)
            {
                OnStoresReceivedByAddress(null);
                return;
            }

            var storeModels = lstData.ToList();
            if (storeModels.Any() == false)
            {
                OnStoresReceivedByAddress(null);
                return;
            }

            OnStoresReceivedByAddress(storeModels.Select(e => e.IdKey != null ? new ItemCatalog
            {
                Id = e.IdKey.Value,
                Name = String.Format("{0} ({1})", e.Value, e.MainAddress),
                Value = e.MainAddress
            } : null).ToList());
        }
    }
}
