using System;
using System.Web.Mvc;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Infrastructure.Resources;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Model.Store;
using Drs.Repository.Account;
using Drs.Repository.Entities;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Log;
using Drs.Repository.Shared;
using Drs.Repository.Store;
using Drs.Service.Factory;
using Drs.Service.Store;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using ResShared = CentralManagement.Resources.ResShared;

namespace CentralManagement.Areas.Store.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER + ", " + RoleConstants.INSTALLER)]
    public class SettingController : Controller
    {
        // GET: Store/Setting
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


        public ActionResult Upsert(int? id)
        {
            StoreUpModel model = null;
            try
            {
                using (var repository = new StoreRepository())
                {
                    if (id.HasValue)
                    {

                        model = repository.FindModelById(id.Value);

                    }
                    else
                    {
                        model = new StoreUpModel
                        {
                            FranchiseStoreId = EntityConstants.NULL_VALUE
                        };
                    }
                    ViewBag.LstFranchises = JsonConvert.SerializeObject(repository.GetFranchises());
                    ViewBag.LastRegion = FactoryAddress.GetRegionChildByZipCode();
                    ViewBag.RegionsEnabled = JsonConvert.SerializeObject(FactoryAddress.GetAddressHierarchyOrderById());
                    ViewBag.RegionLang = JsonConvert.SerializeObject(SettingsData.Constants.AddressUpsertSetting);
                    ViewBag.ManagerStoreUsers = JsonConvert.SerializeObject(new AccountRepository(repository.Db).GetManagerStoreUsers());
                    ViewBag.Address = JsonConvert.SerializeObject(model.Address);
                    model.Address = null;
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
        public ActionResult DoUpsert(StoreUpModel model)
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

                using (var service = new StoreSettingService())
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

                using (var service = new StoreSettingService())
                {
                    service.DoObsoleteStore(id, User.Identity.GetUserId(), response);
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