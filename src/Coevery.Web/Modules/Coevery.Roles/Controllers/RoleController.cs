using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Coevery.Localization;
using Coevery.Logging;
using Coevery.Mvc;
using Coevery.Mvc.Extensions;
using Coevery.Roles.Models;
using Coevery.Roles.Services;
using Coevery.Roles.ViewModels;
using Coevery.Security;
using Coevery.Themes;
using Coevery.UI.Navigation;
using Coevery.UI.Notify;

namespace Coevery.Roles.Controllers {
    [ValidateInput(false)]
    [Themed]
    public class RoleController : Controller {
        private readonly IRoleService _roleService;
        private readonly IAuthorizationService _authorizationService;

        public RoleController(
            ICoeveryServices services,
            IRoleService roleService,
            INotifier notifier,
            IAuthorizationService authorizationService) {
            Services = services;
            _roleService = roleService;
            _authorizationService = authorizationService;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult Index() {
            return List();
        }

        public ActionResult List() {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage roles")))
                return new HttpUnauthorizedResult();

            return View("List");
        }

        [HttpPost, ActionName("List")]
        public ActionResult List(int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc")
        {
            var query = _roleService.GetRoles().ToList();
            var pager = new Pager(Services.WorkContext.CurrentSite, page, pageSize);

            var totalRecords = query.Count();
            var records = query
                .Skip(pager.GetStartIndex())
                .Take(pageSize)
                .Select(x => new RolesListViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                });
            var result = new
            {
                page,
                totalPages = totalRecords / pageSize,
                totalRecords,
                rows = records
            };
            return Json(result);
        }

        public ActionResult Create() {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage roles")))
                return new HttpUnauthorizedResult();

            var model = new RoleCreateViewModel { FeaturePermissions = _roleService.GetInstalledPermissions() };
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST() {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage roles")))
                return new HttpUnauthorizedResult();

            var viewModel = new RoleCreateViewModel();
            TryUpdateModel(viewModel);

            if(String.IsNullOrEmpty(viewModel.Name)) {
                ModelState.AddModelError("Name", T("Role name can't be empty"));
            }

            var role = _roleService.GetRoleByName(viewModel.Name);
            if (role != null) {
                ModelState.AddModelError("Name", T("Role with same name already exists"));
            }

            if (!ModelState.IsValid) {
                viewModel.FeaturePermissions = _roleService.GetInstalledPermissions();
                return View(viewModel);
            }

            _roleService.CreateRole(viewModel.Name);
            foreach (string key in Request.Form.Keys) {
                if (key.StartsWith("Checkbox.") && Request.Form[key] == "true") {
                    string permissionName = key.Substring("Checkbox.".Length);
                    _roleService.CreatePermissionForRole(viewModel.Name,
                                                            permissionName);
                }
            }
            return RedirectToAction("List");
        }

        public ActionResult Edit(int id) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage roles")))
                return new HttpUnauthorizedResult();

            var role = _roleService.GetRole(id);
            if (role == null) {
                return HttpNotFound();
            }

            var model = new RoleEditViewModel { Name = role.Name, Id = role.Id, 
                                                RoleCategoryPermissions = _roleService.GetInstalledPermissions(),
                                                CurrentPermissions = _roleService.GetPermissionsForRole(id)};

            var simulation = UserSimulation.Create(role.Name);
            model.EffectivePermissions = model.RoleCategoryPermissions
                .SelectMany(group => group.Value)
                .Where(permission => _authorizationService.TryCheckAccess(permission, simulation, null))
                .Select(permission=>permission.Name)
                .Distinct()
                .ToList();

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditSavePOST(int id) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage roles")))
                return new HttpUnauthorizedResult();

            var viewModel = new RoleEditViewModel();
            TryUpdateModel(viewModel);

            if (String.IsNullOrEmpty(viewModel.Name)) {
                ModelState.AddModelError("Name", T("Role name can't be empty"));
            }

            var role = _roleService.GetRoleByName(viewModel.Name);
            if (role != null && role.Id != id) {
                ModelState.AddModelError("Name", T("Role with same name already exists"));
            }

            if (!ModelState.IsValid) {
                return Edit(id);
            }

            // Save
            List<string> rolePermissions = new List<string>();
            foreach (string key in Request.Form.Keys) {
                if (key.StartsWith("Checkbox.") && Request.Form[key] == "true") {
                    string permissionName = key.Substring("Checkbox.".Length);
                    rolePermissions.Add(permissionName);
                }
            }
            _roleService.UpdateRole(viewModel.Id, viewModel.Name, rolePermissions);

            Services.Notifier.Information(T("Your Role has been saved."));
            return RedirectToAction("Edit", new { id });
        }

        [HttpPost]
        public ActionResult Delete(List<int> selectedIds)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage roles")))
                return new HttpUnauthorizedResult();
            foreach (var id in selectedIds)
            {
                _roleService.DeleteRole(id);
            }
            Services.Notifier.Information(T("Role was successfully deleted."));

            return new HttpStatusCodeResult(HttpStatusCode.OK, T("Delete succeeded").Text);
        }
    }
}
