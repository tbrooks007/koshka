using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BitsBlog.CMS.Web.Models.ViewModels;

namespace BitsBlog.CMS.Web.Controllers
{
    public class BlogSettingsController : Controller
    {

        public ActionResult Create()
        {
            return View("BlogSettingsCreateUpdate");
        }

        public ActionResult Update(int id, BitsBlog.Core.Blog model)
        {
            return View("BlogSettingsCreateUpdate");
        }
    }
}
