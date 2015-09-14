using System;
using System.Web.Mvc;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Infrastructure.Resources;
using Drs.Model.Constants;
using Drs.Model.Store;
using Drs.Repository.Entities;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Log;
using Drs.Repository.Shared;
using Drs.Repository.Store;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using ResShared = CentralManagement.Resources.ResShared;

namespace CentralManagement.Areas.Store.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER + ", " + RoleConstants.INSTALLER + ", " + RoleConstants.STORE_MANAGER)]
    public class OfflineController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(JqGridFilterModel opts)
        {
            using (var repository = new GenericRepository<ViewStoreInfo>())
            {
                var result = repository.JqGridFindBy(opts, StoreInfoJson.Key, StoreInfoJson.Columns, (e => e.IsObsolete == false)
                    , StoreInfoJson.DynamicToDto);
                return Json(result);
            }
        }

        public ActionResult OffLineList(JqGridFilterModel opts, int id)
        {
            StoreOfflineInfoDto.UtcTime = DateTime.UtcNow;
            using (var repository = new GenericRepository<FranchiseStoreOffLine>())
            {
                var result = repository.JqGridFindBy(opts, StoreOfflineInfoJson.Key, StoreOfflineInfoJson.Columns, (e => e.FranchiseStoreId == id && e.IsObsolete == false)
                    , StoreOfflineInfoJson.DynamicToDto);
                return Json(result);
            }
        }


        public ActionResult Upsert(int storeId, int? id)
        {
            StoreOfflineModel model = null;
            try
            {
                using (var repository = new StoreRepository())
                {
                    model = repository.FindStoreOfflineModelById(storeId, id);

                    ViewBag.StartDateTime = id.HasValue ? model.UtcStartDateTimeSaved.ToString(SharedConstants.DATE_TIME_FORMAT) : DateTime.UtcNow.ToString(SharedConstants.DATE_TIME_FORMAT);
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
        public ActionResult DoUpsert(StoreOfflineModel model)
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

                var response = new ResponseMessageModel();
                using (var repository = new StoreRepository())
                {
                    if (model.FranchiseStoreOffLineId > EntityConstants.NO_VALUE)
                    {
                        repository.UpdateOffline(model, response, User.Identity.GetUserId());
                    }
                    else
                    {
                        repository.AddOffline(model, User.Identity.GetUserId());
                    }
                }

                response.HasError = false;
                return Json(response);
            }
            catch (Exception ex)
            {

                SharedLogger.LogError(ex);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Title = Resources.ResShared.TITLE_REGISTER_FAILED,
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

                using (var service = new StoreRepository())
                {
                    service.DoObsoleteStoreOffline(id, User.Identity.GetUserId(), response);
                }

                if (response.HasError)
                    response.Title = ResShared.TITLE_OBSOLETE_FAILED;

                return Json(response);
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