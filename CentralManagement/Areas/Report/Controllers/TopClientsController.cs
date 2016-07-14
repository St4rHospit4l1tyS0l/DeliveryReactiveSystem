using System;
using System.Web.Mvc;
using Drs.Infrastructure.Resources;
using Drs.Model.Report;
using Drs.Repository.Log;
using Drs.Service.Report;

namespace CentralManagement.Areas.Report.Controllers
{
    public class TopClientsController : Controller
    {

        public ActionResult IndexFrequent()
        {
            return View();
        }

        public ActionResult IndexConsume()
        {
            return View();
        }

        public ActionResult SearchTopFrequentClientByRangeDates(ReportRequestModel reportRequest)
        {
            var response = new ResponseMessageModel { HasError = false };

            try
            {
                IReportService reportService = new ReportService();
                response.Data = reportService.GetTopFrequentClientByRangeDates(reportRequest.StartCalculatedDate, reportRequest.EndCalculatedDate);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                response.HasError = true;
            }

            return Json(response);
        }

        public ActionResult SearchTopConsumerClientByRangeDates(ReportRequestModel reportRequest)
        {
            var response = new ResponseMessageModel { HasError = false };

            try
            {
                IReportService reportService = new ReportService();
                response.Data = reportService.GetTopConsumerClientByRangeDates(reportRequest.StartCalculatedDate, reportRequest.EndCalculatedDate);
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