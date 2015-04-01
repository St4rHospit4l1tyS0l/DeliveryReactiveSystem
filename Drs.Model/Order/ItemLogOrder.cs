using System;
using Drs.Model.Settings;

namespace Drs.Model.Order
{
    public class ItemLogOrder
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }

        public string StrTimestamp
        {
            get
            {
                return Timestamp.ToString("yyyy / MM / dd  |  HH:mm:ss");
            }
        }
        public string Status { get; set; }
        public string StatusEx {
            get
            {
                string status;
                return SettingsData.Constants.TrackConst.LstOrderStatus.TryGetValue(Status, out status) ? status : Status;
            }
        }
    }
}