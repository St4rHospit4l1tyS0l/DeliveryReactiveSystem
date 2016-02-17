using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Infrastructure.Model;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Model.Shared;
using Drs.Model.UiView.Shared;
using Drs.Resources.Network;
using Drs.Service.ReactiveDelivery;
using Drs.Service.Sync;
using Drs.Service.TransferDto;
using Drs.ViewModel.Main;
using ReactiveUI;

namespace Drs.ViewModel.Pos
{
    public static class SyncPosFiles
    {
        public static void GetUnsyncFiles(IReactiveDeliveryClient reactiveDeliveryClient, Action showWnd, IShellContainerVm vm, ConnectionInfoResponse response)
        {
            var showWndSec = showWnd;
            reactiveDeliveryClient.ExecutionProxy.ExecuteRequest<ResponseMessageData<SyncFranchiseModel>, ResponseMessageData<SyncFranchiseModel>>
                (SharedConstants.Server.FRANCHISE_HUB, SharedConstants.Server.LIST_SYNC_FILES_FRANCHISE_HUB_METHOD, TransferDto.SameType)
                .ObserveOn(reactiveDeliveryClient.ConcurrencyService.Dispatcher)
                .SubscribeOn(reactiveDeliveryClient.ConcurrencyService.TaskPool)
                .Subscribe(e => OnGetUnsynFilesOk(e, showWnd, vm, response), i => OnGetUnsynFilesError(i, showWndSec));
        }

        private static void OnGetUnsynFilesOk(IStale<ResponseMessageData<SyncFranchiseModel>> obj, Action showWnd, IShellContainerVm vm,
            ConnectionInfoResponse responseAccount)
        {
            if (obj.IsStale)
            {
                OnGetUnsynFilesError(ResNetwork.ERROR_NETWORK_DOWN, showWnd);
                return;
            }

            if (obj.Data.IsSuccess == false)
            {
                OnGetUnsynFilesError(obj.Data.Message, showWnd);
                return;
            }

            var respMsg = SyncFileService.StartSyncFiles(obj.Data.LstData.ToList());

            if (respMsg.IsSuccess == false)
            {
                OnGetUnsynFilesError(respMsg.Message, showWnd);
                return;
            }

            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                try
                {
                    vm.ChangeCurrentView((StatusScreen)responseAccount.NxWn, false);
                    MessageBus.Current.SendMessage(responseAccount.Msg, SharedMessageConstants.INITIALIZE_ERROR_CHECK);
                    showWnd();
                }
                catch (Exception)
                {
                    OnGetUnsynFilesError("La respuesta del servidor es incorrecta. Revise que tenga la versión correcta en el cliente o en el servidor", showWnd);
                }
            });
        }


        private static void OnGetUnsynFilesError(Exception ex, Action showWnd)
        {
            OnGetUnsynFilesError(ex.Message, showWnd);
        }


        private static void OnGetUnsynFilesError(string msgError, Action showWnd)
        {
            MessageBus.Current.SendMessage(msgError, SharedMessageConstants.INITIALIZE_ERROR_CHECK);
            showWnd();
        }
    }
}
