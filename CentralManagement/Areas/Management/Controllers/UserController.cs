using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Infrastructure.Resources;
using Drs.Repository.Account;
using Drs.Repository.Entities;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Log;
using Drs.Repository.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using ResShared = CentralManagement.Resources.ResShared;

namespace CentralManagement.Areas.Management.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // GET: Default

        //readonly IUserService _licenseService = new UserService();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(JqGridFilterModel opts)
        {
            using (var repository = new GenericRepository<ViewUserInfo>())
            {
                var result = repository.JqGridFindBy(opts, UserInfoJson.Key, UserInfoJson.Columns, null
                    , UserInfoJson.DynamicToDto);
                return Json(result);
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoUpsert(ViewUserInfoModel model)
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

                using (var repository = new UserRepository())
                {
                    if (String.IsNullOrWhiteSpace(model.Id))
                    {
                        //Verificar que el usuario no exista
                        if (repository.IsAlreadyUser(model.UserName))
                        {
                            return Json(new ResponseMessageModel
                            {
                                HasError = true,
                                Title = ResShared.TITLE_REGISTER_FAILED,
                                Message = "El usuario ya existe en el sistema"
                            });
                        }
                        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                        var role = await roleManager.FindByIdAsync(model.RoleId);

                        if (role == null)
                        {
                            return Json(new ResponseMessageModel
                            {
                                HasError = true,
                                Title = ResShared.TITLE_REGISTER_FAILED,
                                Message = "El perfil no existe en el sistema"
                            });
                        }

                        var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
                        var result = await UserManager.CreateAsync(user, model.Password);

                        if (!result.Succeeded)
                        {
                            return Json(new ResponseMessageModel
                            {
                                HasError = true,
                                Title = ResShared.TITLE_REGISTER_FAILED,
                                Message = String.Join(", ", result.Errors.ToArray())
                            });
                        }

                        await UserManager.AddToRoleAsync(user.Id, role.Name);

                        model.Id = user.Id;
                        repository.AddUser(model);

                    }
                    else
                    {
                        if (repository.UpdateUser(model) == false)
                        {
                            return Json(new ResponseMessageModel
                            {
                                HasError = true,
                                Title = ResShared.TITLE_REGISTER_FAILED,
                                Message = "Usuario no se encuentra en el sistema o ya existe un registro con el mismo usuario"
                            });
                        }
                    }
                }

                return Json(new ResponseMessageModel
                {
                    HasError = false,
                    Title = ResShared.TITLE_REGISTER_SUCCESS,
                    Message = ResShared.INFO_REGISTER_SAVED
                });

            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, model.UserName, model.FirstName, model.LastName);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Title = ResShared.TITLE_REGISTER_FAILED,
                    Message = ResShared.ERROR_UNKOWN
                });
            }
        }


        [HttpPost]
        public ActionResult DoObsolete(string id)
        {
            try
            {
                using (var repository = new UserRepository())
                {
                    repository.EnableUser(id, false);
                }

                return Json(new ResponseMessageModel
                {
                    HasError = false,
                    Title = ResShared.TITLE_OBSOLETE_SUCCESS,
                    Message = ResShared.INFO_REGISTER_SAVED
                });
                //}
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


        [HttpPost]
        public ActionResult ChangePass(string id)
        {
            UserChangePassword model = null;

            try
            {
                model = new UserChangePassword{Id = id};
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoChangePass(UserChangePassword model)
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

                using (var repository = new UserRepository())
                {
                    if (repository.UserExistsById(model.Id) == false)
                    {
                        return Json(new ResponseMessageModel
                        {
                            HasError = true,
                            Title = ResShared.TITLE_REGISTER_FAILED,
                            Message = "El usuario ya no existe"
                        });
                    }
                }

                var token = await UserManager.GeneratePasswordResetTokenAsync(model.Id);
                var result = await UserManager.ResetPasswordAsync(model.Id, token, model.Password);

                if (!result.Succeeded)
                {
                    return Json(new ResponseMessageModel
                    {
                        HasError = true,
                        Title = ResShared.TITLE_REGISTER_FAILED,
                        Message = "El usuario ya no existe o su contraseña ya fue modificada"
                    });
                }

                return Json(new ResponseMessageModel
                {
                    HasError = false,
                    Title = ResShared.TITLE_REGISTER_SUCCESS,
                    Message = ResShared.INFO_REGISTER_SAVED
                });

            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, model.Id);
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