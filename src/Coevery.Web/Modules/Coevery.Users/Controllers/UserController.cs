using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Coevery.Core.Settings.Models;
using Coevery.Security;
using Coevery.Settings;
using Coevery.UI.Notify;
using Coevery.ContentManagement;
using Coevery.Themes;
using Coevery.Localization;
using Coevery.Users.Models;
using Coevery.Users.Services;
using Coevery.Users.ViewModels;
using Coevery.Mvc.Extensions;

namespace Coevery.Users.Controllers {
    [Themed]
    public class UserController : Controller, IUpdateModel {
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly ISiteService _siteService;

        public UserController(ICoeveryServices services,
            IMembershipService membershipService,
            IUserService userService,
            ISiteService siteService) {
            Services = services;
            _membershipService = membershipService;
            _userService = userService;
            _siteService = siteService;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index() {
            return List();
        }

        public ActionResult List() {
            var contentItem = Services.ContentManager.New("User");
            contentItem.Weld(new UserPart());
            var model = Services.ContentManager.BuildDisplay(contentItem, "List");
            return View("List", model);
        }

        [HttpPost]
        public ActionResult List(int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc") {
            var query = Services.ContentManager.Query<UserPart, UserPartRecord>();
            var totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1)*pageSize, pageSize)
                .Select(part => new UserListViewModel {
                    Id = part.Id,
                    UserName = part.UserName,
                    Email = part.Email
                })
                .ToList();
            var result = new {
                page,
                totalPages = totalRecords/pageSize,
                totalRecords,
                rows = records
            };
            return Json(result);
        }

        public ActionResult Detail(int id) {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null) {
                return HttpNotFound();
            }

            dynamic model = Services.ContentManager.BuildDisplay(contentItem, "Detail");
            return View((object) model);
        }

        public ActionResult Create() {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users"))) {
                return new HttpUnauthorizedResult();
            }

            var user = Services.ContentManager.New<IUser>("User");
            var editor = Services.New.EditorTemplate(TemplateName: "Parts/User.Create", Model: new UserCreateViewModel(), Prefix: null);
            var actions = Services.New.EditViewAction();
            var model = Services.ContentManager.BuildEditor(user);
            model.Content.Add(editor, "2");
            model.Content.Add(actions, "100");
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST(UserCreateViewModel createModel) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users"))) {
                return new HttpUnauthorizedResult();
            }

            if (!string.IsNullOrEmpty(createModel.UserName)) {
                if (!_userService.VerifyUserUnicity(createModel.UserName, createModel.Email)) {
                    AddModelError("NotUniqueUserName", T("User with that username and/or email already exists."));
                }
            }

            if (!Regex.IsMatch(createModel.Email ?? "", UserPart.EmailPattern, RegexOptions.IgnoreCase)) {
                // http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx    
                ModelState.AddModelError("Email", T("You must specify a valid email address."));
            }

            if (createModel.Password != createModel.ConfirmPassword) {
                AddModelError("ConfirmPassword", T("Password confirmation must match"));
            }

            var user = Services.ContentManager.New<IUser>("User");
            if (ModelState.IsValid) {
                user = _membershipService.CreateUser(new CreateUserParams(
                    createModel.UserName,
                    createModel.Password,
                    createModel.Email,
                    null, null, true));
            }

            var model = Services.ContentManager.UpdateEditor(user, this);
            var userPart = user.As<UserPart>();

            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();

                var editor = Services.New.EditorTemplate(TemplateName: "Parts/User.Create", Model: createModel, Prefix: null);
                model.Content.Add(editor, "2");
                var actions = Services.New.EditViewAction();
                model.Content.Add(actions, "100");
                return View(model);
            }

            Services.Notifier.Information(T("User created"));
            return RedirectToAction("List");
        }

        public ActionResult Edit(int id) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users"))) {
                return new HttpUnauthorizedResult();
            }

            var user = Services.ContentManager.Get<UserPart>(id);
            var model = Services.ContentManager.BuildEditor(user);
            var editor = Services.New.EditorTemplate(TemplateName: "Parts/User.Edit", Model: new UserEditViewModel {User = user}, Prefix: null);
            model.Content.Add(editor, "2");
            var actions = Services.New.EditViewAction();
            model.Content.Add(actions, "100");

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int id, string returnUrl) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users"))) {
                return new HttpUnauthorizedResult();
            }

            var user = Services.ContentManager.Get<UserPart>(id, VersionOptions.DraftRequired);
            string previousName = user.UserName;

            var model = Services.ContentManager.UpdateEditor(user, this);

            var editModel = new UserEditViewModel {User = user};
            if (TryUpdateModel(editModel)) {
                if (!_userService.VerifyUserUnicity(id, editModel.UserName, editModel.Email)) {
                    AddModelError("NotUniqueUserName", T("User with that username and/or email already exists."));
                }
                else if (!Regex.IsMatch(editModel.Email ?? "", UserPart.EmailPattern, RegexOptions.IgnoreCase)) {
                    // http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx    
                    ModelState.AddModelError("Email", T("You must specify a valid email address."));
                }
                else {
                    // also update the Super user if this is the renamed account
                    if (String.Equals(Services.WorkContext.CurrentSite.SuperUser, previousName, StringComparison.Ordinal)) {
                        _siteService.GetSiteSettings().As<SiteSettingsPart>().SuperUser = editModel.UserName;
                    }

                    user.NormalizedUserName = editModel.UserName.ToLowerInvariant();
                }
            }

            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();

                var editor = Services.New.EditorTemplate(TemplateName: "Parts/User.Edit", Model: editModel, Prefix: null);
                model.Content.Add(editor, "2");
                var actions = Services.New.EditViewAction();
                model.Content.Add(actions, "100");
                return View(model);
            }

            Services.ContentManager.Publish(user.ContentItem);

            Services.Notifier.Information(T("User information updated"));
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id) {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(List<int> selectedIds) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users"))) {
                return new HttpUnauthorizedResult();
            }
            foreach (var id in selectedIds) {
                var user = Services.ContentManager.Get<IUser>(id);

                if (user != null) {
                    if (String.Equals(Services.WorkContext.CurrentSite.SuperUser, user.UserName, StringComparison.Ordinal)) {
                        Services.Notifier.Error(T("The Super user can't be removed. Please disable this account or specify another Super user account"));
                    }
                    else if (String.Equals(Services.WorkContext.CurrentUser.UserName, user.UserName, StringComparison.Ordinal)) {
                        Services.Notifier.Error(T("You can't remove your own account. Please log in with another account"));
                    }
                    else {
                        Services.ContentManager.Remove(user.ContentItem);
                        Services.Notifier.Information(T("User {0} deleted", user.UserName));
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK, T("Delete succeeded").Text);
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        public void AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}