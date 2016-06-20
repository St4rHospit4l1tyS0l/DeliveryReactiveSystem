using System;
using System.Collections.Generic;
using Drs.Model.Settings;

namespace Drs.Model.Shared
{
    public class ResponseMessageData<TModel>
    {
        public ResponseMessageData()
        {
            Message = String.Empty;
        }

        public bool IsSuccess { get; set; }
        public IEnumerable<TModel> LstData { get; set; }
        public TModel Data { get; set; }
        public string Message { get; set; }
        public PagerModel Pager { get; set; }

        public static ResponseMessageData<TModel> CreateCriticalMessage(String sMessage)
        {
            return new ResponseMessageData<TModel>
            {
                IsSuccess = false,
                Message = String.Format("{0}. {1}.", sMessage, SettingsData.Resources.CONTACT_SUPPORT)
            };
        }
    }
}
