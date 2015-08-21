using System.Web.Mvc;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Repository.Entities;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Shared;

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


    }
}