using System;
using System.Linq;
using System.Web.Mvc;
using Drs.Infrastructure.Resources;
using Drs.Repository.Entities;
using Drs.Repository.Log;
using Drs.Service.Factory;

namespace CentralManagement.Controllers
{
    [Authorize]
    public class SharedController : Controller
    {
        // GET: Shared
        public ActionResult Index()
        {
            return View();
        }

        // GET: Shared
        public ActionResult ZipCodes(string code)
        {
            try
            {
                using (var repository = new CallCenterEntities())
                {
                    var data = FactoryAddress.GetQueryToExecByZipCode(repository, code).ToList();
                    return Json(new ResponseMessageModel
                    {
                        HasError = false,
                        Data = data,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return Json(new ResponseMessageModel
                {
                    HasError = false,
                    Message = "Se presentó un problema al momento de consultar la información",
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}