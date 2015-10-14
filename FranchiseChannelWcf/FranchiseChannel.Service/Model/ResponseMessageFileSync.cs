using System.IO;
using System.ServiceModel;

namespace FranchiseChannel.Service.Model
{
    [MessageContract]
    public class ResponseMessageFileSync
    {
        [MessageHeader(MustUnderstand = true)]
        public string Message { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public bool HasError { get; set; }

        [MessageBodyMember(Order = 1)]
        public Stream File { get; set; }
    }
}