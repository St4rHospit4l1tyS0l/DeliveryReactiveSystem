using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Drs.Model.Store
{
    [DataContract]
    public class OrderStoreModel
    {
        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string FirstName { get; set; }
        
        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string OrderId { get; set; }

        [DataMember]
        public DateTime OrderTime { get; set; }

        [DataMember]
        public double Total { get; set; }

        [DataMember]
        public List<OrderStoreItemModel> LstOrderItems { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public string ReferenceId { get; set; }
    }
}