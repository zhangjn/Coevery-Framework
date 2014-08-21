using System.Web.Mvc;
using Coevery.DeveloperTools.ListViewDesigner.ViewModels;
using Coevery.Localization;
using Coevery.Security;
using IGridService = Coevery.DeveloperTools.ListViewDesigner.Services.IGridService;

namespace Coevery.DeveloperTools.ListViewDesigner.Controllers {
    public class AdminController : Controller {
        private readonly IGridService _gridService;

        public AdminController(
            ICoeveryServices services,
            IGridService gridService) {
            _gridService = gridService;
            Services = services;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult List() {
            return View();
        }

        public ActionResult Create(string id) {
            var viewModel = _gridService.GetGridViewModel(id);
            if (viewModel == null) {
                return HttpNotFound();
            }

            return View("Edit", viewModel);
        }

        public ActionResult Edit(int id) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to edit queries"))) {
                return new HttpUnauthorizedResult();
            }

            var viewModel = _gridService.GetGridViewModel(id);
            return View(viewModel);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int id, GridViewModel viewModel, string returnUrl) {
            var viewid = _gridService.Save(id, viewModel);
            return Json(new {id = viewid});
        }
    }
}