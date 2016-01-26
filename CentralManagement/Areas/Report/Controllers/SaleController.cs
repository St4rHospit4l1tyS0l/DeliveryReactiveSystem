using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Drs.Model.Constants;
using Drs.Model.Report;
using Drs.Service.Report;

namespace CentralManagement.Areas.Report.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER + ", " + RoleConstants.STORE_MANAGER)]
    public class SaleController : Controller
    {
        private readonly IReportService _service = new ReportService();
        // GET: Report/Sale
        public ActionResult Index()
        {
            List<DailySaleModel> lstDailySales = new List<DailySaleModel>();
            string startDateString = "07/12/2015 00:00:00 AM";
            string endDateString = "09/12/2015 15:09:00 PM";
            DateTime startDate = DateTime.Parse(startDateString,
                                      System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.Parse(endDateString,
                                      System.Globalization.CultureInfo.InvariantCulture);

            lstDailySales = _service.GetDailySaleInfo(startDate, endDate).ToList();
            ViewBag.LstSales = lstDailySales;
            
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //ViewData["JsonSalesData"] = serializer.Serialize(salesData); 
            
            return View();
        }
    }
}