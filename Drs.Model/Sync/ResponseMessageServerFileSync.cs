using System.IO;
using System.ServiceModel;

namespace Drs.Model.Sync
{
    [MessageContract]
    public class ResponseMessageServerFileSync
    {
        [MessageHeader(MustUnderstand = true)]
        public string Message { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public bool HasError { get; set; }

        [MessageBodyMember(Order = 1)]
        public Stream File { get; set; }
    }
}