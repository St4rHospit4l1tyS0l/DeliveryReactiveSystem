using System.Runtime.Serialization;

namespace FranchiseChannel.Service.Model
{
    [DataContract]
    public class UnSyncFilesModel
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string CheckSum { get; set; }

    }
}