using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coevery.Themes;

namespace Coevery.Core.Common.Controllers
{
    [Themed]
    public class ErrorController : Controller {

        public ActionResult NotFound(string url) {
            return HttpNotFound();
        }
    }
}