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
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                throw;
            }
            
        }

        [HttpPost]
        public ActionResult ZipCodes(string code)
        {
            try
            {
                using (var repository = new CallCenterEntities())
                {
                    var data = FactoryAddress.GetQueryToExecByZipCode(repository, code, true).ToList();
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