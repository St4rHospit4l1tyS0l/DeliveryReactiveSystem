using System.Web.Mvc;
using Drs.Model.Constants;

namespace CentralManagement.Areas.Report.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER + ", " + RoleConstants.STORE_MANAGER)]
    public class SaleController : Controller
    {
        // GET: Report/Sale
        public ActionResult Index()
        {
            return View();
        }
    }
}