using System;
using System.Collections.Generic;

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
    }
}
