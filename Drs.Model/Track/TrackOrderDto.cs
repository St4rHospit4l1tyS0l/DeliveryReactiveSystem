using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Drs.Model.Catalog;
using Drs.Model.Properties;
using Drs.Model.Shared;

namespace Drs.Model.Track
{
    public class TrackOrderDto : INotifyPropertyChanged
    {
        private bool _changeCancel;
        public string Phone { get; set; }
        public DateTime StartDatetime { get; set; }

        public string DateOrder
        {
            get
            {
                return StartDatetime.ToString("yyyy / MM / dd");
            }
        }
        public string DateTimeOrder
        {
            get
            {
                return StartDatetime.ToString("yyyy / MM / dd  |  HH:mm:ss");
            }
        }

        public string TimeOrder
        {
            get
            {
                return StartDatetime.ToString("HH:mm:ss");
            }
        }

        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrderAtoId { get; set; }
        public string StoreName { get; set; }
        public decimal OrderTotal { get; set; }
        public string CurrOrderTotal {
            get
            {
                return OrderTotal.ToString("C");
            }
        }
        public string LastStatus { get; set; }
        public string LastStatusEx
        {
            get
            {
                if (CatalogsClientModel.DicOrderStatus == null)
                    return String.Empty;

                ItemCatalog catalog;
                return CatalogsClientModel.DicOrderStatus.TryGetValue(LastStatus, out catalog) ? catalog.Name : LastStatus;

            }
        }
        public string LastStatusColor
        {
            get
            {
                if (CatalogsClientModel.DicOrderStatus == null)
                    return String.Empty;

                ItemCatalog catalog;
                return CatalogsClientModel.DicOrderStatus.TryGetValue(LastStatus, out catalog) ? catalog.Value : "#000000";

            }
        }

        public Visibility IsCancelVisible
        {
            get
            {
                if (IsCanceled.HasValue && IsCanceled.Value)
                    return Visibility.Hidden;


                if (CatalogsClientModel.DicOrderStatus == null)
                    return Visibility.Visible;

                ItemCatalog catalog;
                if(CatalogsClientModel.DicOrderStatus.TryGetValue(LastStatus, out catalog) == false)
                    return Visibility.Visible;

                return CatalogsClientModel.LstStatusCannotCancel.Any(e => e == LastStatus) ? Visibility.Hidden : Visibility.Visible;


            }
        }

        public long OrderToStoreId { get; set; }
        public bool? IsCanceled { get; set; }

        public bool ChangeCancel
        {
            get { return _changeCancel; }
            set
            {
                _changeCancel = value;
                OnPropertyChanged("IsCancelVisible");
            }
        }

        public string Agent { get; set; }
        public string MainAddress { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
