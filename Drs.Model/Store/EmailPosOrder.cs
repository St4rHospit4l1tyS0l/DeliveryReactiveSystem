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
            string result = string.Empty;
            foreach (EmailItemsPosOrder order in ItemsPosOrder)
                result = result + string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", order.Id, order.Name, order.Price.ToString("C"));
            result = result + string.Format("<tr><td>&nbsp;</td><td style\"color: #1689CE;\">Total</td><td>{0}</td></tr>", Total.ToString("C"));
            return result;
        }
    }
}