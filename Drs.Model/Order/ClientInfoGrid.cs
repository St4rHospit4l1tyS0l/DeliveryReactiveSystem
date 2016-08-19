using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Drs.Infrastructure.Entity;
using Drs.Infrastructure.Extensions;
using Drs.Model.Client.Recurrence;
using Drs.Model.Constants;
using Drs.Model.Properties;
using Drs.Model.Shared;
using Drs.Model.Validation;

namespace Drs.Model.Order
{

    public class ClientInfoModel : RecordModel
    {
        public int? ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public int? CompanyId { get; set; }
        public PhoneModel PrimaryPhone { get; set; }
        public PhoneModel SecondPhone { get; set; }
        public DateTime? BirthDate { get; set; }
        public List<RecurrenceClientView> LstRecurrence { get; set; }
        public string LoyaltyCode { get; set; }

        public ClientInfoModel()
        {
            LstRecurrence = new List<RecurrenceClientView> { new RecurrenceClientView(), new RecurrenceClientView(), new RecurrenceClientView(), new RecurrenceClientView()};            
        }

    }

    public class ClientInfoGrid : RecordModel, INotifyPropertyChanged
    {
        private Visibility _isClientError;
        private Visibility _isClientSaveInProgress;
        private string _clientMsgErr;
        private Visibility _isClientOk;
        private long _clientPreId;

        public ClientInfoModel ClientInfo { get; set; }

        public long ClientPreId
        {
            get { return _clientPreId; }
            set
            {
                if(value != SharedConstants.NULL_ID_VALUE)
                    _clientPreId = value;
            }
        }

        public bool IsSelected { get; set; }

        public Visibility IsClientOk
        {
            get { return _isClientOk; }
            set
            {
                _isClientOk = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsClientError
        {
            get { return _isClientError; }
            set
            {
                _isClientError = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsClientSaveInProgress
        {
            get { return _isClientSaveInProgress; }
            set
            {
                _isClientSaveInProgress = value;
                OnPropertyChanged();
            }
        }

        public string ClientMsgErr
        {
            get { return _clientMsgErr; }
            set
            {
                _clientMsgErr = value;
                OnPropertyChanged();
            }
        }

        public static IDictionary<string, Func<object, string>> DicValidation = new Dictionary<string, Func<object, string>>();
        
        static ClientInfoGrid()
        {
            DicValidation.Add(new KeyValuePair<string, Func<object, string>>(StaticReflection.GetMemberName<ClientInfoGrid>(x => x.ClientInfo.FirstName), value => GenericValidator.ValidateEmptyAndLength(value, 2, 100)));
            DicValidation.Add(new KeyValuePair<string, Func<object, string>>(StaticReflection.GetMemberName<ClientInfoGrid>(x => x.ClientInfo.LastName), value => GenericValidator.ValidateEmptyAndLength(value, 2, 200)));
            DicValidation.Add(new KeyValuePair<string, Func<object, string>>(StaticReflection.GetMemberName<ClientInfoGrid>(x => x.ClientInfo.LoyaltyCode), value => GenericValidator.ValidateLength(value, 5, 14)));
        }

        public ClientInfoGrid()
        {
            IsClientOk = Visibility.Collapsed;
            IsClientError = Visibility.Collapsed;
            IsClientSaveInProgress = Visibility.Collapsed;
            ClientPreId = Generator.GenerateUniqueId();
            ClientInfo = new ClientInfoModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}