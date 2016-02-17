using System.Collections.Generic;
using Drs.Model.Report;

namespace CentralManagement.Models
{
    public class ReportsViewModel
    {
        
    }

    public class DailySalesReportModel
    {
        public IEnumerable<DailySaleModel> Sales { get; set; }
    }
}
