using System;
using System.Web.Mvc;
using Drs.Infrastructure.Resources;
using Drs.Model.Report;
using Drs.Repository.Log;
using Drs.Service.Report;

namespace CentralManagement.Areas.Report.Controllers
{
    public class AgentSaleController : Controller
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
                response.Data = reportService.GetAgentSaleInfo(reportRequest.StartCalculatedDate, reportRequest.EndCalculatedDate);
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