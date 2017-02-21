using System.Runtime.Serialization;

namespace Drs.Model.Shared
{
    [DataContract]
    public class ResponseMessageShared
    {
        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public string Message { get; set; }

    }
}
