using System;
using System.Web;
using Drs.Repository.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using CentralManagement.Models;


namespace CentralManagement.Helper
{
    public static class RoleHelper
    {
        public static ApplicationRole GetRoleByName(string name)
        {
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            var result = roleManager.FindByName(name);
            return result;
        }

        public static string RoleForUser(string userName)
        {
            using (var rep = new AccountRepository())
            {
                var role = rep.GetRoleDescByUsername(userName);
                if (role != null)
                    return role;
            }

            return String.Empty;
        }
    }
}