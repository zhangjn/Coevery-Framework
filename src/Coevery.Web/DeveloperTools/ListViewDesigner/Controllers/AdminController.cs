using System.Linq;
using System.Net;
using System.Web.Mvc;
using Coevery.Core.Forms.Services;
using Coevery.Core.Projections.Services;
using Coevery.Core.Projections.ViewModels;
using Coevery.Localization;
using Coevery.Mvc;
using Coevery.Security;
using IGridService = Coevery.DeveloperTools.ListViewDesigner.Services.IGridService;

namespace Coevery.DeveloperTools.ListViewDesigner.Controllers {
    public class AdminController : Controller {
        private readonly IProjectionService _projectionService;
        private readonly IProjectionManager _projectionManager;
        private readonly IFormManager _formManager;
        private readonly IGridService _gridService;

        public AdminController(
            ICoeveryServices services,
            IFormManager formManager,
            IProjectionManager projectionManager,
            IProjectionService projectionService,
            IGridService gridService) {
            _projectionService = projectionService;
            _gridService = gridService;
            _projectionManager = projectionManager;
            _formManager = formManager;
            Services = services;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult List(string id) {
            //var model = new EntityViewListModel {
            //    Layouts = _projectionManager.DescribeLayouts()
            //        .SelectMany(type => type.Descriptors)
            //        .Where(type => type.Category == "Grids")
            //};
            //return View(model);
            return View();
        }

        public ActionResult Create(string id, string category, string type) {
            var viewModel = _gridService.GetViewModel(id, category, type);
            if (viewModel == null || viewModel.Layout == null) {
                return HttpNotFound();
            }
            viewModel.Form = _formManager.Build(viewModel.Layout.Form) ?? Services.New.EmptyForm();
            viewModel.Form.Fields = viewModel.Fields;
            return View("Edit", viewModel);
        }

        public ActionResult Edit(int id) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to edit queries"))) {
                return new HttpUnauthorizedResult();
            }
            ProjectionEditViewModel viewModel = _projectionService.GetProjectionViewModel(id);

            return View(viewModel);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.Save")]
        public ActionResult EditPost(int id, ProjectionEditViewModel viewModel, string returnUrl) {
            viewModel.Layout = _projectionManager.DescribeLayouts()
                .SelectMany(descr => descr.Descriptors)
                .FirstOrDefault(descr => descr.Category == viewModel.Layout.Category && descr.Type == viewModel.Layout.Type);
            if (viewModel.Layout == null) {
                return HttpNotFound();
            }
            _formManager.Validate(new ValidatingContext {FormName = viewModel.Layout.Form, ModelState = ModelState, ValueProvider = ValueProvider});
            if (!ModelState.IsValid) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewid = _gridService.Save(id, viewModel);
            return Json(new {id = viewid});
        }
    }
}