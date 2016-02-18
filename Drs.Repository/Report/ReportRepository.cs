using System;
using System.Collections.Generic;
using System.Linq;
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
            //return DbEntities.Daily_Sales_group_by_date(startDate, endDate)
            //        //.Where(e => e.SaleDate >= startDate && e.SaleDate <= endDate)
            //        .Select(x => x.Total != null ? new DailySaleModel { 
            //            month  = x.SaleDate, 
            //            sales = (decimal) x.Total} : null)
            //        .ToList();   
            return null;
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
