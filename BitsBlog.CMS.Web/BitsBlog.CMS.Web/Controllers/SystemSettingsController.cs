using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BitsBlog.CMS.Web.Controllers
{
    public class SystemSettingsController : Controller
    {
        public ActionResult Index()
        {
            return View("SystemSettingsUpdate");
        }

        public ActionResult Update(BitsBlog.Core.SystemSettings model)
        {
            return View("SystemSettingsUpdate");
        }
    }
}
