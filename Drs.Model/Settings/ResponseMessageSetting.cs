using System.Collections.Generic;
using System.Runtime.Serialization;
using Drs.Model.Shared;

namespace Drs.Model.Settings
{
    [DataContract]
    public class ResponseMessageSetting : ResponseMessage
    {
        [DataMember]
        public IDictionary<string, string> DicSettings { get; set; }

        [DataMember]
        public IList<ControlTitleModel> LstControls { get; set; }
    }
}
