using System;
using System.Web.Mvc;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Infrastructure.Resources;
using Drs.Model.Store;
using Drs.Repository.Entities;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Log;
using Drs.Repository.Shared;
using Drs.Repository.Store;
using Newtonsoft.Json;
using ResShared = CentralManagement.Resources.ResShared;

namespace CentralManagement.Areas.Store.Controllers
{
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
                    //ViewBag.DicLanguage = JsonConvert.SerializeObject();
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

                //using (var repository = new GenericRepository<Location>())
                //{
                //    if (model.Id == EntityConstants.NULL_VALUE)
                //    {
                //        var inModel = model.ToEntity();
                //        inModel.IsObsolete = false;
                //        repository.Add(inModel);
                //    }
                //    else
                //    {
                //        var oldModel = repository.FindById(model.Id);

                //        var inModel = model.UpdateModel(oldModel);

                //        repository.Update(inModel);


                //    }

                //    return Json(new ResponseMessageModel
                //    {
                //        HasError = false,
                //        Title = ResShared.TITLE_REGISTER_SUCCESS,
                //        Message = ResShared.INFO_REGISTER_SAVED
                //    });
                //}
                return null;

            }
            catch (Exception ex)
            {

                SharedLogger.LogError(ex);
                //SharedLogger.LogError(ex, model.ClientId, model.FirstName, model.LastName);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Title = ResShared.TITLE_REGISTER_FAILED,
                    Message = ResShared.ERROR_UNKOWN
                });
            }
        }


    }
}