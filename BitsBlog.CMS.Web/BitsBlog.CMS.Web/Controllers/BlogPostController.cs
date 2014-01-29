using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BitsBlog.CMS.Web.Controllers
{
    public class BlogPostController : Controller
    {
        //[AllowHtml]
        public ActionResult Create()
        {
            return View("BlogPostCreateUpdate");
        }

        public ActionResult Update()
        {
            return View("BlogPostCreateUpdate");
        }

        public ActionResult Delete()
        {
            return View();
        }
    }
}
