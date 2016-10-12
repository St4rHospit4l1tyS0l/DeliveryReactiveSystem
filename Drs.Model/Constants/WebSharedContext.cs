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

        public static int GetMenuPosition(string role)
        {
            List<MenuItem> lstMenuByRole;

            if(DicWebMenu.TryGetValue(role, out lstMenuByRole) == false)
                return 0;

            var area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"];
            var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];
            var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"];
            var route = String.Format("{0}/{1}/{2}", area, controller, action);
            return FindMenuPosition(route, lstMenuByRole);
        }

        private static int FindMenuPosition(string route, List<MenuItem> lstMenuByRole)
        {
            var menuItem = lstMenuByRole.FirstOrDefault(e => e.Route == route);
            if (menuItem != null)
                return menuItem.Position;

            return (lstMenuByRole.Where(item => item.SubMenu.Any())
                .Select(item => FindMenuPosition(route, item.SubMenu)))
                .FirstOrDefault(position => position != 0);
        }
    }
}