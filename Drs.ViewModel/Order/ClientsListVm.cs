using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Client;
using Drs.Model.Client.Recurrence;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Repository.Shared;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.ViewModel.Main;
using Drs.ViewModel.Shared;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public class ClientsListVm : UcViewModelBase, IClientsListVm
    {
        private IUpsertClientFoVm _upsertClient;
        private readonly IReactiveDeliveryClient _client;
        private ClientInfoGrid _clientSelection;
        private Func<OrderModel> _orderModel;
        private bool _isGettingData;

        public ClientsListVm(IUpsertClientFoVm upsertClientFo, IReactiveDeliveryClient client)
        {
            _upsertClient = upsertClientFo;
            _client = client;
            LstChildren.Add(_upsertClient);
            AddClient = ReactiveCommand.CreateAsyncTask(Observable.Return(true), _ => OnAddClient());
            EditClient = ReactiveCommand.CreateAsyncTask(Observable.Return(true), OnEditClient);
            RemoveClient = ReactiveCommand.CreateAsyncTask(Observable.Return(true), OnRemoveClient);
            RetrySaveClient = ReactiveCommand.CreateAsyncTask(Observable.Return(true), OnRetrySaveClient);

            MessageBus.Current.Listen<PropagateOrderModel>(SharedMessageConstants.PROPAGATE_LASTORDER_CLIENT).Subscribe(OnPropagate);
        }

        private void OnPropagate(PropagateOrderModel model)
        {
            var client = LstClients.FirstOrDefault(e => e.ClientInfo.ClientId == model.Order.ClientId);

            if (client == null)
                return;

            RxApp.MainThreadScheduler.Schedule(_ => { ClientSelection = client; });

            if (model.Order != null)
                MessageBus.Current.SendMessage(model, SharedMessageConstants.PROPAGATE_LASTORDER_ADDRESS);
        }

        public override ResponseMessage OnViewSelected(int iSelectedTab)
        {
            var response = ValidateModel(ClientFlags.ValidateOrder.Phone | ClientFlags.ValidateOrder.Franchise);
            return response;
        }

        protected override void OnShellContainerVmChange(IShellContainerVm value)
        {
            base.OnShellContainerVmChange(value);
            ShellContainerVm.AddOrUpdateFlyouts(_upsertClient);
        }

        public IReactiveCommand<Unit> AddClient { get; set; }
        public IReactiveCommand<Unit> EditClient { get; set; }
        public IReactiveCommand<Unit> RemoveClient { get; set; }
        
        public ClientInfoGrid ClientSelection
        {
            get { return _clientSelection; }
            set
            {
                this.RaiseAndSetIfChanged(ref _clientSelection, value);
                ClearAndSelect(_clientSelection);
                OnClientSelected(_clientSelection);
            }
        }

        public bool IsGettingData
        {
            get { return _isGettingData; }
            set { this.RaiseAndSetIfChanged(ref _isGettingData, value); }
        }

        private void ClearAndSelect(ClientInfoGrid clientSelection)
        {
            foreach (var client in LstClients)
            {
                client.IsSelected = false;
            }
            
            if (clientSelection != null)
                clientSelection.IsSelected = true;
            else if (LstClients.Count > 0)
                LstClients[0].IsSelected = true;

        }

        public IReactiveCommand<Unit> RetrySaveClient { get; set; }

        public Func<ClientFlags.ValidateOrder, ResponseMessage> ValidateModel { get; set; }

        public void SetOrderModel(Func<OrderModel> orderModel)
        {
            _orderModel = orderModel;
        }


        public void OnClientChanged(ClientInfoGrid obj)
        {
            OnClientChanged(obj, true);
        }

        public void OnClientChanged(ClientInfoGrid model, bool bHasToSelect)
        {
            if (model != null)
            {
                switch (model.Status)
                {
                    case SharedConstants.Client.RECORD_ERROR_SAVED:
                        model.IsClientOk = Visibility.Collapsed;
                        model.IsClientError = Visibility.Visible;
                        model.IsClientSaveInProgress = Visibility.Collapsed;
                        model.ClientMsgErr = model.Message;
                        break;
                    case SharedConstants.Client.RECORD_ONPROGRESS_TO_SAVED:
                        model.IsClientOk = Visibility.Collapsed;
                        model.IsClientError = Visibility.Collapsed;
                        model.IsClientSaveInProgress = Visibility.Visible;
                        break;
                    case SharedConstants.Client.RECORD_SAVED:
                        model.IsClientOk = Visibility.Visible;
                        model.IsClientError = Visibility.Collapsed;
                        model.IsClientSaveInProgress = Visibility.Collapsed;
                        break;
                }
            }
            
            if(bHasToSelect)
                RxApp.MainThreadScheduler.Schedule(_ => { ClientSelection = model; });
        }

        public void ProcessPhone(ListItemModel model)
        {
            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                IsGettingData = true;
                LstClients.Clear(); 
            });
            _client.ExecutionProxy
                .ExecuteRequest<String, String, ResponseMessageData<ClientInfoModel>, ResponseMessageData<ClientInfoModel>>
                (model.Value, TransferDto.SameType, SharedConstants.Server.CLIENT_HUB,
                    SharedConstants.Server.SEARCH_CLIENTS_BY_PHONE_CLIENT_HUB_METHOD, TransferDto.SameType)
                .Subscribe(OnClientsListReady, OnClientsListError);
        }

        public event Action<ClientInfoGrid> ClientSelected;

        protected virtual void OnClientSelected(ClientInfoGrid obj)
        {
            var handler = ClientSelected;
            if (handler != null) handler(obj);
        }

        private void OnClientsListReady(IStale<ResponseMessageData<ClientInfoModel>> obj)
        {
            if (obj.IsStale)
            {
                OnClientsListError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnClientsListError(obj.Data.Message);
                return;
            }

            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                IsGettingData = false;
                var lstRecurrence = new List<long>(obj.Data.LstData.Count());
                LstClients.Clear();
                var bIsFirst = true;
                foreach (var clientInfo in obj.Data.LstData)
                {
                    var clientGrid = new ClientInfoGrid
                    {
                        ClientInfo = clientInfo,
                        Status = SharedConstants.Client.RECORD_SAVED,
                        IsSelected = bIsFirst
                    };

                    lstRecurrence.Add(clientInfo.ClientId ?? EntityConstants.NULL_VALUE);

                    OnClientChanged(clientGrid, bIsFirst);
                    LstClients.Add(clientGrid);
                    bIsFirst = false;
                }


                if (lstRecurrence.Count != 0)
                {
                    _client.ExecutionProxy.ExecuteRequest<IList<long>, IList<long>, ResponseMessageData<RecurrenceResponseModel>, ResponseMessageData<RecurrenceResponseModel>>
                        (lstRecurrence, TransferDto.SameType, SharedConstants.Server.CLIENT_HUB,
                        SharedConstants.Server.CALCULATE_RECURRENCE_CLIENT_HUB_METHOD, TransferDto.SameType)
                        .Subscribe(OnRecurrenceClientsOk, OnRecurrenceClientsError);
                }

                if(bIsFirst)
                    OnClientChanged(null, true);

            });
        }

        private void OnRecurrenceClientsError(Exception obj)
        {
        }

        private void OnRecurrenceClientsOk(IStale<ResponseMessageData<RecurrenceResponseModel>> obj)
        {
            if (obj.IsStale)
                return;

            if (obj.Data.IsSuccess == false || obj.Data.Data == null || obj.Data.Data.LstRecurrence.Count == 0)
                return;

            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                
                var lstClientRecurrence = obj.Data.Data.LstRecurrence;

                foreach (var clientRecurrence in lstClientRecurrence)
                {
                    var client = LstClients.FirstOrDefault(e => e.ClientInfo.ClientId == clientRecurrence.ClientId);

                    if(client == null)
                        continue;

                    client.ClientInfo.LstRecurrence = clientRecurrence.ToListRecurrence();

                }

            });
        }

        private void OnClientsListError(Exception ex)
        {
            OnClientsListError(ex.Message);
        }

        private void OnClientsListError(string msgErr)
        {
            MessageBus.Current.SendMessage(new MessageBoxSettings
            {
                Message = msgErr,
                Title = "Cargando información de o los clientes",
            }, SharedMessageConstants.MSG_SHOW_ERRQST);
        }

        public IReactiveList<ClientInfoGrid> LstClients
        {
            get
            {
                var orderModel = _orderModel();
                return orderModel == null ? null : orderModel.LstClientInfo;
            }
        }

        public IUpsertClientFoVm UpsertClient
        {
            get { return _upsertClient; }
            set { this.RaiseAndSetIfChanged(ref _upsertClient, value); }
        }

        public async Task<Unit> OnAddClient()
        {
            await Task.Run(() =>
            {
                var response = ValidateModel(ClientFlags.ValidateOrder.Phone | ClientFlags.ValidateOrder.Franchise);
                if (response.IsSuccess == false)
                {
                    MessageBus.Current.SendMessage(new MessageBoxSettings
                    {
                        Message = response.Message,
                        Title = "Información faltante",
                    }, SharedMessageConstants.MSG_SHOW_ERRQST);
                    return;
                }
                UpsertClient.Clean();
                UpsertClient.IsOpen = true;
            });
            return new Unit();
        }

        public async Task<Unit> OnEditClient(object oRow)
        {
            await Task.Run(() =>
            {
                var clInfo = oRow as ClientInfoGrid;
                if (clInfo == null)
                    return;

                UpsertClient.Fill(clInfo);
                UpsertClient.IsOpen = true;
            });
            return new Unit();
        }


        private async Task<Unit> OnRetrySaveClient(object o)
        {
            var lstItemSelected = o as ObservableCollection<object>;

            if (lstItemSelected != null && lstItemSelected.Count > 0)
            {
                var model = lstItemSelected[0] as ClientInfoGrid;
                if (model != null)
                    await Task.Run(() => MessageBus.Current.SendMessage(model, SharedMessageConstants.ORDER_CLIENTINFO));
            }

            return new Unit();
        }

        public async Task<Unit> OnRemoveClient(object oRow)
        {
            await Task.Run(() =>
            {
                var clInfo = oRow as ClientInfoGrid;
                if (clInfo == null || clInfo.ClientInfo == null || clInfo.ClientInfo.PrimaryPhone == null || clInfo.ClientInfo.ClientId == null)
                    return;

                MessageBus.Current.SendMessage(new MessageBoxSettings
                {
                    Message = "¿Desea eliminar este cliente de la lista?",
                    Title = "Eliminar el cliente",
                    Settings = new MetroDialogSettings
                    {
                        AffirmativeButtonText = "eliminar",
                        NegativeButtonText = "cancelar"
                    },
                    Style = MessageDialogStyle.AffirmativeAndNegative,
                    Callback = (x => DoRemoveClient(x, clInfo))

                }, SharedMessageConstants.MSG_SHOW_ERRQST);

            });
            return new Unit();
        }

        private void DoRemoveClient(MessageDialogResult result, ClientInfoGrid clInfo)
        {
            if (result != MessageDialogResult.Affirmative)
                return;

            var relClientPhone = new ClientPhoneModel
            {
                ClientId = clInfo.ClientInfo.ClientId ?? SharedConstants.NULL_ID_VALUE,
                ClientPhoneId = clInfo.ClientInfo.PrimaryPhone.PhoneId
            };

            _client.ExecutionProxy
                .ExecuteRequest<ClientPhoneModel, ClientPhoneModel, ResponseMessageData<bool>, ResponseMessageData<bool>>
                (relClientPhone, TransferDto.SameType, SharedConstants.Server.CLIENT_HUB,
                    SharedConstants.Server.REMOVE_REL_PHONECLIENT_CLIENT_HUB_METHOD, TransferDto.SameType)
                .Subscribe(x => OnRemoveDone(x, clInfo), OnRemoveError);


        }

        private void OnRemoveError(Exception ex)
        {
            OnRemoveError(ex.Message);
        }
        private void OnRemoveError(String msgError)
        {
            MessageBus.Current.SendMessage(new MessageBoxSettings
            {
                Message = String.Format("No fue posible eliminar el cliente debido a: {0}", msgError),
                Title = "Eliminar el cliente"
            }, SharedMessageConstants.MSG_SHOW_ERRQST);
        }

        private void OnRemoveDone(IStale<ResponseMessageData<bool>> obj, ClientInfoGrid clInfo)
        {
            if (obj.IsStale){
                OnRemoveError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnRemoveError(obj.Data.Message);
                return;
            }

            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                var client = LstClients.FirstOrDefault(e => e.ClientPreId == clInfo.ClientPreId);
                if(client != null)
                    LstClients.Remove(client);
            });
        }
    }
}
