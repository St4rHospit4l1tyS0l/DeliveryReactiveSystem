namespace Drs.Model.Report
{
    public class AgentSaleModel
    {
        public string UserName { get; set; }
        public string AgentName { get; set; }
        public int NumberSales { get; set; }
        public string SaleDate { get; set; }
        public decimal Total { get; set; }
    }
}
