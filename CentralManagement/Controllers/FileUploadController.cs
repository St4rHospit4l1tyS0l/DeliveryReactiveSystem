using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Drs.Infrastructure.Extensions.Io;
using Drs.Model.Settings;
using Drs.Repository.Shared;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CentralManagement.Controllers
{
    public class FileUploadController : ApiController
    {
        public static string UploadFolder
        {
            get
            {
                return Path.Combine(HttpContext.Current.Server.MapPath("~"), ConfigurationManager.AppSettings["UploadFiles"]);
            }
        }

        private static string UploadTmpFolder
        {
            get
            {
                return Path.Combine(HttpContext.Current.Server.MapPath("~"), ConfigurationManager.AppSettings["TmpFiles"]);
            }
        }

        [System.Web.Mvc.Authorize]
        [Route("api/upload"), System.Web.Mvc.HttpPost]
        public async Task<HttpResponseMessage> Upload()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = GetMultipartProvider();
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                var originalFileName = GetDeserializedFileName(result.FileData.First());

                var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);

                var uploadFolder = UploadFolder;

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                var uidFileName = Guid.NewGuid();

                var destinationFile = new FileInfo(Path.Combine(uploadFolder, uidFileName.ToString()));
                
                if (destinationFile.Exists)
                    destinationFile.Delete();
               
                uploadedFileInfo.MoveTo(destinationFile.FullName);

                await result.ExecutePostProcessingAsync();

                var checkSum = destinationFile.FullName.GetChecksum();

                using (IResourceRepository repository = new ResourceRepository())
                {
                    repository.Save(originalFileName, uidFileName, uploadFolder, User.Identity.GetUserId(), SettingsData.Constants.FranchiseConst.SYNC_FILE_TYPE_LOGO, checkSum);
                    return Request.CreateResponse(HttpStatusCode.OK, new { ResourceName = uidFileName.ToString(), IsSuccess = true });
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { IsSuccess = false, Msg = ex.Message });
            }
        }

        private MultipartFormDataStreamProvider GetMultipartProvider()
        {
            if (!Directory.Exists(UploadTmpFolder))
                Directory.CreateDirectory(UploadTmpFolder);

            return new MultipartFormDataStreamProvider(UploadTmpFolder);
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);

            try
            {
                JToken.Parse(fileName);
                return JsonConvert.DeserializeObject(fileName).ToString();
            }
            catch (Exception)
            {
                try
                {
                    return new FileInfo(fileName.Replace('\"', ' ')).Name;
                }
                catch (Exception)
                {
                    return fileName;
                }
            }
        }

        private string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }
    }
}
