using System;
using System.Web.Mvc;
using Drs.Infrastructure.Resources;
using Drs.Model.Report;
using Drs.Repository.Log;
using Drs.Service.Report;

namespace CentralManagement.Areas.Report.Controllers
{
    public class TopProductsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchTopProductsByRangeDates(ReportRequestModel reportRequest)
        {
            var response = new ResponseMessageModel { HasError = false };

            try
            {
                IReportService reportService = new ReportService();
                response.Data = reportService.GetTopProductsByRangeDates(reportRequest.StartCalculatedDate, reportRequest.EndCalculatedDate);
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