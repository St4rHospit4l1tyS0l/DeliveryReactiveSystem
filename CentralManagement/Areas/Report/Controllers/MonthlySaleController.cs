using System;
using System.Web.Mvc;
using Drs.Infrastructure.Resources;
using Drs.Model.Constants;
using Drs.Model.Report;
using Drs.Repository.Log;
using Drs.Service.Report;

namespace CentralManagement.Areas.Report.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER)]
    public class MonthlySaleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchMonthlyByYear(int year)
        {
            var response = new ResponseMessageModel{HasError = false};

            try
            {
                IReportService reportService = new ReportService();
                response.Data = reportService.GetSalesMonthlyByYear(year);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                response.HasError = true;
            }
            return Json(response);
        }
    }
}