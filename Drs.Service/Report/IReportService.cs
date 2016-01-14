using System.Collections.Generic;
using Drs.Model.Report;

namespace Drs.Service.Report
{
    public interface IReportService
    {
        IEnumerable<DailySaleModel> GetDailySaleInfo();
    }
}
