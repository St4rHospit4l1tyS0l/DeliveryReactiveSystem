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
    
    public partial class ViewMonthSalesByDay
    {
        public int Key { get; set; }
        public Nullable<int> OrderYear { get; set; }
        public Nullable<int> OrderMonth { get; set; }
        public Nullable<int> OrderDay { get; set; }
        public Nullable<decimal> TotalPerDay { get; set; }
    }
}