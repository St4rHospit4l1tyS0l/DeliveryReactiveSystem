using System.Runtime.Serialization;

namespace Drs.Model.Settings
{
    [DataContract]
    public class ControlTitleModel
    {
        [DataMember]
        public string Container { get; set; }
        
        [DataMember]
        public string ControlName { get; set; }
        
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsEnabled { get; set; }

        [DataMember]
        public string Validation { get; set; }
    }
}
