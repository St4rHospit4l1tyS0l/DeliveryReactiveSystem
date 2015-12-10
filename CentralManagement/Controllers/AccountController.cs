using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CentralManagement.Models;
using Drs.Model.Constants;
using Drs.Model.Shared;
using Drs.Repository.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace CentralManagement.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
            //ViewBag.ReturnUrl = returnUrl;
            //return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ResponseMsg { IsSuccess = false, Message = "Usuario y/o contraseñas no válidos" });
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return Json(new ResponseMsg { IsSuccess = true, Message = ViewBag.errorMessage, UrlToGo = Url.Action("Index", "Home", new{ area = ""}) });
                    //return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return Json(new ResponseMsg { IsSuccess = false, Message = "Cuenta desactivada" });
                    //return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return Json(new ResponseMsg { IsSuccess = false, Message = "Requiere verificación" });
                    //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
                //case SignInStatus.Failure:
                default:
                    return Json(new ResponseMsg { IsSuccess = false, Message = "Usuario y/o contraseñas inválidos" });
                    //ModelState.AddModelError("", "Invalid login attempt.");
                    //return View(model);
            }
        }


        // 99630110@..Lc
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> CreateUserDefault()
        {
            var user = new ApplicationUser { UserName = "MainAdminUser", Email = "administrator@starpms.com" };
            var result = await UserManager.CreateAsync(user, "C@llc3nt3rm4n4g3r");

            if (result.Succeeded)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                if (!roleManager.RoleExists(RoleConstants.INSTALLER))
                    await roleManager.CreateAsync(new IdentityRole(RoleConstants.INSTALLER));
                if (!roleManager.RoleExists(RoleConstants.AGENT))
                    await roleManager.CreateAsync(new IdentityRole(RoleConstants.AGENT));
                if (!roleManager.RoleExists(RoleConstants.MANAGER))
                    await roleManager.CreateAsync(new IdentityRole(RoleConstants.MANAGER));

                await UserManager.AddToRoleAsync(user.Id, RoleConstants.INSTALLER);
                await SignInManager.SignInAsync(user, false, false);

                //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //if (Request.Url == null)
                //    return RedirectToAction("Index", "Home");
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, Request.Url.Scheme);

                //await UserManager.SendEmailAsync(user.Id, "Verifica tu cuenta", "Por favor verifica tu cuenta. Debes dar clic <a href=\"" + callbackUrl + "\">aquí</a>");

                // Uncomment to debug locally 
                // TempData["ViewBagLink"] = callbackUrl;

                //ViewBag.Message = "Se ha enviado un correo para confirmar tu cuenta. Usted debe confirmarla antes de ingresar al sistema";

                return Json("OK", JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index", "Home");
            }

            return Json(new JavaScriptSerializer().Serialize(result.Errors), JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirme su cuenta de correo");

                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //if (Request.Url == null)
                    //    return RedirectToAction("Index", "Home");
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, Request.Url.Scheme);

                    //await UserManager.SendEmailAsync(user.Id, "Verifica tu cuenta", "Por favor verifica tu cuenta. Debes dar clic <a href=\"" + callbackUrl + "\">aquí</a>");

                    // Uncomment to debug locally 
                    // TempData["ViewBagLink"] = callbackUrl;

                    ViewBag.Message = "Se ha enviado un correo para confirmar tu cuenta. Usted debe confirmarla antes de ingresar al sistema";

                    return View("Info");
                    //return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        ////
        //// GET: /Account/ConfirmEmail
        //[AllowAnonymous]
        //public async Task<ActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //        return View("Error");
        //    }
        //    var result = await UserManager.ConfirmEmailAsync(userId, code);
        //    return View(result.Succeeded ? "ConfirmEmail" : "Error");
        //}

        ////
        //// GET: /Account/ForgotPassword
        //[AllowAnonymous]
        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return Json(new ResponseMsg { IsSuccess = true, Message = "Revise su correo por favor" });
        //            //return View("ForgotPasswordConfirmation");
        //        }

        //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //        // Send an email with this link
        //        string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        if (Request.Url == null) return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, Request.Url.Scheme);
        //        //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Para recuperar su contraseña de clic <a href=\"" + callbackUrl + "\">aquí</a>");
        //        //return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //        return Json(new ResponseMsg { IsSuccess = true, Message = "Las instrucciones para recuperar su contraseña se han enviado a su correo" });
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return Json(new ResponseMsg { IsSuccess = false, Message = "Ingrese un correo válido" });
        //    //return View(model);
        //}


        //
        // POST: /Account/LogOff
        //[ValidateAntiForgeryToken]
        [HttpGet]
        public ActionResult LogOff()
        {
            //AuthenticationManager.SignOut();
            //return RedirectToAction("Index", "Home");
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            Session.Abandon();
            return RedirectToAction("Login", "Account", new { area = "" });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}