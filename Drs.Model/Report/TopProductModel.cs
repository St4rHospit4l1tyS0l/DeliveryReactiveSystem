namespace Drs.Model.Report
{
    public class TopProductModel
    {
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalByProduct { get; set; }
        public int FranchiseStoreId { get; set; }
        public string StoreName { get; set; }
        public string FranchiseName { get; set; }
        public string Period { get; set; }
    }
}