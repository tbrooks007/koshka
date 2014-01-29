using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BitsBlog.Authentication.MembershipProviders;
using System.Web.Security;

namespace BitsBlog.CMS.Web.Controllers
{
    public class BitsBaseController : Controller
    {
        protected static BitsBlogMembershipProvider MembershipProvider = null;
        protected readonly string MembershipProviderName = "BitsBlogMembershipProvider";

        public BitsBaseController()
        {
            if (MembershipProvider == null)
                MembershipProvider = (BitsBlogMembershipProvider)Membership.Providers["BitsBlogMembershipProvider"];
        }
    }
}
