using System;
using System.Collections.Generic;
using Drs.Model.Report;

namespace Drs.Service.Report
{
    public interface IReportService
    {
        IEnumerable<DailySaleModel> GetDailySaleInfo(DateTime startDate, DateTime endDate);
        IEnumerable<AgentSalesModel> GetAgentSaleInfo(DateTime startCalculatedDate, DateTime endCalculatedDate);
        IEnumerable<TopProductModel> GetTopProductsByRangeDates(DateTime startCalculatedDate, DateTime endCalculatedDate);
        IEnumerable<ClientSalesModel> GetTopFrequentClientByRangeDates(DateTime startCalculatedDate, DateTime endCalculatedDate);
        IEnumerable<ClientSalesModel> GetTopConsumerClientByRangeDates(DateTime startCalculatedDate, DateTime endCalculatedDate);
        IEnumerable<MonthSalesModel> GetMonthSalesByDays(int year, int month);
        IEnumerable<MonthlySaleModel> GetSalesMonthlyByYear(int year);
        IEnumerable<ClientOrderModel> GetClientOrderInfoByFranchiseAndDate(ReportRequestModel requestModel);
    }
}
