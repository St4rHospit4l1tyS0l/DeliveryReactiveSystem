using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Drs.Model.Shared;

namespace Drs.Model.Constants
{
    public static class WebSharedContext
    {
        public static Dictionary<string, List<MenuItem>> DicWebMenu { get; set; }

        public static MenuItem GetMenuPosition(string role)
        {
            List<MenuItem> lstMenuByRole;

            if(DicWebMenu.TryGetValue(role, out lstMenuByRole) == false)
                return null;

            var area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"];
            var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];
            var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"];
            var route = String.Format("{0}/{1}/{2}", area, controller, action);
            return FindMenuPosition(route, lstMenuByRole);
        }

        private static MenuItem FindMenuPosition(string route, List<MenuItem> lstMenuByRole)
        {
            var menuItem = lstMenuByRole.FirstOrDefault(e => e.Route == route);
            if (menuItem != null)
                return menuItem;

            foreach (var item in lstMenuByRole.Where(item => item.SubMenu.Any()))
            {
                menuItem = FindMenuPosition(route, item.SubMenu);
                if (menuItem != null) return menuItem;
            }
            return null;
        }
    }
}