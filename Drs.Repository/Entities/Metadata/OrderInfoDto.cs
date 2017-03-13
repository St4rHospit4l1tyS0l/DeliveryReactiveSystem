using System;
using Drs.Model.Settings;

namespace Drs.Repository.Entities.Metadata
{
    public class OrderInfoDto : ViewOrderInfo
    {
        public string StartDatetimeTx
        {
            get { return StartDatetime.ToString(SettingsData.Constants.SystemConst.FORMAT_DATETIME_V1); }
        }

        public string TotalTx
        {
            get { return string.Format(SettingsData.Constants.SystemConst.CURRENCY_FORMAT, Total); }
        }

    }
}