﻿using System.Web.Mvc;
using Coevery.Themes;
using Coevery.UI.Admin;

namespace Coevery.DeveloperTools.EntityManagement.Controllers {
    [Admin, Themed]
    public class HomeController : Controller {
        public ActionResult Index(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}