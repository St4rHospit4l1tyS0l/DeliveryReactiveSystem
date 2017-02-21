using System.Collections.Generic;
using System.Runtime.Serialization;
using Drs.Model.Shared;

namespace Drs.Model.Store
{
    [DataContract]
    public class ResponseStoreOrdersMessage : ResponseMessageShared
    {
        [DataMember]
        public List<OrderStoreModel> LstOrders { get; set; }
    }
}
    