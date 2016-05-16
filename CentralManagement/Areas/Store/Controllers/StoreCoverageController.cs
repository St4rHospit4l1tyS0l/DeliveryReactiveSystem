using System;
using System.Web.Mvc;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Repository.Entities;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Log;
using Drs.Repository.Order;
using Drs.Repository.Shared;
using Newtonsoft.Json;

namespace CentralManagement.Areas.Store.Controllers
{
    public class StoreCoverageController : Controller
    {
        // GET: Store/Coverage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(JqGridFilterModel opts)
        {
            using (var repository = new GenericRepository<ViewStoreCoverage>())
            {
                var result = repository.JqGridFindBy(opts, ViewStoreCoverageJson.Key, ViewStoreCoverageJson.Columns, (e => e.IsObsolete == false)
                    , ViewStoreCoverageJson.DynamicToDto);
                return Json(result);
            }
        }

        public ActionResult Assign(int id)
        {
            try
            {
                using (var repository = new FranchiseRepository())
                {
                    ViewBag.Model = JsonConvert.SerializeObject(new
                    {
                        Franchise = repository.GetFranchiseMapInfoById(id),
                        LstStoresByFranchise = repository.GetListStoresByFranchiseId(id),
                    });
                        
                }

            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                ViewBag.FranchiseName = "No encontrada";
            }

            return View();
        }
    }
}