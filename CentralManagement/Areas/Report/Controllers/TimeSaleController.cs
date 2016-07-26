using System;
using System.Diagnostics;
using System.Web.Mvc;
using Drs.Infrastructure.Resources;
using Drs.Model.Report;
using Drs.Repository.Log;
using Drs.Service.Report;

namespace CentralManagement.Areas.Report.Controllers
{
    public class TimeSaleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchDaysByRange(ReportRequestModel reportRequest)
        {
            var response = new ResponseMessageModel{HasError = false};

            try
            {
                IReportService reportService = new ReportService();
                response.Data = reportService.GetDailySaleInfo(reportRequest.StartCalculatedDate, reportRequest.EndCalculatedDate);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                response.HasError = true;
            }
            return Json(response);
        }

        public ActionResult IndexMonthSalesByDay()
        {
            return View();
        }
        public ActionResult SearchMonthSalesByDays(ReportRequestMonthModel reportRequest)
        {
            var response = new ResponseMessageModel { HasError = false };

            try
            {
                IReportService reportService = new ReportService();
                response.Data = reportService.GetMonthSalesByDays(reportRequest.Year, reportRequest.Month);
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