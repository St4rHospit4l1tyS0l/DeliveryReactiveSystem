using System;
using System.ServiceModel;

namespace FranchiseChannel.Service.Model
{
    [MessageContract]
    public class RequestMessageFileSync
    {
        [MessageBodyMember]
        public Guid UidVersion { get; set; }

        [MessageBodyMember]
        public String FileName { get; set; }
    }
}