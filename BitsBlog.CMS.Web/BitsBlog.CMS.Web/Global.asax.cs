using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BitsBlog.CMS.Web.MVCAuthorizeFilters;

namespace BitsBlog.CMS.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LoginRequired());
            //filters.Add(new RequireHttpsAttribute()); 
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(Object sender, EventArgs E)
        {
            BitsBlog.Authentication.Utils.AuthUtils.Logout();
        }

        protected void Session_End(Object sender, EventArgs E)
        {
            BitsBlog.Authentication.Utils.AuthUtils.Logout();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //do not allow pages into frames
            HttpContext.Current.Response.AddHeader("x-frame-options", "DENY"); //TODO add rules to IIS for this as a custom header
        }
    }
}