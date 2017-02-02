using System;
using System.Web.Mvc;
using CentralManagement.Controllers;
using Drs.Infrastructure.Attributes;
using Drs.Infrastructure.Resources;
using Drs.Model.Constants;
using Drs.Model.Report;
using Drs.Model.Shared;
using Drs.Repository.Log;
using Drs.Repository.Store;
using Drs.Service.Report;
using Newtonsoft.Json;

namespace CentralManagement.Areas.Report.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER)]
    public class ClientOrderController : BaseController
    {
        public ActionResult Index()
        {
            using (var repository = new StoreRepository())
            {
                ViewBag.LstFranchises = JsonConvert.SerializeObject(repository.GetFranchises().InsertAllOption("Todas las franquicias"));
                ViewBag.LstFranchiseStores = JsonConvert.SerializeObject(repository.GetFranchisesStores().InsertAllOption("Todas las sucursales"));
            }

            return View();
        }

        public ActionResult SearchByFranchiseAndDate(ReportRequestModel reportRequest)
        {
            var response = new ResponseMessageModel{HasError = false};

            try
            {
                IReportService reportService = new ReportService();
                response.Data = reportService.GetClientOrderInfoByFranchiseAndDate(reportRequest);
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