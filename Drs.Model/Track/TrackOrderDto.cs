using System;
using Drs.Model.Settings;

namespace Drs.Model.Track
{
    public class TrackOrderDto
    {
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
                return StartDatetime.ToString("HH : mm");
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
        public string LastStatusEx {
            get
            {
                string status;
                return SettingsData.Constants.TrackConst.LstOrderStatus.TryGetValue(LastStatus, out status) ? status : LastStatus;
            }
        }
        public long OrderToStoreId { get; set; }
    }
}
