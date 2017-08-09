using System;
using System.Web.Mvc;
using CentralManagement.Controllers;
using Drs.Infrastructure.Resources;
using Drs.Model.Constants;
using Drs.Model.Report;
using Drs.Model.Shared;
using Drs.Repository.Account;
using Drs.Repository.Log;
using Drs.Repository.Shared;
using Drs.Repository.Store;
using Drs.Service.Report;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace CentralManagement.Areas.Report.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER + ", " + RoleConstants.STORE_MANAGER_ALL + ", " + RoleConstants.STORE_MANAGER)]
    public class ClientOrderController : BaseController
    {
        public ActionResult Index()
        {
            using (var repository = new StoreRepository())
            {
                ViewBag.LstFranchises = JsonConvert.SerializeObject(repository.GetFranchises().InsertAllOption("Todas las franquicias"));
                ViewBag.LstFranchiseStores = JsonConvert.SerializeObject(repository.GetFranchisesStores().InsertAllOption("Todas las sucursales"));
                ViewBag.IsUserAdmin = User.IsInRole(RoleConstants.MANAGER) || User.IsInRole(RoleConstants.STORE_MANAGER_ALL);
            }

            return View();
        }

        public ActionResult SearchByFranchiseAndDate(ReportRequestModel reportRequest)
        {
            var response = new ResponseMessageModel { HasError = false };

            try
            {
                if (User.IsInRole(RoleConstants.STORE_MANAGER))
                {
                    reportRequest.Id = EntityConstants.NULL_VALUE;
                    reportRequest.SecondId = EntityConstants.NULL_VALUE;
                    using (var repository = new AccountRepository())
                    {
                        reportRequest.ListStoresIds = repository.GetStoresIdsByUser(User.Identity.GetUserId());
                    }
                }

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