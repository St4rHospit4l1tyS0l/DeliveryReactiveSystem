using System;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public class SearchNewPhoneVm : UcViewModelBase, ISearchNewPhoneVm
    {
        private readonly IReactiveDeliveryClient _client;
        private IAutoCompleteTextVm _phoneSearchVm;

        public SearchNewPhoneVm(IAutoCompleteTextVm phoneSearch, IReactiveDeliveryClient client)
        {
            _client = client;
            PhoneSearchVm = phoneSearch;
            PhoneSearchVm.ExecuteSearch += ExecuteSearchPhone;
            PhoneSearchVm.DoExecuteEvent += FindOrCreateClient;
        }

        public override bool Initialize(bool bForceToInit = false)
        {
            PhoneSearchVm.Search = String.Empty;
            return base.Initialize(bForceToInit);
        }

        private void FindOrCreateClient(ListItemModel model)
        {
            if (!ValidatePhone(model)) 
                return;
            
            MessageBus.Current.SendMessage(model, SharedMessageConstants.ORDER_CLIENTPHONE);
            GoToNextStep(SharedConstants.Client.ORDER_TAB_FRANCHISE);
        }

        private bool ValidatePhone(ListItemModel model)
        {
            var phone = model.Value;
            if (String.IsNullOrWhiteSpace(phone))
            {
                MessageBus.Current.SendMessage(new MessageBoxSettings
                {
                    Message = "No ha definido un teléfono para tomar la orden",
                    Title = "Error de validación",
                }, SharedMessageConstants.MSG_SHOW_ERRQST);
                return false;
            }

            string msgError;
            if (phone.Trim().Length < SettingsData.Client.MinLengthPhone)
            {
                msgError = "La longitud mínima del teléfono es de " + SettingsData.Client.MinLengthPhone;
            }
            else if (phone.Trim().Length > SettingsData.Client.MaxLengthPhone)
            {
                msgError = "La longitud máxima del teléfono es de " + SettingsData.Client.MaxLengthPhone;
            }
            else
            {
                return true;
            }
            
            MessageBus.Current.SendMessage(new MessageBoxSettings
            {
                Message = msgError,
                Title = "Error de validación",
            }, SharedMessageConstants.MSG_SHOW_ERRQST);
            return false;
        }

        private void ExecuteSearchPhone(string phoneSearch)
        {
            _client.ExecutionProxy
                .ExecuteRequest
                <String, String, ResponseMessageData<ListItemModel>, ResponseMessageData<ListItemModel>>
                (phoneSearch, TransferDto.SameType, SharedConstants.Server.CLIENT_HUB,
                    SharedConstants.Server.SEARCH_BY_PHONE_CLIENT_HUB_METHOD, TransferDto.SameType)
                .Subscribe(PhoneSearchVm.OnResultReady, PhoneSearchVm.OnResultError);
        }


        public IAutoCompleteTextVm PhoneSearchVm
        {
            get { return _phoneSearchVm; }
            set { this.RaiseAndSetIfChanged(ref _phoneSearchVm, value); }
        }
    }
}
