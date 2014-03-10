using System.Web.Mvc;
using Coevery.Themes;
using Coevery.UI.Admin;

namespace Coevery.Core.Common.Controllers {
    public class AdminController : Controller {
        [Admin, Themed]
        public ActionResult Index(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}