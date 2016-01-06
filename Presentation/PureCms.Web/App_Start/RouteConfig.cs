using PureCms.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PureCms.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.Add(new PluginRouteProvider());

            //默认路由(此路由不能删除)
            routes.MapRoute("default",
                            "{controller}/{action}",
                            new { controller = "home", action = "index" },
                            new[] { "PureCms.Web.Controllers" });
        }
    }
}