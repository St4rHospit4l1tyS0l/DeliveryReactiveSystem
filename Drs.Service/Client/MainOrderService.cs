using System;
using System.Linq;
using System.Reactive.Concurrency;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Account;
using Drs.Model.Address;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Service.ReactiveDelivery;
using ReactiveUI;

namespace Drs.Service.Client
{
    public class MainOrderService : IMainOrderService
    {
        private OrderModel _model;
        private readonly IReactiveDeliveryClient _client;
        private readonly Func<ClientFlags.ValidateOrder, ResponseMessage> _validateModel;

        public MainOrderService(IReactiveDeliveryClient client)
        {
            _client = client;
            _validateModel = OnValidateModel;
        }

        public void ResetAndCreateNewOrder()
        {
            _model = new OrderModel();
        }

        private ResponseMessage OnValidateModel(ClientFlags.ValidateOrder validate)
        {
            try
            {
                if ((validate & ClientFlags.ValidateOrder.Phone) == ClientFlags.ValidateOrder.Phone)
                {
                    if (_model.PhoneInfo.Status != SharedConstants.Client.RECORD_SAVED)
                    {
                        return new ResponseMessage { IsSuccess = false, Message = "Aún no se ha definido o almacenado el teléfono", View = SharedConstants.Client.ORDER_TAB_PHONE };
                    }
                }

                if ((validate & ClientFlags.ValidateOrder.Franchise) == ClientFlags.ValidateOrder.Franchise)
                {
                    if (_model.Franchise.Status != SharedConstants.Client.RECORD_SAVED)
                    {
                        return new ResponseMessage { IsSuccess = false, Message = "Aún no se ha definido o almacenado la franquicia", View = SharedConstants.Client.ORDER_TAB_FRANCHISE };
                    }
                }


                if ((validate & ClientFlags.ValidateOrder.Client) == ClientFlags.ValidateOrder.Client)
                {
                    var item = _model.LstClientInfo.FirstOrDefault(e => e.IsSelected);
                    if (item == null || item.Status != SharedConstants.Client.RECORD_SAVED)
                    {
                        return new ResponseMessage { IsSuccess = false, Message = "Aún no se ha definido o almacenado al cliente", View = SharedConstants.Client.ORDER_TAB_CLIENTS };
                    }
                }


                if ((validate & ClientFlags.ValidateOrder.Address) == ClientFlags.ValidateOrder.Address)
                {
                    var item = _model.LstAddressInfo.FirstOrDefault(e => e.IsSelected);
                    if (item == null || item.Status != SharedConstants.Client.RECORD_SAVED)
                    {
                        return new ResponseMessage { IsSuccess = false, Message = "Aún no se ha definido o almacenado la dirección", View = SharedConstants.Client.ORDER_TAB_CLIENTS };
                    }
                }

                if ((validate & ClientFlags.ValidateOrder.StoreAvailable) == ClientFlags.ValidateOrder.StoreAvailable)
                {
                    var item = _model.StoreModel;
                    if (item == null || String.IsNullOrEmpty(item.Key))
                    {
                        return new ResponseMessage { IsSuccess = false, Message = "No hay tienda disponible para esta dirección", View = SharedConstants.Client.ORDER_TAB_CLIENTS };
                    }
                }

                if ((validate & ClientFlags.ValidateOrder.Order) == ClientFlags.ValidateOrder.Order)
                {
                    var item = _model.PosCheck;
                    if (item == null || item.LstItems == null || item.LstItems.Count == 0)
                    {
                        return new ResponseMessage { IsSuccess = false, Message = "Aún no se ha obtenido el pedido del POS", View = SharedConstants.Client.ORDER_TAB_ORDER };
                    }
                }


                if ((validate & ClientFlags.ValidateOrder.OrderSaved) == ClientFlags.ValidateOrder.OrderSaved)
                {
                    var item = _model.PosCheck;
                    if (item == null || item.LstItems == null || item.LstItems.Count == 0 || item.Status != SharedConstants.Client.RECORD_SAVED)
                    {
                        return new ResponseMessage { IsSuccess = false, Message = "Aún no se ha almacenado el pedido del POS", View = SharedConstants.Client.ORDER_TAB_DELIVERY };
                    }
                }

                return new ResponseMessage { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ResponseMessage { IsSuccess = false, Message = ex.Message };
            }
        }

        public void ProcessPhone(ListItemModel itemModel)
        {
            _model.PhoneInfo.Phone = itemModel.Value;

            int phoneId;
            if (!String.IsNullOrWhiteSpace(itemModel.Key) && int.TryParse(itemModel.Key, out phoneId))
            {
                _model.PhoneInfo.PhoneId = phoneId;
                _model.PhoneInfo.Status = SharedConstants.Client.RECORD_SAVED;
                OnPhoneChanged(_model.PhoneInfo);
            }
            else
            {
                SavePhoneInformation();
            }

        }

        public void SavePhoneInformation()
        {
            _model.PhoneInfo.Username = CurrentUserSettings.UserInfo.Username;
            _model.PhoneInfo.Status = SharedConstants.Client.RECORD_ONPROGRESS_TO_SAVED;
            OnPhoneChanged(_model.PhoneInfo);

            _client.ExecutionProxy.ExecuteRequest<PhoneModel, PhoneModel, ResponseMessageData<PhoneModel>, ResponseMessageData<PhoneModel>>
                (_model.PhoneInfo, TransferDto.TransferDto.SameType, SharedConstants.Server.ORDER_HUB, SharedConstants.Server.SAVE_PHONE_ORDER_HUB_METHOD, TransferDto.TransferDto.SameType)
                .Subscribe(OnPhoneSaved, OnPhoneSavedError);
        }

        private void OnPhoneSavedError(Exception ex)
        {
            OnPhoneSavedError(ex.Message);
        }

        private void OnPhoneSavedError(string msgError)
        {
            _model.PhoneInfo.Message = msgError;
            _model.PhoneInfo.Status = SharedConstants.Client.RECORD_ERROR_SAVED;
            OnPhoneChanged(_model.PhoneInfo);
        }

        private void OnPhoneSaved(IStale<ResponseMessageData<PhoneModel>> obj)
        {
            if (obj.IsStale)
            {
                OnPhoneSavedError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnPhoneSavedError(obj.Data.Message);
                return;
            }

            var modelSaved = obj.Data.Data;
            _model.PhoneInfo.PhoneId = modelSaved.PhoneId;
            _model.PhoneInfo.Phone = modelSaved.Phone;
            _model.PhoneInfo.Status = SharedConstants.Client.RECORD_SAVED;
            OnPhoneChanged(_model.PhoneInfo);
        }

        public event Action<PhoneModel> PhoneChanged;
        public event Action<FranchiseInfoModel> FranchiseChanged;
        public event Action<ClientInfoGrid> ClientChanged;
        public event Action<AddressInfoGrid> AddressChanged;
        public event Action<PosCheck> PosOrderChanged;
        public event Action<OrderModelDto> SendOrderToStoreStatusChanged;


        protected virtual void OnPosCheckChanged(PosCheck posCheck)
        {
            var handler = PosOrderChanged;
            if (handler != null) handler(posCheck);
        }


        protected virtual void OnPhoneChanged(PhoneModel model)
        {
            var handler = PhoneChanged;
            if (handler != null) handler(model);
        }

        public void ProcessFranchise(FranchiseInfoModel franchiseInfo)
        {
            _model.Franchise.Code = franchiseInfo.Code;
            _model.Franchise.Title = franchiseInfo.Title;
            _model.Franchise.Status = SharedConstants.Client.RECORD_SAVED;
            OnFranchiseChanged(franchiseInfo);
        }

        public Func<ClientFlags.ValidateOrder, ResponseMessage> ValidateModel
        {
            get { return _validateModel; }
        }

        protected virtual void OnFranchiseChanged(FranchiseInfoModel model)
        {
            var handler = FranchiseChanged;
            if (handler != null) handler(model);
        }

        public void ProcessAddress(AddressInfoGrid infoModel)
        {
            var oldAddressInfo = _model.LstAddressInfo.FirstOrDefault(e =>
                (infoModel.AddressInfo.AddressId != null && e.AddressInfo.AddressId == infoModel.AddressInfo.AddressId) || infoModel.PreId == e.PreId);

            if (oldAddressInfo != null)
            {
                RxApp.MainThreadScheduler.Schedule(_ => _model.LstAddressInfo.Remove(oldAddressInfo));
            }

            RxApp.MainThreadScheduler.Schedule(_ => _model.LstAddressInfo.Add(infoModel));

            infoModel.Status = SharedConstants.Client.RECORD_ONPROGRESS_TO_SAVED;
            infoModel.Username = CurrentUserSettings.UserInfo.Username;
            infoModel.AddressInfo.PrimaryPhone = _model.PhoneInfo;

            OnAddressChanged(infoModel);

            _client.ExecutionProxy.ExecuteRequest<AddressInfoModel, AddressInfoModel, ResponseMessageData<AddressInfoModel>, ResponseMessageData<AddressInfoModel>>
                (infoModel.AddressInfo, TransferDto.TransferDto.SameType, SharedConstants.Server.ADDRESS_HUB,
                    SharedConstants.Server.SAVE_ADDRESS_ADDRESS_HUB_METHOD, TransferDto.TransferDto.SameType)
                .Subscribe(x => OnAddressSaved(x, infoModel), e => OnAddressSavedError(e, infoModel));

        }


        //public IReactiveList<ClientInfoGrid> LstClientInfo
        //{
        //    get { return _model.LstClientInfo; }
        //}

        //public IReactiveList<AddressInfoGrid> LstAddressInfo
        //{
        //    get { return _model.LstAddressInfo; }
        //}

        public OrderModel OrderModel
        {
            get { return _model; }
        }

        public void ProcessPosOrder(PosCheck posCheck)
        {
            _model.PosCheck = posCheck;
            SavePosOrder();
        }

        public void SavePosOrder()
        {
            _model.PosCheck.Username = CurrentUserSettings.UserInfo.Username;
            _model.PosCheck.FranchiseCode = _model.Franchise.Code;
            _model.PosCheck.Status = SharedConstants.Client.RECORD_ONPROGRESS_TO_SAVED;

            OnPosCheckChanged(_model.PosCheck);

            _client.ExecutionProxy
                .ExecuteRequest<PosCheck, PosCheck, ResponseMessageData<PosCheck>, ResponseMessageData<PosCheck>>
                (_model.PosCheck, TransferDto.TransferDto.SameType, SharedConstants.Server.ORDER_HUB,
                    SharedConstants.Server.SAVE_POS_CHECK_ORDER_HUB_METHOD, TransferDto.TransferDto.SameType)
                .Subscribe(OnPosCheckSavedOk, OnPosCheckSavedError);
        }

        public void SendOrderToStore(OrderDetails orderDetails)
        {
            _model.Username = CurrentUserSettings.UserInfo.Username;
            _model.OrderDetails = orderDetails;

            var modelDto = _model.ToDto();
            _client.ExecutionProxy
                .ExecuteRequest<OrderModelDto, OrderModelDto, ResponseMessageData<OrderModelDto>, ResponseMessageData<OrderModelDto>>
                (modelDto, TransferDto.TransferDto.SameType, SharedConstants.Server.STORE_HUB,
                    SharedConstants.Server.SEND_ORDER_TO_STORE_HUB_METHOD, TransferDto.TransferDto.SameType)
                .Subscribe(x => OnSendOrderToStoreOk(x, modelDto), x => OnSendOrderToStoreError(x, modelDto));
        }


        private void OnSendOrderToStoreError(Exception ex, OrderModelDto modelDto)
        {
            OnSendOrderToStoreError(ex.Message, modelDto);
        }

        private void OnSendOrderToStoreError(String sMsgErr, OrderModelDto modelDto)
        {
            modelDto.HasError = true;
            modelDto.Message = sMsgErr;
            OnSendOrderToStoreStatusChanged(modelDto);
        }

        private void OnSendOrderToStoreOk(IStale<ResponseMessageData<OrderModelDto>> obj, OrderModelDto modelDto)
        {
            if (obj.IsStale)
            {
                OnSendOrderToStoreError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN, modelDto);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnSendOrderToStoreError(obj.Data.Message, modelDto);
                return;
            }

            modelDto = obj.Data.Data;
            modelDto.HasError = false;
            modelDto.Message = String.Format("Enviando a la tienda: {0}.\nDirección: {1}\nTeléfono(s): {2}",
                modelDto.Store.Value, modelDto.Store.MainAddress, string.Join(", ", modelDto.Store.LstPhones));
            OnSendOrderToStoreStatusChanged(modelDto);
        }

