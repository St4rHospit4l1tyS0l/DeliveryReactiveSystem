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
    
    public partial class PosOrderItem
    {
        public long PosOrderItemId { get; set; }
        public long PosOrderId { get; set; }
        public long CheckItemId { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int LevelItem { get; set; }
        public Nullable<long> ParentId { get; set; }
        public int ModCode { get; set; }
        public int Origin { get; set; }
    
        public virtual PosOrder PosOrder { get; set; }
    }
}
