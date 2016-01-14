using System.Collections.Generic;
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
        public IEnumerable<Model.Report.DailySaleModel> GetDailySaleInfo()
        {
            using (_repository)
            {
                return _repository.GetDailySaleInfo();
            }
        }
    }
}
