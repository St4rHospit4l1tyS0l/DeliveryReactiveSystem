using System;
using System.Collections.Generic;
using Drs.Model.Report;
using Drs.Repository.Shared;

namespace Drs.Repository.Report
{
    public interface IReportRepository : IBaseOneRepository, IDisposable
    {
        IEnumerable<DailySaleModel> GetDailySaleInfo(DateTime startDate, DateTime endDate);
        IEnumerable<AgentSalesModel> GetAgentSaleInfo(DateTime startDate, DateTime endDate);
        IEnumerable<TopProductModel> GetTopProductsByRangeDates(DateTime startDate, DateTime endDate);
        IEnumerable<ClientSalesModel> GetTopFrequentClientByRangeDates(DateTime startDate, DateTime endDate);
        IEnumerable<ClientSalesModel> GetTopConsumerClientByRangeDates(DateTime startDate, DateTime endDate);
        IEnumerable<MonthSalesModel> GetMonthSalesByDays(int year, int month);
    }
}
