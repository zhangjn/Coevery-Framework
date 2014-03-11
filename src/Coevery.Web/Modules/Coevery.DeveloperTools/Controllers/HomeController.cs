using System.Web.Mvc;
using Coevery.Themes;
using Coevery.UI.Admin;

namespace Coevery.DeveloperTools.Controllers {
    [Admin]
    public class HomeController : Controller {
        [Themed]
        public ActionResult Index(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}