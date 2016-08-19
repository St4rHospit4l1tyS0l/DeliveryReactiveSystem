using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Annotations;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Model.UiView.Shared;
using Drs.Repository.Log;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.ViewModel.Shared;
using ReactiveUI;


namespace Drs.ViewModel.Order
{
    public class LastOrderFoVm : FlyoutBaseVm, ILastOrderFoVm, IDataErrorInfo
    {
        private readonly IReactiveDeliveryClient _client;
        private PropagateOrderModel _propagateOrder;
        private string _titleLastOrder;
        private string _franchiseName;
        private bool _isGettingSelectedOrder;

        public string this[string columnName]
        {
            get
            {
                return null;
            }
        }

        public string Error { get; [UsedImplicitly] private set; }

        public string FranchiseName
        {
            get { return _franchiseName; }
            set { this.RaiseAndSetIfChanged(ref _franchiseName, value); }
        }

        public IReactiveList<QtItemModel> LstItems { get; set; }

        public IReactiveList<StackButtonModel> LstLastOrdersButtons { get; set; }

        public IReactiveCommand<Unit> DoLastOrderCommand { get; set; }

        public IReactiveCommand<Unit> DoEditLastOrderCommand { get; set; }

        public IReactiveCommand<Unit> OrderCommand { get; set; }


        public string TitleLastOrder
        {
            get { return _titleLastOrder; }
            set { this.RaiseAndSetIfChanged(ref _titleLastOrder, value); }
        }

        public string LastOrderDateTx
        {
            get { return _titleLastOrder; }
            set { this.RaiseAndSetIfChanged(ref _titleLastOrder, value); }
        }


        public LastOrderFoVm(IReactiveDeliveryClient client)
        {
            _client = client;
            LstItems = new ReactiveList<QtItemModel>();
            LstLastOrdersButtons = new ReactiveList<StackButtonModel>();
            var canOrder = this.WhenAny(vm => vm.PropagateOrder, x => x.Value != null);
            DoLastOrderCommand = ReactiveCommand.CreateAsyncTask(canOrder, _ => OnDoLastOrder(false));
            DoEditLastOrderCommand = ReactiveCommand.CreateAsyncTask(canOrder, _ => OnDoLastOrder(true));
            OrderCommand = ReactiveCommand.CreateAsyncTask(Observable.Return(true), OnDoOrderCommand);

            MessageBus.Current.Listen<String>(SharedMessageConstants.FLYOUT_LASTORDER_CLOSE).Subscribe(OnClose);
        }

        private async Task<Unit> OnDoOrderCommand(object o)
        {
            await Task.Run(() =>
            {
                var posOrderId = o as int?;

                if (posOrderId == null)
                    return;

                RxApp.MainThreadScheduler.Schedule(_ =>
                {
                    SingleSelectButton(posOrderId.Value);
                    OnPosOrderIdSelected(posOrderId.Value);
                });
            });

            return new Unit();
        }