        protected virtual void OnSendOrderToStoreStatusChanged(OrderModelDto obj)
        {
            Action<OrderModelDto> handler = SendOrderToStoreStatusChanged;
            if (handler != null) handler(obj);
        }

        private void OnPosCheckSavedError(Exception ex)
        {
            OnPosCheckSavedError(ex.Message);
        }

        private void OnPosCheckSavedError(string msgError)
        {
            _model.PosCheck.Message = msgError;
            _model.PosCheck.Status = SharedConstants.Client.RECORD_ERROR_SAVED;
            OnPosCheckChanged(_model.PosCheck);
        }

        private void OnPosCheckSavedOk(IStale<ResponseMessageData<PosCheck>> obj)
        {
            if (obj.IsStale)
            {
                OnPosCheckSavedError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnPosCheckSavedError(obj.Data.Message);
                return;
            }

            var modelSaved = obj.Data.Data;
            _model.PosCheck.Id = modelSaved.Id;
            _model.PosCheck.CheckId = modelSaved.CheckId;
            _model.PosCheck.GuidId = modelSaved.GuidId;
            _model.PosCheck.Status = SharedConstants.Client.RECORD_SAVED;
            OnPosCheckChanged(_model.PosCheck);
        }

        public void ProcessClient(ClientInfoGrid clientInfoModel)
        {
            var oldClientInfo = _model.LstClientInfo.FirstOrDefault(e =>
                (clientInfoModel.ClientInfo.ClientId != null && e.ClientInfo.ClientId == clientInfoModel.ClientInfo.ClientId) || clientInfoModel.ClientPreId == e.ClientPreId);

            if (oldClientInfo != null)
            {
                RxApp.MainThreadScheduler.Schedule(_ => _model.LstClientInfo.Remove(oldClientInfo));
            }

            RxApp.MainThreadScheduler.Schedule(_ => _model.LstClientInfo.Add(clientInfoModel));

            clientInfoModel.Status = SharedConstants.Client.RECORD_ONPROGRESS_TO_SAVED;
            clientInfoModel.Username = CurrentUserSettings.UserInfo.Username;
            clientInfoModel.ClientInfo.PrimaryPhone = _model.PhoneInfo;

            if (clientInfoModel.ClientInfo.SecondPhone != null)
                clientInfoModel.ClientInfo.SecondPhone.Username = CurrentUserSettings.UserInfo.Username;

            OnClientChanged(clientInfoModel);

            _client.ExecutionProxy.ExecuteRequest<ClientInfoModel, ClientInfoModel, ResponseMessageData<ClientInfoModel>, ResponseMessageData<ClientInfoModel>>
                (clientInfoModel.ClientInfo, TransferDto.TransferDto.SameType, SharedConstants.Server.ORDER_HUB, SharedConstants.Server.SAVE_CLIENT_ORDER_HUB_METHOD, TransferDto.TransferDto.SameType)
                .Subscribe(x => OnClientSaved(x, clientInfoModel), e => OnClientSavedError(e, clientInfoModel));
        }


