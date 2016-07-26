using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using Drs.Infrastructure.Extensions;
using Drs.Model.Report;
using Drs.Model.Settings;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

namespace Drs.Repository.Report
{
    public class ReportRepository : BaseOneRepository, IReportRepository
    {
        public ReportRepository()
        {

        }

        public ReportRepository(CallCenterEntities dbEntities)
            : base(dbEntities)
        {

        }
        public IEnumerable<DailySaleModel> GetDailySaleInfo(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.FloorDate();
            endDate = endDate.CeilDate();

            return Db.ViewDailySales.Where(e => e.OrderDate >= startDate && e.OrderDate < endDate)
                .OrderBy(e => e.LastStatus).ThenBy(e => e.OrderDate).Select(e => new DailySaleModel
                {
                    OrderDate = e.OrderDate,
                    LastStatus = e.LastStatus,
                    Franchise = e.Franchise,
                    FranchiseStoreId = e.FranchiseStoreId,
                    FranchiseStore = e.FranchiseStore,
                    SalesPerDay = e.SalesPerDay,
                    TotalPerDay = e.TotalPerDay
                }).ToList();

        }

        public IEnumerable<AgentSalesModel> GetAgentSaleInfo(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.FloorDate();
            endDate = endDate.CeilDate();

            return Db.ViewAgentSales.Where(e => e.OrderDate >= startDate && e.OrderDate < endDate)
                .OrderBy(e => e.FullName).ThenBy(e => e.LastStatus).ThenBy(e => e.OrderDate).Select(e => new AgentSalesModel
                {
                    FullName = e.FullName,
                    OrderDate = e.OrderDate,
                    LastStatus = e.LastStatus,
                    Franchise = e.Franchise,
                    FranchiseStoreId = e.FranchiseStoreId,
                    FranchiseStore = e.FranchiseStore,
                    SalesPerDay = e.SalesPerDay,
                    TotalPerDay = e.TotalPerDay
                }).ToList();
        }

        public IEnumerable<TopProductModel> GetTopProductsByRangeDates(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.FloorDate();
            endDate = endDate.CeilDate();

            var period = String.Format("Del {0} al {1}",
                startDate.ToString(Model.Constants.SharedConstants.REPORT_DATE_FORMAT)
                , endDate.ToString(Model.Constants.SharedConstants.REPORT_DATE_FORMAT));

            return Db.ufnGetTopProductsByRangeDates(startDate, endDate).Select(e => new TopProductModel
            {
                Period = period,
                ProductName = e.ProductName,
                Quantity = e.Quantity,
                TotalByProduct = e.TotalByProduct,
                FranchiseStoreId = e.FranchiseStoreId,
                StoreName = e.StoreName,
                FranchiseName = e.FranchiseName
            }).ToList();
        }

        public IEnumerable<ClientSalesModel> GetTopFrequentClientByRangeDates(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.FloorDate();
            endDate = endDate.CeilDate();

            var period = String.Format("Del {0} al {1}",
                startDate.ToString(Model.Constants.SharedConstants.REPORT_DATE_FORMAT)
                , endDate.ToString(Model.Constants.SharedConstants.REPORT_DATE_FORMAT));

            return Db.ufnGetFrequentClientByRangeDates(startDate, endDate).Select(e => new ClientSalesModel
            {
                Period = period,
                FullName = e.FullName,
                BirthDate = e.BirthDate,
                Email = e.Email,
                CompanyName = e.CompanyName,
                LoyaltyCode = e.LoyaltyCode,
                TotalByProduct = e.TotalByProduct,
                StoreName = e.StoreName,
                FranchiseName = e.FranchiseName
            }).ToList();
        }

        public IEnumerable<ClientSalesModel> GetTopConsumerClientByRangeDates(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.FloorDate();
            endDate = endDate.CeilDate();

            var period = String.Format("Del {0} al {1}",
                startDate.ToString(Model.Constants.SharedConstants.REPORT_DATE_FORMAT)
                , endDate.ToString(Model.Constants.SharedConstants.REPORT_DATE_FORMAT));

            return Db.ufnGetConsumerClientByRangeDates(startDate, endDate).Select(e => new ClientSalesModel
            {
                Period = period,
                FullName = e.FullName,
                BirthDate = e.BirthDate,
                Email = e.Email,
                CompanyName = e.CompanyName,
                LoyaltyCode = e.LoyaltyCode,
                TotalByConsume = e.TotalByConsume,
                StoreName = e.StoreName,
                FranchiseName = e.FranchiseName
            }).ToList();
        }

        public IEnumerable<MonthSalesModel> GetMonthSalesByDays(int year, int month)
        {
            return DbEntities.ViewMonthSalesByDay.Where(e => e.OrderYear == year && e.OrderMonth == month)
                .Select(e => new MonthSalesModel
                {
                    Key = e.Key,
                    Year = e.OrderYear,
                    Month = e.OrderMonth,
                    Day = e.OrderDay,
                    TotalPerDay = e.TotalPerDay
                }).ToList();
        }
    }
}
