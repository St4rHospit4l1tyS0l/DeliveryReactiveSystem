namespace Drs.Model.Report
{
    public class DailySaleModel
    {
        public string Date { get; set; }
        public string SaleDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

    }
}
