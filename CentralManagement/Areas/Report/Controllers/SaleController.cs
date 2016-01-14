using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CentralManagement.Models;
using Drs.Model.Constants;
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
            ViewBag.LstSales = _service.GetDailySaleInfo();
            
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //ViewData["JsonSalesData"] = serializer.Serialize(salesData); 
            
            return View();
        }
    }
}