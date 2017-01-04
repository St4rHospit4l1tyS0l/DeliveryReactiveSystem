using System;
using Drs.Infrastructure.Extensions;

namespace Drs.Model.Report
{
    public class MonthlySaleModel
    {
        public int Key { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string YearMonth {
            get
            {
                return string.Format("{0} / {1}", Year, Month.GetMonthName());
            }
        }

        public decimal? TotalPerMonth { get; set; }

        public int FranchiseStoreId { get; set; }

        public string FranchiseStore { get; set; }

        public string Franchise { get; set; }

        public int? SalesPerMonth { get; set; }
    }
}