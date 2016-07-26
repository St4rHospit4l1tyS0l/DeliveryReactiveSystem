using System;
using System.Collections.Generic;
using Drs.Model.Report;
using Drs.Repository.Report;

namespace Drs.Service.Report  
{
    public class ReportService : IReportService 
    {
        private readonly IReportRepository _repository;

        public ReportService()
            : this(new ReportRepository())
        {
            
        }
        
        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<DailySaleModel> GetDailySaleInfo(DateTime startDate, DateTime endDate)
        {
            using (_repository)
            {
                return _repository.GetDailySaleInfo(startDate, endDate);
            }
        }

        
        public IEnumerable<AgentSalesModel> GetAgentSaleInfo(DateTime startDate, DateTime endDate)
        {
            using (_repository)
            {
                return _repository.GetAgentSaleInfo(startDate, endDate);
            }
        }

        public IEnumerable<TopProductModel> GetTopProductsByRangeDates(DateTime startDate, DateTime endDate)
        {
            using (_repository)
            {
                return _repository.GetTopProductsByRangeDates(startDate, endDate);
            }
        }

        public IEnumerable<ClientSalesModel> GetTopFrequentClientByRangeDates(DateTime startDate, DateTime endDate)
        {
            using (_repository)
            {
                return _repository.GetTopFrequentClientByRangeDates(startDate, endDate);
            }
        }

        public IEnumerable<ClientSalesModel> GetTopConsumerClientByRangeDates(DateTime startDate, DateTime endDate)
        {
            using (_repository)
            {
                return _repository.GetTopConsumerClientByRangeDates(startDate, endDate);
            }
        }

        public IEnumerable<MonthSalesModel> GetMonthSalesByDays(int year, int month)
        {
            using (_repository)
            {
                return _repository.GetMonthSalesByDays(year, month);
            }
        }
    }
}
