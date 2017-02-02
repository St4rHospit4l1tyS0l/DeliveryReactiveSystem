using System;
using System.Collections.Generic;
using System.Linq;
using Drs.Infrastructure.Extensions;
using Drs.Model.Report;
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
                TotalByConsume = e.TotalByConsume,
                CompanyName = e.CompanyName,
                LoyaltyCode = e.LoyaltyCode,
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
                TotalByProduct = e.TotalByProduct,
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

        public IEnumerable<MonthlySaleModel> GetSalesMonthlyByYear(int year)
        {
            return DbEntities.ViewMonthlySales.Where(e => e.OrderYear == year)
                .Select(e => new MonthlySaleModel
                {
                    Key = e.Key,
                    Year = e.OrderYear,
                    Month =  e.OrderMonth,
                    Franchise = e.Franchise,
                    FranchiseStore = e.FranchiseStore,
                    FranchiseStoreId = e.FranchiseStoreId,
                    SalesPerMonth = e.SalesPerMonth,
                    TotalPerMonth = e.TotalPerMonth
                }).ToList();
        }

        public IList<ClientOrderModel> GetClientOrderInfoByFranchiseAndDate(ReportRequestModel requestModelTime)
        {
            var query = DbEntities.ViewClientOrderInfoByFranchiseStore.AsQueryable();

            if (requestModelTime.Id != EntityConstants.NULL_VALUE)
                query = query.Where(e => e.FranchiseId == requestModelTime.Id);

            if (requestModelTime.SecondId != EntityConstants.NULL_VALUE)
                query = query.Where(e => e.FranchiseStoreId == requestModelTime.SecondId);

            var startDate = requestModelTime.StartCalculatedDate;
            var endDate = requestModelTime.EndCalculatedDate.AddDays(1);

            query = query.Where(e => e.FirstDatetime >= startDate && e.FirstDatetime < endDate);


            return query.Select(e => new ClientOrderModel
            {
                ClientId = e.ClientId,
                FirstDatetime = e.FirstDatetime,
                FirstName = e.FirstName,
                FranchiseName = e.FranchiseName,
                FranchiseStoreName = e.FranchiseStoreName,
                LastName = e.LastName,
                LastStatus = e.LastStatus,
                OrderAtoId = e.OrderAtoId,
                OrderToStoreId = e.OrderToStoreId,
                Phone = e.Phone,
                Total = e.Total
            }).ToList();
        }


        public IList<PosOrderInfoModel> GetPosOrderInfoByFranchiseAndDate(ReportRequestModel requestModelTime)
        {
            var query = DbEntities.ViewPosOrderInfo.AsQueryable();

            if (requestModelTime.Id != EntityConstants.NULL_VALUE)
                query = query.Where(e => e.FranchiseId == requestModelTime.Id);

            if (requestModelTime.SecondId != EntityConstants.NULL_VALUE)
                query = query.Where(e => e.FranchiseStoreId == requestModelTime.SecondId);

            var startDate = requestModelTime.StartCalculatedDate;
            var endDate = requestModelTime.EndCalculatedDate.AddDays(1);

            query = query.Where(e => e.FirstDatetime >= startDate && e.FirstDatetime < endDate);

            return query.Select(e => new PosOrderInfoModel
            {
                ItemId = e.ItemId,
                Name = e.Name,
                OrderToStoreId = e.OrderToStoreId,
                Price = e.Price
            }).ToList();
        }
    }
}
