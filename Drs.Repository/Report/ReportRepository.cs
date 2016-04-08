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
        public IEnumerable<DailySaleModel> GetDailySaleInfo(string startDate, string endDate)
        {
            return DbEntities.ViewDailySales
                .Select(e => new DailySaleModel
                {
                    PosOrderId = (int) e.PosOrderId,
                    Month = e.SaleDate,
                    Sales = (decimal) e.Total
                })
                .Where(e => String.Compare(e.Month, startDate, StringComparison.Ordinal) >= 0 && String.Compare(e.Month, endDate, StringComparison.Ordinal) <=0)
                .ToList();
            //return DbEntities.Daily_Sales_group_by_date(startDate, endDate)
            //        //.Where(e => e.SaleDate >= startDate && e.SaleDate <= endDate)
            //        .Select(x => x.Total != null ? new DailySaleModel { 
            //            month  = x.SaleDate, 
            //            sales = (decimal) x.Total} : null)
            //        .ToList();   
            //return null;
        }

        public IEnumerable<AgentSalesModel> GetAgentSaleInfo(DateTime startDate, DateTime endDate)
        {
            //return DbEntities.User_Sales_group_by_date(startDate, endDate)
            //    .Where(e => e.SaleDate >= startDate && e.SaleDate <= endDate)
            //    .Select(e => new AgentSalesModel
            //    {
            //        AgentName = e.UserName,
            //        SubTotal = (decimal) e.Subtotal,
            //        Tax = (decimal) e.Taxes,
            //        Total = (decimal) e.Total

            //    }
            //    );
            return null;

        }
    }
}
