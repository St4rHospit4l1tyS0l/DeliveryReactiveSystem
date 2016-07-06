namespace Drs.Model.Report
{
    public class DailySaleModel 
    {

        public string LastStatus { get; set; }

        public System.DateTime? OrderDate { get; set; }

        public decimal? TotalPerDay { get; set; }

        public int FranchiseStoreId { get; set; }

        public string FranchiseStore { get; set; }

        public string Franchise { get; set; }
    }
}
