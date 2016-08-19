using System.Runtime.Serialization;

namespace Drs.Model.Shared
{
    [DataContract]
    public class ItemCatalog
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SecondName { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Code { get; set; }
    }
}
