using System;
using System.Collections.Generic;
using System.Linq;

namespace Drs.Model.Store
{
    public class EmailPosOrder
    {
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public List<EmailItemsPosOrder> ItemsPosOrder { get; set; }

        public string GetInfo()
        {
            return ItemsPosOrder.Aggregate(string.Empty, (current, items) => current + string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", 
                items.Id, items.Name, items.Price.ToString("C")));
        }
    }
}