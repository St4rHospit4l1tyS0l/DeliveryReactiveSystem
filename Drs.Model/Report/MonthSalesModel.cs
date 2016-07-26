namespace Drs.Model.Report
{
    public class MonthSalesModel
    {
        public int Key { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public decimal? TotalPerDay { get; set; }
    }
}