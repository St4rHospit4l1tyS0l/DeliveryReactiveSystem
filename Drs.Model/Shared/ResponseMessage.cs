using System.Runtime.Serialization;

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
    }
}
