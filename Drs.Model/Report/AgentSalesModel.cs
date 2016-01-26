using System;

namespace Drs.Model.Report
{
    public class AgentSalesModel
    {
        public string AgentName { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
