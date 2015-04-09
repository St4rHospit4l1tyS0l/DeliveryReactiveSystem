using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Drs.Infrastructure.Resources;
using Drs.Model.Account;
using Drs.Model.Constants;
using Drs.Repository.Account;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Log;
using Drs.Service.Account;
using ResShared = CentralManagement.Resources.ResShared;

namespace CentralManagement.Areas.Management.Controllers
{

    [Authorize(Roles = RoleConstants.MANAGER)]

    public class LicenseController : Controller
    {
        // GET: Default
        private readonly IAccountService _service = new AccountService();

        public ActionResult Index()
        {
            var devices = _service.GetLstDevices();
            var jsSer = new JavaScriptSerializer();
            ViewBag.LstClients = jsSer.Serialize(devices.LstClients);
            ViewBag.LstServers = jsSer.Serialize(devices.LstServers);
            return View();
        }

        [HttpPost]
        public ActionResult DoSelectServer(int id, bool enable)
        {
            try
            {
                if (_service.DoSelectServer(id, enable) == false)
                {
                    return Json(new ResponseMessageModel
                    {
                        HasError = true,
                        Message = "Equipo servidor no registrado"
                    });
                }


                return Json(new ResponseMessageModel
                {
                    HasError = false,
                    Title = ResShared.TITLE_REGISTER_SUCCESS
                });
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id, enable);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Title = ResShared.TITLE_REGISTER_FAILED,
                    Message = ResShared.ERROR_UNKOWN
                });
            }
        }


        [HttpPost]
        public ActionResult DoSelectClient(int id, bool enable)
        {
            try
            {
                if (_service.DoSelectClient(id, enable) == false)
                {
                    return Json(new ResponseMessageModel
                    {
                        HasError = true,
                        Message = "Equipo cliente no registrado"
                    });
                }


                return Json(new ResponseMessageModel
                {
                    HasError = false,
                    Title = ResShared.TITLE_REGISTER_SUCCESS
                });
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id, enable);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Title = ResShared.TITLE_REGISTER_FAILED,
                    Message = ResShared.ERROR_UNKOWN
                });
            }
        }

        [HttpPost]
        public ActionResult Upsert(string id)
        {
            ViewUserInfoModel model = null;

            try
            {
                using (var rolRepository = new RoleRepository())
                {
                    ViewBag.LstRoles = new JavaScriptSerializer().Serialize(rolRepository.FindAll());
                }

                if (String.IsNullOrEmpty(id) == false)
                {
                    using (var repository = new UserRepository())
                    {
                        model = UserInfoDto.ToDto(repository.FindViewById(id));
                    }
                    ViewBag.IsNew = false;
                }
                else
                {
                    model = new ViewUserInfoModel
                    {
                        Id = String.Empty
                    };
                    ViewBag.IsNew = true;
                }
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AskForLicense()
        {
            try
            {
                ResponseMessageModel response = await _service.AskForLicense();
                return Json(response);
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

    }
}