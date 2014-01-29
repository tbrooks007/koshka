using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BitsBlog.CMS.Web.Models.ViewModels;
using BitsBlog.CMS.Web.MVCAuthorizeFilters;

namespace BitsBlog.CMS.Web.Controllers
{
    public class AdminController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult InvalidLogin(LoginViewModel model)
        {
            if(string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                ViewBag.ErrorMessage = "Please enter a valid username and password.";

            return View("index", model);
        }
    }
}
