using System.CodeDom;
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
        public IEnumerable<DailySaleModel> GetDailySaleInfo()
        {
                return DbEntities.PosOrder.Join(
                    DbEntities.PosOrderItem, 
                    pos => pos.PosOrderId,
                    item => item.PosOrderId,
                    (pos, posI) =>
                        new
                        {
                            Date = pos.OrderDatetime,
                            Subtotal = pos.Subtotal,
                            Tax = pos.Taxes,
                            Total = pos.Total
                        }
                    ).Select(e => new DailySaleModel { SaleDate = e.Date, SubTotal = e.Subtotal, Tax = e.Tax, Total = e.Total}).ToList();

        }
    }
}
