using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Infrastructure.Model;
using Drs.Infrastructure.Resources;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Repository.Account;
using Drs.Repository.Log;
using Drs.Repository.Order;
using Drs.Service.Account;
using Drs.Service.Franchise;
using ResShared = CentralManagement.Resources.ResShared;

namespace CentralManagement.Areas.Terminal.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER + ", " + RoleConstants.INSTALLER)]
    public class SettingController : Controller
    {
        // GET: Terminal/Setting
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult List(JqGridFilterModel opts)
        {
            var devices = new AccountService().GetLstClients();

            if(devices.LstClients == null)
                devices.LstClients = new List<ConnectionFullModel>();

            var result = new
            {
                page = 1,
                records = devices.LstClients.Count,
                total = 1,
                rows = devices.LstClients.Select(e => new
                {
                    id = e.DeviceId,
                    cell = new
                    {
                        id = e.DeviceId,
                        Name = e.DeviceName,
                        e.StartDateTx,
                        e.EndDateTx,
                        e.Code
                    }
                })
            };

            return Json(result);
        }

        public ActionResult Upsert(int id)
        {
            try
            {
                var jSer = new JavaScriptSerializer();
                using (var repository = new FranchiseRepository())
                {
                    IAccountService accountService = new AccountService(new AccountRepository(repository.Db));
                    ViewBag.Model = jSer.Serialize(new AccountService(new AccountRepository(repository.Db)).GetTerminalInfo(id));
                    ViewBag.LstFranchises = jSer.Serialize(new FranchiseService(repository).LstFranchise());
                    ViewBag.LstTerminalFranchise = jSer.Serialize(accountService.GetLstTerminalFranchise(id));
                }
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
            }
            return View(String.Empty);
        }

        public ActionResult DoUpsert(TerminaFranchiseModel model)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return Json(new ResponseMessageModel
                    {
                        HasError = true,
                        Title = ResShared.TITLE_REGISTER_FAILED,
                        Message = ResShared.ERROR_INVALID_MODEL
                    });
                }

                return Json(new ResponseMessageModel
                {
                    HasError = false,
                    Data = new AccountService().UpsertTerminalFranchise(model)
                });
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Message = "Error al momento de guardar la información: " + ex.Message,
                    Title = ResShared.TITLE_REGISTER_FAILED

                });
            }
        }

    }
}