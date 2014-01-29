using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitsBlog.CMS.Web.MVCAuthorizeFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowAnonymousAttribute : Attribute { }
}