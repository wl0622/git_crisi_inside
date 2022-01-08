using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace inside.crsri
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "MvcPager_PageSize",
                "{controller}/{action}/{id}/{pageindex}",
                new { controller = "Article", action = "list", id = UrlParameter.Optional, pageindex = UrlParameter.Optional }, new { action = "list", pageindex = @"\d*" }
                );


            routes.MapRoute(
                "MvcPagerQuery_PageSize",
                "{controller}/{action}/{keyword}/{pageindex}",
                new { controller = "Article", action = "SearchResult", keyword = UrlParameter.Optional, pageindex = UrlParameter.Optional }, new { action = "SearchResult", pageindex = @"\d*" }
                );

            routes.MapRoute(
            "jhxy_PageSize",
            "{controller}/{action}/{id}/{pageindex}",
            new { controller = "jhxy", action = "Index", id = UrlParameter.Optional, pageindex = UrlParameter.Optional }
            );
        }
    }
}