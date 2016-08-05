using System;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Track
{
    public class SearchTrackOrderVm : UcViewModelBase, ISearchTrackOrderVm
    {
        private readonly IReactiveDeliveryClient _client;
        private IAutoCompleteTextVm _phoneSearchVm;
        private IAutoCompleteTextVm _nameSearchVm;

        public SearchTrackOrderVm(IAutoCompleteTextVm phoneSearch, IAutoCompleteTextVm nameSearchVm,  IReactiveDeliveryClient client)
        {
            _client = client;

            NameSearchVm = nameSearchVm;
            NameSearchVm.Watermark = "Ingrese el nombre o apellido del cliente";
            NameSearchVm.ExecuteSearch += ExecuteSearchClientName;
            NameSearchVm.DoExecuteEvent += FindByClientName;
            
            PhoneSearchVm = phoneSearch;
            PhoneSearchVm.Watermark = "Ingrese el teléfono";
            PhoneSearchVm.ExecuteSearch += ExecuteSearchPhone;
            PhoneSearchVm.DoExecuteEvent += FindByPhone;
        }

        private void FindByClientName(ListItemModel model)
        {
            if (!ValidateModel(model))
                return;
            OnClientNameChanged(model.IdKey ?? -1);            
        }

        public override bool Initialize(bool bForceToInit = false)
        {
            PhoneSearchVm.Search = String.Empty;
            PhoneSearchVm.IsFocused = true;
            NameSearchVm.Search = String.Empty;
            return base.Initialize(bForceToInit);
        }

        private void FindByPhone(ListItemModel model)
        {
            if (!ValidateModel(model))
                return;
            OnPhoneChanged(model.Value);
        }

        private bool ValidateModel(ListItemModel model)
        {
            if (String.IsNullOrWhiteSpace(model.Value))
                return false;

            //if(valur.Trim().Length >= SettingsData.Client.MinLengthPhone)
            //    return true;

            //MessageBus.Current.SendMessage(new MessageBoxSettings
            //{
            //    Message = String.Format("Debe ingresar al menos {0} caracteres", SettingsData.Client.MinLengthPhone),
            //    Title = "Rastreo de pedidos",
            //}, SharedMessageConstants.MSG_SHOW_ERRQST);
            return true;

        }

        private void ExecuteSearchPhone(string phoneSearch)
        {
            NameSearchVm.Search = String.Empty;
            _client.ExecutionProxy
                .ExecuteRequest
                <String, String, ResponseMessageData<ListItemModel>, ResponseMessageData<ListItemModel>>
                (phoneSearch, TransferDto.SameType, SharedConstants.Server.CLIENT_HUB,
                    SharedConstants.Server.SEARCH_BY_PHONE_CLIENT_HUB_METHOD, TransferDto.SameType)
                .Subscribe(PhoneSearchVm.OnResultReady, PhoneSearchVm.OnResultError);
        }


        private void ExecuteSearchClientName(string clientNameSearch)
        {
            PhoneSearchVm.Search = String.Empty;
            _client.ExecutionProxy
                .ExecuteRequest
                <String, String, ResponseMessageData<ListItemModel>, ResponseMessageData<ListItemModel>>
                (clientNameSearch, TransferDto.SameType, SharedConstants.Server.CLIENT_HUB,
                    SharedConstants.Server.SEARCH_BY_CLIENTNAME_CLIENT_HUB_METHOD, TransferDto.SameType)
                .Subscribe(NameSearchVm.OnResultReady, NameSearchVm.OnResultError);
        }

        public event Action<String> PhoneChanged;

        protected virtual void OnPhoneChanged(String phone)
        {
            var handler = PhoneChanged;
            if (handler != null) handler(phone);
        }


        public event Action<int> ClientNameChanged;

        protected virtual void OnClientNameChanged(int clientName)
        {
            var handler = ClientNameChanged;
            if (handler != null) handler(clientName);
        }

        public IAutoCompleteTextVm PhoneSearchVm
        {
            get { return _phoneSearchVm; }
            set { this.RaiseAndSetIfChanged(ref _phoneSearchVm, value); }
        }

        public IAutoCompleteTextVm NameSearchVm
        {
            get { return _nameSearchVm; }
            set { this.RaiseAndSetIfChanged(ref _nameSearchVm, value); }
        }
    }
}
