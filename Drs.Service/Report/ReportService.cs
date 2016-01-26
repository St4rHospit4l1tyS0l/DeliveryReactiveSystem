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
    }
}
