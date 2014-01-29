using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BitsBlog.CMS.Web.Controllers;

namespace BitsBlog.CMS.Web.MVCAuthorizeFilters
{
    public class LoginRequired : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) 
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
                
            if (!skipAuthorization) 
            {
                base.OnAuthorization(filterContext);

                //redirect unauthenticated user to the login page.
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.Result = new RedirectResult("~/Admin/Index");
                    return;
                }

                //if (filterContext.Result is HttpUnauthorizedResult)
                //{
                //    filterContext.Result = new RedirectResult("~/Account/AccessDenied");
                //    return;
                //}
            }
        }
    }
}
