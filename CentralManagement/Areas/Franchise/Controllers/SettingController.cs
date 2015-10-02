using System;
using System.Web.Mvc;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Infrastructure.Resources;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Log;
using Drs.Repository.Order;
using Drs.Repository.Shared;
using Drs.Service.Franchise;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using ResShared = CentralManagement.Resources.ResShared;

namespace CentralManagement.Areas.Franchise.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER + ", " + RoleConstants.INSTALLER)]
    public class SettingController : Controller
    {
        // GET: Franchise/Setting
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(JqGridFilterModel opts)
        {
            using (var repository = new GenericRepository<Drs.Repository.Entities.Franchise>())
            {
                var result = repository.JqGridFindBy(opts, FranchiseInfoJson.Key, FranchiseInfoJson.Columns, (e => e.IsObsolete == false)
                    , FranchiseInfoJson.DynamicToDto);
                return Json(result);
            }
        }


        public ActionResult Upsert(int? id)
        {
            FranchiseUpModel model = null;
            try
            {
                using (var repository = new FranchiseRepository())
                {
                    if (id.HasValue)
                    {
                        model = repository.FindModelById(id.Value);
                    }
                    else
                    {
                        model = new FranchiseUpModel
                        {
                            FranchiseId = EntityConstants.NULL_VALUE
                        };
                    }

                    ViewBag.Model = JsonConvert.SerializeObject(model);
                }
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DoUpsert(FranchiseUpModel model)
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

                using (var service = new FranchiseSettingService())
                {
                    var response = service.ValidateModel(model);

                    if (response.HasError)
                    {
                        response.Title = ResShared.TITLE_REGISTER_FAILED;
                        return Json(response);
                    }

                    model.UserInsUpId = User.Identity.GetUserId();
                    response = service.Save(model);

                    if (response.HasError)
                        response.Title = ResShared.TITLE_REGISTER_FAILED;
                    
                    return Json(response);
                }
            }
            catch (Exception ex)
            {

                SharedLogger.LogError(ex);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Title = ResShared.TITLE_REGISTER_FAILED,
                    Message = ResShared.ERROR_UNKOWN
                });
            }
        }

        [HttpPost]
        public ActionResult DoObsolete(int id)
        {
            try
            {
                var response = new ResponseMessageModel();

                using (var service = new FranchiseSettingService())
                {
                    service.DoObsolete(id, User.Identity.GetUserId(), response);
                }

                if (response.HasError)
                    response.Title = ResShared.TITLE_OBSOLETE_FAILED;

                return Json(response);
                //}
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Title = ResShared.TITLE_OBSOLETE_FAILED,
                    Message = ResShared.ERROR_UNKOWN
                });
            }
        }

    }
}