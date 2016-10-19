using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Infrastructure.Resources;
using Drs.Model.Constants;
using Drs.Model.Store;
using Drs.Repository.Entities;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Log;
using Drs.Repository.Notification;
using Drs.Repository.Shared;
using Drs.Repository.Store;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using ResShared = CentralManagement.Resources.ResShared;

namespace CentralManagement.Areas.Store.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER + ", " + RoleConstants.STORE_MANAGER)]
    public class StoreNotificationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(JqGridFilterModel opts)
        {
            Expression<Func<ViewStoreInfo, bool>> extraFilter;
            var userId = User.Identity.GetUserId();

            if (User.IsInRole(RoleConstants.STORE_MANAGER))
                extraFilter = (e => e.IsObsolete == false && e.ManagerUserId == userId);
            else
                extraFilter = (e => e.IsObsolete == false);


            using (var repository = new GenericRepository<ViewStoreInfo>())
            {
                var result = repository.JqGridFindBy(opts, StoreInfoJson.Key, StoreInfoJson.Columns, extraFilter
                    , StoreInfoJson.DynamicToDto);
                return Json(result);
            }
        }

        public ActionResult Upsert(int storeId)
        {
            try
            {
                using (var repository = new NotificationRepository())
                {
                    ViewBag.Model = JsonConvert.SerializeObject(new
                    {
                        Items = repository.GetNotificationOfToday(storeId),
                        CatMessages = repository.GetCatMessages(),
                        StoreName = repository.GetStoreName(storeId),
                        FranchiseStoreId = storeId
                    });
                }    
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
            }
            return View();
        }

        [HttpPost]
        public ActionResult DoUpsert(StoreNotificationModel model)
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

                var userId = User.Identity.GetUserId();
                using (var repository = new NotificationRepository())
                {
                    using (var trans = repository.Db.Database.BeginTransaction())
                    {
                        var response = repository.InsertNotification(model, userId);
                        trans.Commit();
                        return Json(response);
                    }
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
        public ActionResult DoDelete(StoreNotificationModel model)
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

                using (var repository = new NotificationRepository())
                {
                    using (var trans = repository.Db.Database.BeginTransaction())
                    {
                        var response = repository.DeleteNotification(model);
                        trans.Commit();
                        return Json(response);
                    }
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
        public ActionResult Notifications(string notification, int limit)
        {
            try
            {
                using (var repository = new NotificationRepository())
                {
                    var data = repository.GetNotifications(notification, limit);
                    return Json(new ResponseMessageModel
                    {
                        HasError = false,
                        Data = data,
                    });
                }
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return Json(new ResponseMessageModel
                {
                    HasError = false,
                    Message = "Se presentó un problema al momento de consultar la información",
                });
            }
        }
    }
}