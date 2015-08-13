using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
        private PropagateOrderModel _propagateOrder;
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

        public string FranchiseName
        {
            get { return _franchiseName; }
            set { this.RaiseAndSetIfChanged(ref _franchiseName, value); }
        }

        public IReactiveList<QtItemModel> LstItems { get; set; }

        public IReactiveCommand<Unit> DoLastOrderCommand { get; set; }

        public IReactiveCommand<Unit> DoEditLastOrderCommand { get; set; }


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
            DoLastOrderCommand = ReactiveCommand.CreateAsyncTask(Observable.Return(true), _ => OnDoLastOrder(false));
            DoEditLastOrderCommand = ReactiveCommand.CreateAsyncTask(Observable.Return(true), _ => OnDoLastOrder(true));
            MessageBus.Current.Listen<String>(SharedMessageConstants.FLYOUT_LASTORDER_CLOSE).Subscribe(OnClose);
        }

        private void OnClose(string msg)
        {
            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                if (IsOpen)
                    IsOpen = false;
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
            _client.ExecutionProxy.ExecuteRequest<String, String, ResponseMessageData<PropagateOrderModel>, ResponseMessageData<PropagateOrderModel>>
                    (model.Value, TransferDto.SameType, SharedConstants.Server.ORDER_HUB,
                        SharedConstants.Server.LAST_ORDER_ORDER_HUB_METHOD, TransferDto.SameType)
                    .Subscribe(e => OnPosCheckReady(e, model.Value), OnPosCheckError);

                //_client.ExecutionProxy.ExecuteRequest<PosCheck, PosCheck, PosCheck, PosCheck>(PosCheck,
                //    TransferDto.SameType, SharedConstants.Server.ORDER_HUB, SharedConstants.Server.CALCULATE_PRICES_ORDER_HUB_METHOD,
                //    TransferDto.SameType).Subscribe(OnCalculatePricesOk, OnCalculatePricesError);
                ////IsOnSignIn = Visibility.Collapsed;
                //IsOnWaiting = Visibility.Visible;
                //var pass = (PasswordBox)x;
                //using (var loginSvc = new LoginSvcClient())
                //{
                //    try
                //    {
                //        var response =
                //            await
                //                loginSvc.LoginAsync(new LoginModel
                //                {
                //                    Password = Cypher.Encrypt(pass.Password),
                //                    Username = Cypher.Encrypt(UserName)
                //                });
                //        return response;
                //    }
                //    catch (Exception ex)
                //    {
                //        return new ResponseMessage { IsSuccess = false, Message = ResNetwork.ERROR_NETWORK_DOWN + ex.Message};
                //    }
                //    finally
                //    {
                //        pass.Password = String.Empty;
                //        IsOnSignIn = Visibility.Visible;
                //        IsOnWaiting = Visibility.Collapsed;
                //        MessageBus.Current.SendMessage(String.Empty, SharedMessageConstants.LOGIN_FOCUS_USERNAME);
                //    }
                //}
            //    return null;
            //});


            DoLastOrderCommand.Subscribe(x =>
            {
                //if (x.IsSuccess)
                //{
                //    var bForceToInit = CurrentUserSettings.UserInfo.Username == null ||
                //                        CurrentUserSettings.UserInfo.Username != UserName;

                //    CurrentUserSettings.UserInfo.Username = UserName; 
                //    ShellContainerVm.ChangeCurrentView(StatusScreen.ShMenu, true, bForceToInit);  //Only when comes from login, it should be reinit
                //}
                //else
                //{
                //    MessageBus.Current.SendMessage(new MessageBoxSettings
                //    {
                //        Message = x.Message,
                //        Title = "Error al ingresar",
                //    }, SharedMessageConstants.MSG_SHOW_ERRQST);
                //    //Message = x.Message;
                //}
            });
        }

        //private void OnCalculatePricesError(object o)
        //{
        //    throw new NotImplementedException();
        //}

        //private void OnCalculatePricesOk(IStale<PosCheck> stale)
        //{
        //    throw new NotImplementedException();
        //}

        public PropagateOrderModel PropagateOrder
        {
            get { return _propagateOrder; }
            set { _propagateOrder = this.RaiseAndSetIfChanged(ref _propagateOrder, value); }
        }


        private void OnPosCheckError(Exception ex)
        {
            IsOpen = false;
        }

        private void OnPosCheckReady(IStale<ResponseMessageData<PropagateOrderModel>> obj, string phone)
        {

            if (obj.IsStale)
                return;

            if (obj.Data.IsSuccess == false || obj.Data.Data == null || obj.Data.Data.PosCheck == null || obj.Data.Data.PosCheck.LstItems.Count == 0)
                return;

            PropagateOrder = obj.Data.Data;

            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                LstItems.Clear();

                var lstNewItems = PropagateOrder.PosCheck.LstItems.GroupBy(e => new { e.ItemId, e.RealName, e.ParentId }).Select(e => new QtItemModel
                {
                    ItemId = e.Key.ItemId,
                    Name = e.Key.RealName,
                    Quantity = e.Count()
                }).ToList();

                foreach (var item in lstNewItems)
                {
                    LstItems.Add(item);
                }

                LastOrderDateTx = PropagateOrder.PosCheck.OrderDateTime.ToString(" dd/MM/yyyy |  HH:mm:ss");
                TitleLastOrder = String.Format("Último pedido de {0}", phone);
                FranchiseName = String.Format("Franquicia: {0}", PropagateOrder.Franchise.Name);
                IsOpen = true;
            });
        }

    }
}
