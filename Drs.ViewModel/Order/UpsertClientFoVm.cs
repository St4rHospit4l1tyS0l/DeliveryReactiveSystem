using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public class UpsertClientFoVm : FlyoutBaseVm, IUpsertClientFoVm, IDataErrorInfo
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _company;
        private int? _companyId;
        private PhoneModel _secondPhone;
        private DateTime? _birthDate;
        private IAutoCompleteTextVm _companySearchVm;
        private readonly IReactiveDeliveryClient _client;
        private IAutoCompleteTextVm _secondPhoneVm;
        private string _loyaltyCode;

        public UpsertClientFoVm(IAutoCompleteTextVm companySearchVm, IAutoCompletePhoneVm secondPhoneVm, IReactiveDeliveryClient client)
        {
            _client = client;
            CompanySearchVm = companySearchVm;
            SecondPhoneVm = secondPhoneVm;

            ClientPreId = SharedConstants.NULL_ID_VALUE;
            ClientId = SharedConstants.NULL_ID_VALUE;

            CompanySearchVm.Watermark = "Empresa";
            CompanySearchVm.ExecuteSearch += ExecuteSearchCompany;
            CompanySearchVm.DoExecuteEvent += OnSelectCompany;

            SecondPhoneVm.ExecuteSearch += ExecuteSearchPhone;
            SecondPhoneVm.DoExecuteEvent += OnSelectPhone;

            var canSave = this.WhenAnyValue(vm => vm.FirstName, vm => vm.LastName, vm => vm.LoyaltyCode, (f, l, lc) =>
                !String.IsNullOrWhiteSpace(f) && f.Length >= 2 && f.Length <= 100
                && !String.IsNullOrWhiteSpace(l) && l.Length >= 2 && l.Length <= 200
                && (String.IsNullOrEmpty(lc) || lc.Length >= 2 && lc.Length <= 14));

            UpsertCommand = ReactiveCommand.CreateAsyncTask(canSave, _ => SaveClient());

        }

        private void OnSelectCompany(ListItemModel companyModel)
        {
            if (companyModel == null)
            {
                Company = null;
                CompanyId = null;
            }
            else
            {
                Company = companyModel.Value;
                int iValue;
                int.TryParse(companyModel.Key, out iValue);
                CompanyId = iValue;
            }
        }

        private void ExecuteSearchCompany(string companySearch)
        {
            _client.ExecutionProxy.ExecuteRequest
                <String, String, ResponseMessageData<ListItemModel>, ResponseMessageData<ListItemModel>>
                (companySearch, TransferDto.SameType, SharedConstants.Server.CLIENT_HUB,
                    SharedConstants.Server.SEARCH_BY_COMPANY_CLIENT_HUB_METHOD, TransferDto.SameType)
                .Subscribe(CompanySearchVm.OnResultReady, CompanySearchVm.OnResultError);

        }


        private void OnSelectPhone(ListItemModel model)
        {
            if (model == null || String.IsNullOrWhiteSpace(model.Value))
            {
                SecondPhone = null;
            }
            else
            {
                int iValue;
                int.TryParse(model.Key, out iValue);
                SecondPhone = new PhoneModel { Phone = model.Value, PhoneId = iValue };
            }
        }

        private void ExecuteSearchPhone(string phoneSearch)
        {
            _client.ExecutionProxy
                .ExecuteRequest<String, String, ResponseMessageData<ListItemModel>, ResponseMessageData<ListItemModel>>
                (phoneSearch, TransferDto.SameType, SharedConstants.Server.CLIENT_HUB,
                    SharedConstants.Server.SEARCH_BY_PHONE_CLIENT_HUB_METHOD, TransferDto.SameType)
                .Subscribe(SecondPhoneVm.OnResultReady, SecondPhoneVm.OnResultError);
        }

        private async Task<Unit> SaveClient()
        {
            await Task.Run(() =>
            {
                if (!IsOpen)
                    return;

                IsOpen = false;

                MessageBus.Current.SendMessage(
                    new ClientInfoGrid
                    {
                        ClientPreId = ClientPreId,
                        ClientInfo =
                        {
                            LoyaltyCode =  LoyaltyCode,
                            BirthDate = BirthDate,
                            Company = Company,
                            CompanyId = CompanyId,
                            Email = Email,
                            FirstName = FirstName,
                            LastName = LastName,
                            SecondPhone = SecondPhone,
                            ClientId = ClientId
                        }
                    }, SharedMessageConstants.ORDER_CLIENTINFO);
            });
            return new Unit();
        }

        public long ClientPreId { get; set; }

        public string FirstName
        {
            get { return _firstName; }
            set { this.RaiseAndSetIfChanged(ref _firstName, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { this.RaiseAndSetIfChanged(ref _lastName, value); }
        }

        public string Email
        {
            get { return _email; }
            set { this.RaiseAndSetIfChanged(ref _email, value); }
        }

        public string Company
        {
            get { return _company; }
            set { this.RaiseAndSetIfChanged(ref _company, value); }
        }

        public int? CompanyId
        {
            get { return _companyId; }
            set { this.RaiseAndSetIfChanged(ref _companyId, value); }
        }

        public PhoneModel SecondPhone
        {
            get { return _secondPhone; }
            set { this.RaiseAndSetIfChanged(ref _secondPhone, value); }
        }

        public DateTime? BirthDate
        {
            get { return _birthDate; }
            set { this.RaiseAndSetIfChanged(ref _birthDate, value); }
        }

        public string LoyaltyCode
        {
            get { return _loyaltyCode; }
            set { this.RaiseAndSetIfChanged(ref _loyaltyCode, value); }
        }

        public IReactiveCommand<Unit> UpsertCommand { get; private set; }

        public string this[string columnName]
        {
            get
            {
                if (IsInitialize == false || IsOpen == false || IsOpenFinished == false)
                    return null;

                Func<object, string> validator;
                if (!ClientInfoGrid.DicValidation.TryGetValue(columnName, out validator)) return null;
                var value = StaticReflection.GetValue(this, columnName);
                return validator(value);
            }
        }

        public IAutoCompleteTextVm CompanySearchVm
        {
            get { return _companySearchVm; }
            set { this.RaiseAndSetIfChanged(ref _companySearchVm, value); }
        }

        public IAutoCompleteTextVm SecondPhoneVm
        {
            get { return _secondPhoneVm; }
            set { this.RaiseAndSetIfChanged(ref _secondPhoneVm, value); }
        }

        public void Clean()
        {
            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                FirstName = String.Empty;
                LastName = String.Empty;
                Email = String.Empty;
                Company = String.Empty;
                CompanyId = null;
                CompanySearchVm.Search = String.Empty;
                CompanySearchVm.ListData.Clear();
                SecondPhone = null;
                SecondPhoneVm.Search = String.Empty;
                SecondPhoneVm.ListData.Clear();
                BirthDate = null;
                LoyaltyCode = String.Empty;
                ClientPreId = SharedConstants.NULL_ID_VALUE;
                ClientId = SharedConstants.NULL_ID_VALUE;
            });
        }

        public void Fill(ClientInfoGrid clInfo)
        {
            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                ClientPreId = clInfo.ClientPreId;
                ClientId = clInfo.ClientInfo.ClientId;
                FirstName = clInfo.ClientInfo.FirstName;
                LastName = clInfo.ClientInfo.LastName;
                Email = clInfo.ClientInfo.Email;
                Company = clInfo.ClientInfo.Company;
                CompanyId = clInfo.ClientInfo.CompanyId;
                CompanySearchVm.IsDone = SharedConstants.Client.IS_TRUE;
                CompanySearchVm.Search = clInfo.ClientInfo.Company;

                SecondPhone = clInfo.ClientInfo.SecondPhone;
                SecondPhoneVm.Search = SecondPhone != null ? SecondPhone.Phone : String.Empty;
                SecondPhoneVm.IsDone = SharedConstants.Client.IS_TRUE;

                BirthDate = clInfo.ClientInfo.BirthDate;
                LoyaltyCode = clInfo.ClientInfo.LoyaltyCode;
            });
        }

        public int? ClientId { get; set; }

        public string Error { get { return string.Empty; } }
    }
}
