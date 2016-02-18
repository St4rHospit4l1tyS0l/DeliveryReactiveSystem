using System;
using System.Linq;
using System.Web.Mvc;
using CentralManagement.Models;
using Drs.Model.Constants;
using Drs.Service.Report;
using Newtonsoft.Json;

namespace CentralManagement.Areas.Report.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER + ", " + RoleConstants.STORE_MANAGER)]
    public class SaleController : Controller
    {
        //private readonly IReportService _service = new ReportService();
        // GET: Report/Sale
        public ActionResult Index()
        {

            const string startDateString = "12/05/2015 00:00:00 AM";
            const string endDateString = "12/20/2015 23:59:59 PM";
            DateTime startDate = DateTime.Parse(startDateString,
                                      System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.Parse(endDateString,
                                      System.Globalization.CultureInfo.InvariantCulture);


            ViewBag.reportTitle = " Reporte total de venta por día ";
            ViewBag.sDate = startDateString;
            ViewBag.eDate = endDateString;
            ViewBag.filterByLocation = true;
            var service = new ReportService();
            var ventas = new DailySalesReportModel();
            ventas.Sales = service.GetDailySaleInfo(startDate, endDate).ToList();
            ViewBag.LstSales = JsonConvert.SerializeObject(ventas.Sales);
            ViewBag.Sales = ventas.Sales;
            return View();
        }
    }
}