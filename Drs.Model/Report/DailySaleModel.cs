namespace Drs.Model.Report
{
    public class DailySaleModel 
    {

        public string LastStatus { get; set; }

        public System.DateTime? OrderDate { get; set; }
        
        public string OrderDateTx {
            get
            {
                return OrderDate == null ? "ND" : OrderDate.Value.ToString(Constants.SharedConstants.DATE_FORMAT_REPORT);
            }
        }

        public decimal? TotalPerDay { get; set; }

        public int FranchiseStoreId { get; set; }

        public string FranchiseStore { get; set; }

        public string Franchise { get; set; }

        public int? SalesPerDay { get; set; }
    }
}