        private void OnClientSavedError(Exception exception, ClientInfoGrid clientInfoModel)
        {
            OnClientSavedError(exception.Message, clientInfoModel);
        }

        private void OnClientSavedError(String msgError, ClientInfoGrid clientInfoModel)
        {
            clientInfoModel.Message = msgError;
            clientInfoModel.Status = SharedConstants.Client.RECORD_ERROR_SAVED;
            OnClientChanged(clientInfoModel);
        }

        private void OnClientSaved(IStale<ResponseMessageData<ClientInfoModel>> obj, ClientInfoGrid clientInfoModel)
        {
            if (obj.IsStale)
            {
                OnClientSavedError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN, clientInfoModel);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnClientSavedError(obj.Data.Message, clientInfoModel);
                return;
            }

            var modelSaved = obj.Data.Data;
            clientInfoModel.ClientInfo.ClientId = modelSaved.ClientId;
            clientInfoModel.ClientInfo.SecondPhone = modelSaved.SecondPhone;
            clientInfoModel.ClientInfo.CompanyId = modelSaved.CompanyId;
            clientInfoModel.Status = SharedConstants.Client.RECORD_SAVED;
            OnClientChanged(clientInfoModel);
        }

        private void OnAddressSavedError(Exception exception, AddressInfoGrid infoModel)
        {
            OnAddressSavedError(exception.Message, infoModel);
        }

        private void OnAddressSavedError(String msgError, AddressInfoGrid infoModel)
        {
            infoModel.Message = msgError;
            infoModel.Status = SharedConstants.Client.RECORD_ERROR_SAVED;
            OnAddressChanged(infoModel);
        }

        private void OnAddressSaved(IStale<ResponseMessageData<AddressInfoModel>> obj, AddressInfoGrid infoModel)
        {
            if (obj.IsStale)
            {
                OnAddressSavedError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN, infoModel);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnAddressSavedError(obj.Data.Message, infoModel);
                return;
            }

            var modelSaved = obj.Data.Data;
            infoModel.AddressInfo.AddressId = modelSaved.AddressId;
            infoModel.Status = SharedConstants.Client.RECORD_SAVED;
            OnAddressChanged(infoModel);
        }

        protected virtual void OnClientChanged(ClientInfoGrid model)
        {
            var handler = ClientChanged;
            if (handler != null) handler(model);
        }


        protected virtual void OnAddressChanged(AddressInfoGrid model)
        {
            var handler = AddressChanged;
            if (handler != null) handler(model);
        }
    }
}
