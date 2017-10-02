//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Drs.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderToStore
    {
        public OrderToStore()
        {
            this.OrderToStoreLog = new HashSet<OrderToStoreLog>();
            this.Recurrence = new HashSet<Recurrence>();
            this.StoreStatus = new HashSet<StoreStatus>();
        }
    
        public long OrderToStoreId { get; set; }
        public string LastStatus { get; set; }
        public System.DateTime LastUpdate { get; set; }
        public long ClientPhoneId { get; set; }
        public long AddressId { get; set; }
        public long ClientId { get; set; }
        public int FranchiseId { get; set; }
        public int FranchiseStoreId { get; set; }
        public long PosOrderId { get; set; }
        public string UserInsId { get; set; }
        public System.DateTime StartDatetime { get; set; }
        public Nullable<System.DateTime> EndDatetime { get; set; }
        public string OrderAtoId { get; set; }
        public string OrderMode { get; set; }
        public string OrderModeCharge { get; set; }
        public Nullable<System.DateTime> PromiseTime { get; set; }
        public int PaymentId { get; set; }
        public Nullable<bool> IsCanceled { get; set; }
        public Nullable<System.DateTime> DateTimeCanceled { get; set; }
        public int FailedStatusCounter { get; set; }
        public int PosOrderStatus { get; set; }
        public string ExtraNotes { get; set; }
        public int InputType { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual CatPayment CatPayment { get; set; }
        public virtual Client Client { get; set; }
        public virtual ClientPhone ClientPhone { get; set; }
        public virtual Franchise Franchise { get; set; }
        public virtual FranchiseStore FranchiseStore { get; set; }
        public virtual ICollection<OrderToStoreLog> OrderToStoreLog { get; set; }
        public virtual ICollection<Recurrence> Recurrence { get; set; }
        public virtual ICollection<StoreStatus> StoreStatus { get; set; }
        public virtual PosOrder PosOrder { get; set; }
    }
}
