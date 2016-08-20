using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions;
using Drs.Infrastructure.Extensions.Proc;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Service.Franchise;
using Drs.Service.ReactiveDelivery;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public class OrderPosVm : UcViewModelBase, IOrderPosVm
    {
        private readonly IReactiveDeliveryClient _client;

        public IReactiveCommand<Unit> ReloadPosCommand { get; set; }


        public OrderPosVm(IReactiveDeliveryClient client)
        {
            _client = client;
            ReloadPosCommand = ReactiveCommand.CreateAsyncTask(Observable.Return(true), _ => OnReloadPos());
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
            var response = ValidateModel(ClientFlags.ValidateOrder.Phone | ClientFlags.ValidateOrder.Franchise | ClientFlags.ValidateOrder.Client | ClientFlags.ValidateOrder.Address );

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
    }
}
