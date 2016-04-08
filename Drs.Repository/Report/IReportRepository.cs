using System;
using System.Collections.Generic;
using Drs.Model.Report;
using Drs.Repository.Shared;

namespace Drs.Repository.Report
{
    public interface IReportRepository : IBaseOneRepository, IDisposable
    {
        IEnumerable<DailySaleModel> GetDailySaleInfo(string startDate, string endDate);
        IEnumerable<AgentSalesModel> GetAgentSaleInfo(DateTime startDate, DateTime endDate);
    }
}
