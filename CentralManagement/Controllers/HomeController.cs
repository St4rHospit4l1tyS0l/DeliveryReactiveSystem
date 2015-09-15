using System;
using System.Security.Principal;
using System.Web.Mvc;
using Drs.Model.Constants;
using Drs.Repository.Account;
using Microsoft.AspNet.Identity;

namespace CentralManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public static String GetRolesByUser(IPrincipal user)
        {
            using (var repository = new UserRepository())
            {
                return String.Join(", ", repository.RolesByUserId(user.Identity.GetUserId()));
            }

        }
    }
}