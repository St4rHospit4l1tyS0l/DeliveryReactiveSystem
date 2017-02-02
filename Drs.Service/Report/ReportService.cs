using System;
using System.Collections.Generic;
using System.Linq;
using Drs.Model.Report;
using Drs.Repository.Report;
using ReactiveUI;

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

        public IEnumerable<MonthlySaleModel> GetSalesMonthlyByYear(int year)
        {
            using (_repository)
            {
                return _repository.GetSalesMonthlyByYear(year);
            }
        }

        public IEnumerable<ClientOrderModel> GetClientOrderInfoByFranchiseAndDate(ReportRequestModel requestModelTime)
        {
            using (_repository)
            {
                var dicItems = _repository.GetClientOrderInfoByFranchiseAndDate(requestModelTime).ToDictionary(e => e.OrderToStoreId);
                var lstPosItems = _repository.GetPosOrderInfoByFranchiseAndDate(requestModelTime);

                foreach (var posOrderInfoModel in lstPosItems)
                {
                    ClientOrderModel clientOrderModel;
                    if (dicItems.TryGetValue(posOrderInfoModel.OrderToStoreId, out clientOrderModel))
                        clientOrderModel.LstPosOrder.Add(posOrderInfoModel);
                }
                
                return dicItems.Values;
            }
        }
    }
}
