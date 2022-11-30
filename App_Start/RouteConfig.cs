using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PPCP07302018
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ClaimConfirm",
                url: "ClaimConfirm/{id}",
                defaults: new { controller = "Account", action = "ClaimConfirm", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "ClaimDeny",
               url: "ClaimDeny/{id}",
               defaults: new { controller = "Account", action = "ClaimDeny", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Member", action = "MemberLogin", id = UrlParameter.Optional }
            );
        }
    }
}
