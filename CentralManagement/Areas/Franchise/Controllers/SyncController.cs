using System;
using System.Threading;
using System.Web.Mvc;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Infrastructure.Resources;
using Drs.Model.Constants;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Log;
using Drs.Repository.Shared;
using Drs.Service.Franchise;
using Microsoft.AspNet.Identity;
using ResShared = CentralManagement.Resources.ResShared;

namespace CentralManagement.Areas.Franchise.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER + ", " + RoleConstants.INSTALLER)]
    public class SyncController : Controller
    {
        // GET: Franchise/Sync
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


        public ActionResult VersionList(JqGridFilterModel opts, int id)
        {
            using (var repository = new GenericRepository<Drs.Repository.Entities.FranchiseDataVersion>())
            {
                var result = repository.JqGridFindBy(opts, FranchiseDataVersionInfoJson.Key, FranchiseDataVersionInfoJson.Columns, (e => e.IsObsolete == false && e.FranchiseId == id)
                    , FranchiseDataVersionInfoJson.DynamicToDto);
                return Json(result);
            }
        }


        [HttpPost]
        public ActionResult DoVersion(int franchiseId)
        {

            try
            {
                using (var service = new FranchiseSettingService())
                {
                    var response = new ResponseMessageModel();
                    service.CreateNewVersion(franchiseId, response, User.Identity.GetUserId());
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
                    service.DoObsoleteVersion(id, User.Identity.GetUserId(), response);
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