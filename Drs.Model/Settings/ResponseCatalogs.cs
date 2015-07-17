using System.Collections.Generic;
using System.Runtime.Serialization;
using Drs.Model.Shared;

namespace Drs.Model.Settings
{
    [DataContract]
    public class ResponseCatalogs : ResponseMessage
    {
        [DataMember]
        public IList<ItemCatalog> LstPayments { get; set; }

        [DataMember]
        public IList<ItemCatalog> LstDeliveryStatus { get; set; }
    }
}