        private void OnClose(string msg)
        {
            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                if (IsOpen)
                    IsOpen = false;

                LstItems.Clear();
                LstLastOrdersButtons.Clear();
            });

        }

        private async Task<Unit> OnDoLastOrder(bool hasEdit)
        {
            PropagateOrder.HasEdit = hasEdit;
            await Task.Run(() => MessageBus.Current.SendMessage(PropagateOrder, SharedMessageConstants.PROPAGATE_LASTORDER_FRANCHISE));
            RxApp.MainThreadScheduler.Schedule(_ => { IsOpen = false; });
            return new Unit();
        }


        public void ProcessPhone(ListItemModel model)
        {
            RxApp.MainThreadScheduler.Schedule(_ => { IsGettingSelectedOrder = true; });

            _client.ExecutionProxy.ExecuteRequest<String, String, ResponseMessageData<LastOrderInfoModel>, ResponseMessageData<LastOrderInfoModel>>
                    (model.Value, TransferDto.SameType, SharedConstants.Server.ORDER_HUB,
                        SharedConstants.Server.LAST_N_ORDERS_ORDER_HUB_METHOD, TransferDto.SameType)
                    .Subscribe(OnLastNthOrdersReady, OnLastNthOrdersError);
        }

        public void OnPosOrderIdSelected(int posOrderId)
        {
            RxApp.MainThreadScheduler.Schedule(_ => { IsGettingSelectedOrder = true; });

            _client.ExecutionProxy.ExecuteRequest<int, int, ResponseMessageData<PropagateOrderModel>, ResponseMessageData<PropagateOrderModel>>
                    (posOrderId, TransferDto.SameType, SharedConstants.Server.ORDER_HUB,
                        SharedConstants.Server.POS_ORDER_BYID_ORDER_HUB_METHOD, TransferDto.SameType)
                    .Subscribe(OnPosCheckReady, OnPosCheckError);

        }

        public PropagateOrderModel PropagateOrder
        {
            get { return _propagateOrder; }
            set { _propagateOrder = this.RaiseAndSetIfChanged(ref _propagateOrder, value); }
        }

        public bool IsGettingSelectedOrder
        {
            get { return _isGettingSelectedOrder; }
            set { _isGettingSelectedOrder = this.RaiseAndSetIfChanged(ref _isGettingSelectedOrder, value); }
        }

        private void OnLastNthOrdersError(Exception ex)
        {
            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                IsOpen = false;
            });
        }

        private void OnLastNthOrdersReady(IStale<ResponseMessageData<LastOrderInfoModel>> obj)
        {
            RxApp.MainThreadScheduler.Schedule(_ =>{IsOpen = false;});

            if (obj.IsStale || obj.Data.IsSuccess == false || obj.Data.LstData == null || !obj.Data.LstData.Any())
                return;

            try
            {
                RxApp.MainThreadScheduler.Schedule(_ =>
                {
                    PropagateOrder = null;
                    LstItems.Clear();
                    LstLastOrdersButtons.Clear();

                    foreach (var order in obj.Data.LstData)
                    {
                        LstLastOrdersButtons.Add(new StackButtonModel(order));
                    }

                    var firstPosOrderId = LstLastOrdersButtons[0].PosOrderId;
                    OnPosOrderIdSelected(firstPosOrderId);
                    SingleSelectButton(firstPosOrderId);
                    IsOpen = true;
                });
                
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                MessageBus.Current.SendMessage(new MessageBoxSettings
                {
                    Message = "Existe el siguiente problema al obtener los últimos pedidos: " + ex.Message,
                    Title = "Error al obtener los últimos pedidos",
                }, SharedMessageConstants.MSG_SHOW_ERRQST);
            }
        }

        private void SingleSelectButton(int firstPosOrderId)
        {
            var order = LstLastOrdersButtons.FirstOrDefault(e => e.PosOrderId == firstPosOrderId);
            if (order == null)
                return;

            if (!order.IsSelected)
                order.IsSelected = true;

            foreach (var orderNotSelected in LstLastOrdersButtons.Where(e => e.PosOrderId != firstPosOrderId))
                orderNotSelected.IsSelected = false;

        }


        private void OnPosCheckError(Exception ex)
        {
            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                IsGettingSelectedOrder = false;
                PropagateOrder = null;
                LstItems.Clear();
            });

            MessageBus.Current.SendMessage(new MessageBoxSettings
            {
                Message = "Existe el siguiente problema al obtener el pedido: " + ex.Message,
                Title = "Error al obtener el pedido",
            }, SharedMessageConstants.MSG_SHOW_ERRQST);
        }

        private void OnPosCheckReady(IStale<ResponseMessageData<PropagateOrderModel>> obj)
        {

            try
            {
                if (obj.IsStale || obj.Data.IsSuccess == false || obj.Data.Data == null ||
                    obj.Data.Data.PosCheck == null || obj.Data.Data.PosCheck.LstItems.Count == 0)
                {
                    OnPosCheckError(new Exception("No hay datos que mostrar. Información no válida"));
                    return;
                }

                RxApp.MainThreadScheduler.Schedule(_ =>
                {
                    IsGettingSelectedOrder = false;
                    LstItems.Clear();
                    PropagateOrder = obj.Data.Data;
                });


                try
                {
                    if (PropagateOrder != null && PropagateOrder.PosCheck != null)
                        PropagateOrder.PosCheck.FixItemParents();
                }
                catch (Exception ex)
                {
                    SharedLogger.LogError(ex);
                    MessageBus.Current.SendMessage(new MessageBoxSettings
                    {
                        Message = "Existe el siguiente problema al obtener el pedido: " + ex.Message,
                        Title = "Error al obtener el pedido",
                    }, SharedMessageConstants.MSG_SHOW_ERRQST);
                }

                RxApp.MainThreadScheduler.Schedule(_ =>
                {
                    var lstNewItems = PropagateOrder.PosCheck.LstItems.Select(e => new QtItemModel
                    {
                        ItemId = e.ItemId,
                        Name = e.Name,
                        Quantity = 1
                    }).ToList();

                    foreach (var item in lstNewItems)
                    {
                        LstItems.Add(item);
                    }

                    LastOrderDateTx = PropagateOrder.PosCheck.OrderDateTime.ToString(" dd/MM/yyyy |  HH:mm:ss");
                    TitleLastOrder = String.Format("Pedido del {0}", PropagateOrder.Order.Phone);
                    FranchiseName = String.Format("Franquicia: {0}", PropagateOrder.Franchise.Name);
                    IsOpen = true;
                });
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                MessageBus.Current.SendMessage(new MessageBoxSettings
                {
                    Message = "Existe el siguiente problema al obtener la información del pedido: " + ex.Message,
                    Title = "Error al obtener el pedido",
                }, SharedMessageConstants.MSG_SHOW_ERRQST);
            }
        }
    }
}
