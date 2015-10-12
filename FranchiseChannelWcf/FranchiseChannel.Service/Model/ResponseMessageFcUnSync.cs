using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FranchiseChannel.Service.Model
{
    [DataContract]
    public class ResponseMessageFcUnSync : ResponseMessageFc
    {
        [DataMember]
        public List<UnSyncFilesModel> LstFiles { get; set; }
    }
}