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
            :base (dbEntities)
        {
            
        }
        public IEnumerable<DailySaleModel> GetDailySaleInfo(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.FloorDate();
            endDate = endDate.CeilDate();

            return Db.ViewDailySales.Where(e => e.OrderDate >= startDate && e.OrderDate < endDate)
                .OrderBy(e => e.OrderDate).ThenBy(e => e.LastStatus).Select(e => new DailySaleModel
                {
                    LastStatus = e.LastStatus,
                    OrderDate = e.OrderDate,
                    TotalPerDay = e.TotalPerDay,
                    FranchiseStoreId = e.FranchiseStoreId,
                    FranchiseStore = e.FranchiseStore,
                    Franchise = e.Franchise
                }).ToList();

        }

    }
}
