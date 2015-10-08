using System.Runtime.Serialization;

namespace FranchiseChannel.Service.Model
{
    [DataContract]
    public class ResponseMessageFc
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public int TotalFiles { get; set; }
    }
}