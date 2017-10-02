using System;
using System.Collections.Generic;

namespace Drs.Model.Store
{
    public class EmailPosOrder
    {
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public List<EmailItemsPosOrder> ItemsPosOrder { get; set; }
    }
}