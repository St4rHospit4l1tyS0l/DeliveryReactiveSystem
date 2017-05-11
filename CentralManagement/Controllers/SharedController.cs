using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drs.Infrastructure.Resources;
using Drs.Repository.Entities;
using Drs.Repository.Log;
using Drs.Repository.Shared;
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

    
     
        public ActionResult Resource(Guid? resource)
        {
            if(!resource.HasValue)
                return null;

            using (var repository = new ResourceRepository())
            {
                var originalFileName = repository.GetFileNameByStoreName(resource.Value);
                return String.IsNullOrEmpty(originalFileName) ? null : File(Path.Combine(FileUploadController.UploadFolder, resource.ToString()), MimeMapping.GetMimeMapping(originalFileName));
            }
        }


        public ActionResult Download(Guid resource)
        {
            using (var repository = new ResourceRepository())
            {
                var originalFileName = repository.GetFileNameByStoreName(resource);

                if (String.IsNullOrEmpty(originalFileName))
                    return null;

                Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + originalFileName + "\"");
                return File(Path.Combine(FileUploadController.UploadFolder, resource.ToString()), "application/force-download;");
            }
        }
    }
}