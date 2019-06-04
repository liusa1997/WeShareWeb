using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace 剧享网
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
               // defaults: new { controller = "Manager", action = "MainPage", id = UrlParameter.Optional }
              // defaults: new { controller = "NotifyInfo", action = "NotifyIndex", id = UrlParameter.Optional }
               // defaults: new { controller = "T_User", action = "Create", id = UrlParameter.Optional }
               defaults: new { controller = "T_User", action = "Index", id = UrlParameter.Optional }
               // defaults: new { controller = "T_User", action = "contact", id = UrlParameter.Optional }
               //defaults: new { controller = "Test", action = "Test", id = UrlParameter.Optional }
               //defaults: new { controller = "SearchInfo", action = "Index", id = UrlParameter.Optional }
               //defaults: new { controller = "T_User", action = "MyEassy", id = UrlParameter.Optional }
            );
        }
    }
}
