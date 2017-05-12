using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Drs.Infrastructure.Extensions;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Infrastructure.Extensions.Io;
using Drs.Infrastructure.Extensions.Proc;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Model.Store;
using Drs.Repository.Log;
using Drs.Service.Franchise;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public class OrderPosVm : UcViewModelBase, IOrderPosVm
    {
        private readonly IReactiveDeliveryClient _client;
        private IDisposable _subscription;

        public IReactiveCommand<Unit> ReloadPosCommand { get; set; }
        public IReactiveList<StoreNotificationCategoryViewModel> LstNotificaionCategories { get; set; }


        public OrderPosVm(IReactiveDeliveryClient client)
        {
            _client = client;
            ReloadPosCommand = ReactiveCommand.CreateAsyncTask(Observable.Return(true), _ => OnReloadPos());
            LstNotificaionCategories = new ReactiveList<StoreNotificationCategoryViewModel>();
        }

        private async Task<Unit> OnReloadPos()
        {
            await Task.Run(() =>
            {
                if (!PosService.DeletePosFoldersDataAndNewDataIfPosIsDown())
                {
                    ShowPosApp();
                    return;
                }
                ReloadPosAction.SafeExecuteAction();
            });

            return new Unit();
        }

        public override ResponseMessage OnViewSelected(int iSelectedTab)
        {
            var response = ValidateModel(ClientFlags.ValidateOrder.Phone | ClientFlags.ValidateOrder.Franchise | ClientFlags.ValidateOrder.Client | ClientFlags.ValidateOrder.Address);

            if (response.IsSuccess)
                ShowPosApp();

            return response;
        }

        private void ShowPosApp()
        {
            ProcessExt.ForceSetFocusApp(SettingsData.AlohaIber.Replace(SettingsData.Constants.EXTENSION_EXE, String.Empty));
        }

        public Func<ClientFlags.ValidateOrder, ResponseMessage> ValidateModel { get; set; }

        public Action ReloadPosAction { get; set; }
        public void OnChangeStore(ItemCatalog item, bool bIsLastStore)
        {
            if (SettingsData.Store.EnableStoreNotifications == false)
                return;

            RxApp.MainThreadScheduler.Schedule(_ => LstNotificaionCategories.Clear());

            DisposeSubscription();

            _subscription = _client.ExecutionProxy.ExecuteRequest<int, int, ResponseMessageData<StoreNotificationCategoryModel>,
                ResponseMessageData<StoreNotificationCategoryModel>>(item.Id, TransferDto.SameType, SharedConstants.Server.STORE_HUB,
                SharedConstants.Server.GET_NOTIFICATIONS_BY_STORE_STORE_HUB_METHOD, TransferDto.SameType)
                .Subscribe(OnResultNotificationOk, OnResultNotificationError);

        }
        private void OnResultNotificationError(Exception ex)
        {
            OnResultNotificationError(ex.Message);
        }

        private void OnResultNotificationError(string sMsg)
        {
            MessageBus.Current.SendMessage(new MessageBoxSettings
            {
                Message = sMsg,
                Title = "Cargando la(s) dirección(es)",
            }, SharedMessageConstants.MSG_SHOW_ERRQST);
        }

        private void OnResultNotificationOk(IStale<ResponseMessageData<StoreNotificationCategoryModel>> obj)
        {

            if (obj.IsStale)
            {
                OnResultNotificationError(Resources.Network.ResNetwork.ERROR_NETWORK_DOWN);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnResultNotificationError(obj.Data.Message);
                return;
            }

            var lstData = (obj.Data.LstData as IList<StoreNotificationCategoryModel>) == null ? new List<StoreNotificationCategoryModel>() : obj.Data.LstData.ToList();

            if (!lstData.Any())
            {
                OnResultNotificationError("No hay una sucursal disponible en la dirección que seleccionó");
                return;
            }

            RxApp.MainThreadScheduler.Schedule(_ => 
            {
                try
                {
                    LstNotificaionCategories.ClearAndAddRange(
                        lstData.OrderBy(e => e.Position).Select(e => new StoreNotificationCategoryViewModel
                        {
                            CategoryName = e.CategoryName,
                            Color = e.Color,
                            NotificationsVm = e.Notifications.Select(i => new MessageNotificationVm
                            {
                                ItemImage = string.IsNullOrWhiteSpace(i.Resource)
                                    ? null
                                    : new BitmapImage(
                                        new Uri(
                                            (SharedConstants.Client.URI_IMAGE_NOTIFICATION + i.Resource)
                                                .AbsolutePathRelativeToEntryPointLocation())),
                                Message = i.Message
                            }).ToList(),
                            Position = e.Position
                        }));

                }
                catch (Exception ex)
                {
                    OnResultNotificationError(ex);
                }
            });
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
    }
}
