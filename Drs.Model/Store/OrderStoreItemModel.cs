using System.Runtime.Serialization;

namespace Drs.Model.Store
{
    [DataContract]
    public class OrderStoreItemModel
    {
        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public long? ParentId { get; set; }

        [DataMember]
        public int Level { get; set; }
    }
}