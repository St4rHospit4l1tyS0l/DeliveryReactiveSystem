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
            //string startDateString = "08/12/2015 00:00:00 AM";
            //string endDateString = "08/12/2015 15:09:00 PM";
            //DateTime startDate = DateTime.Parse(startDateString,
            //                          System.Globalization.CultureInfo.InvariantCulture); 
            //DateTime endDate = DateTime.Parse(endDateString,
            //                          System.Globalization.CultureInfo.InvariantCulture);
            return DbEntities.Daily_Sales_group_by_date(startDate, endDate)
                    .Where(e => e.SaleDate >= startDate && e.SaleDate <= endDate)
                    .Select(x => new DailySaleModel { 
                        SaleDate  = x.SaleDate.ToString(), 
                        SubTotal = (decimal) x.Subtotal, 
                        Tax = (decimal) x.Taxes, 
                        Total = (decimal) x.Total})
                    .ToList();    
        }

        public IEnumerable<AgentSalesModel> GetAgentSaleInfo(DateTime startDate, DateTime endDate)
        {
            return DbEntities.User_Sales_group_by_date(startDate, endDate)
                .Where(e => e.Column1 >= startDate && e.Column1 <= endDate)
                .Select(e => new AgentSalesModel
                {
                    AgentName = e.UserName,
                    SubTotal = (decimal) e.Subtotal,
                    Tax = (decimal) e.Taxes,
                    Total = (decimal) e.Total

                }
                );
        }
    }
}
