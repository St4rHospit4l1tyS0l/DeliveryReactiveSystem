using System;
using Drs.Infrastructure.Extensions.Proc;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Service.ReactiveDelivery;
using Drs.ViewModel.Shared;

namespace Drs.ViewModel.Order
{
    public class OrderPosVm : UcViewModelBase, IOrderPosVm
    {
        private readonly IReactiveDeliveryClient _client;

        public OrderPosVm(IReactiveDeliveryClient client)
        {
            _client = client;
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

    }
}
