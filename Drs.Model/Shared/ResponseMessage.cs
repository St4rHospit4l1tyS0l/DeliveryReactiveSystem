using System.Runtime.Serialization;
using Drs.Model.Account;

namespace Drs.Model.Shared
{
    [DataContract]
    public class ResponseMessage
    {
        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public string Message { get; set; }

        public int View { get; set; }

        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public UserDetailModel UserDetail { get; set; }
    }
}
