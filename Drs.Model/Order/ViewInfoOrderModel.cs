using System;
using System.Collections.Generic;
using Drs.Model.Settings;

namespace Drs.Model.Order
{
    public class ViewInfoOrderModel
    {
        public long OrderToStoreId { get; set; }
        public string Phone { get; set; }
        public string FranchiseName { get; set; }
        public string FranchiseStoreName { get; set; }
        public DateTime StartDatetime { get; set; }
        public string StartDatetimeTx {
            get
            {
                return StartDatetime.ToString(SettingsData.Constants.SystemConst.FORMAT_DATETIME_V1);
            }

        }
        public string TotalTx
        {
            get { return string.Format(SettingsData.Constants.SystemConst.CURRENCY_FORMAT, Total); }
        }

        public string LastStatus { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Reference { get; set; }
        public bool IsMap { get; set; }
        public string PlaceId { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public decimal Total { get; set; }
        public string UserNameIns { get; set; }
        public List<InfoItemModel> LstItems { get; set; }
        public string Notes { get; set; }
    }
}