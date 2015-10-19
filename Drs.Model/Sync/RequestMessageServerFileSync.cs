using System;
using System.ServiceModel;

namespace Drs.Model.Sync
{
    [MessageContract]
    public class RequestMessageServerFileSync
    {
        [MessageBodyMember]
        public Guid UidVersion { get; set; }

        [MessageBodyMember]
        public String FileName { get; set; }
    }
}