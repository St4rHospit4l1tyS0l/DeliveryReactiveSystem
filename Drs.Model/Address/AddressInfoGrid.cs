using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Drs.Infrastructure.Entity;
using Drs.Model.Annotations;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;

namespace Drs.Model.Address
{

    public class AddressInfoModel : RecordModel
    {
        public int? AddressId { get; set; }
        public string MainAddress { get; set; }
        public string ExtIntNumber { get; set; }
        public string Reference { get; set; }
        public ListItemModel Country { get; set; }
        public ListItemModel RegionA { get; set; }
        public ListItemModel RegionB { get; set; }
        public ListItemModel RegionC { get; set; }
        public ListItemModel RegionD { get; set; }
        public ListItemModel ZipCode { get; set; }
        public PhoneModel PrimaryPhone { get; set; }
    }

    public class AddressInfoGrid : RecordModel, INotifyPropertyChanged
    {
        private Visibility _isError;
        private Visibility _isSaveInProgress;
        private string _msgErr;
        private Visibility _isOk;
        private long _preId;

        public AddressInfoModel AddressInfo { get; set; }

        public long PreId
        {
            get { return _preId; }
            set
            {
                if(value != SharedConstants.NULL_ID_VALUE)
                    _preId = value;
            }
        }

        public bool IsSelected { get; set; }

        public Visibility IsOk
        {
            get { return _isOk; }
            set
            {
                _isOk = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsError
        {
            get { return _isError; }
            set
            {
                _isError = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsSaveInProgress
        {
            get { return _isSaveInProgress; }
            set
            {
                _isSaveInProgress = value;
                OnPropertyChanged();
            }
        }

        public string MsgErr
        {
            get { return _msgErr; }
            set
            {
                _msgErr = value;
                OnPropertyChanged();
            }
        }

        public static IDictionary<string, Func<object, string>> DicValidation = new Dictionary<string, Func<object, string>>();
        

        public AddressInfoGrid()
        {
            IsOk = Visibility.Collapsed;
            IsError = Visibility.Collapsed;
            IsSaveInProgress = Visibility.Collapsed;
            PreId = Generator.GenerateUniqueId();
            AddressInfo = new AddressInfoModel();
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