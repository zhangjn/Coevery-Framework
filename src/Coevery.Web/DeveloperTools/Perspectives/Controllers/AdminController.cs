using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Navigation;
using Coevery.Core.Navigation.Models;
using Coevery.DeveloperTools.Perspectives.Models;
using Coevery.DeveloperTools.Perspectives.ViewModels;
using Coevery.Localization;
using Coevery.Logging;
using Coevery.UI.Navigation;
using Coevery.Utility;

namespace Coevery.DeveloperTools.Perspectives.Controllers {
    public class AdminController : Controller, IUpdateModel {
        private readonly IContentManager _contentManager;
        private readonly INavigationManager _navigationManager;

        public AdminController(
            ICoeveryServices coeveryServices,
            IContentManager contentManager,
            INavigationManager navigationManager) {
            Services = coeveryServices;
            _contentManager = contentManager;
            _navigationManager = navigationManager;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; private set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult List(string id) {
            return View(new GridRowSortSettingViewModel {
                Url = "api/Perspectives/Perspective/PostReorderInfo",
                Method = "Post",
                Handler = string.Empty
            });
        }

        public ActionResult Create() {
            PerspectiveViewModel model = new PerspectiveViewModel();
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST(PerspectiveViewModel model) {
            var contentItem = _contentManager.New("Menu");
            var currPos = 0;
            var lastPos = _contentManager.Query<PerspectivePart, PerspectivePartRecord>().OrderByDescending(x => x.Position).ForType("Menu").List().FirstOrDefault();
            if (lastPos != null) {
                currPos = lastPos.Position;
                currPos++;
            }
            contentItem.As<PerspectivePart>().Title = model.Title;
            contentItem.As<PerspectivePart>().Description = model.Description;
            contentItem.As<PerspectivePart>().Position = currPos;
            contentItem.As<PerspectivePart>().CurrentLevel = 0;
            _contentManager.Create(contentItem, VersionOptions.Draft);
            _contentManager.Publish(contentItem);
            return Json(new {id = contentItem.Id});
        }

        public ActionResult Edit(int id) {
            var contentItem = _contentManager.Get(id, VersionOptions.Latest);
            PerspectiveViewModel model = new PerspectiveViewModel();
            model.Id = contentItem.As<PerspectivePart>().Id;
            model.Title = contentItem.As<PerspectivePart>().Title;
            model.Description = contentItem.As<PerspectivePart>().Description;
            model.Position = contentItem.As<PerspectivePart>().Position;
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPOST(PerspectiveViewModel model) {
            var contentItem = _contentManager.Get(model.Id, VersionOptions.DraftRequired);
            contentItem.As<PerspectivePart>().Title = model.Title;
            contentItem.As<PerspectivePart>().Description = model.Description;
            contentItem.As<PerspectivePart>().Position = model.Position;
            _contentManager.Publish(contentItem);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Detail(int id) {
            var contentItem = _contentManager.Get(id, VersionOptions.Latest);
            var model = new PerspectiveViewModel {
                Id = contentItem.As<PerspectivePart>().Id,
                Title = contentItem.As<PerspectivePart>().Title,
                Description = contentItem.As<PerspectivePart>().Description,
                Position = contentItem.As<PerspectivePart>().Position,
                NavigationTypeList = new List<ContentTypeDefinition>()
            };
            foreach (var type in PerspectiveViewModel.NavigationTypes) {
                var temp = _contentManager.GetContentTypeDefinitions().FirstOrDefault(definition => definition.Name == type);
                if (temp != null) {
                    model.NavigationTypeList.Add(temp);
                }
            }
            return View(model);
        }

        public ActionResult EditNavigationItem(int id, string type) {
            var menuPart = Services.ContentManager.Get<MenuPart>(id);

            if (menuPart == null) {
                return HttpNotFound();
            }

            try {
                var model = Services.ContentManager.BuildEditor(menuPart, "Detail");

                model.Title = menuPart.Menu.As<PerspectivePart>().Title;

                return View("NavigationItem", model);
            }
            catch (Exception exception) {
                Logger.Error(T("Creating menu item failed: {0}", exception.Message).Text);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, (T("Creating menu item failed: {0}", exception.Message).Text));
            }
        }

        [HttpPost, ActionName("EditNavigationItem")]
        public ActionResult EditNavigationItemPOST(int id) {
            if (!Services.Authorizer.Authorize(Permissions.ManageMainMenu, T("Couldn't manage the main menu"))) {
                return new HttpUnauthorizedResult();
            }

            var menuPart = Services.ContentManager.Get<MenuPart>(id);

            if (menuPart == null) {
                return HttpNotFound();
            }

            var model = Services.ContentManager.UpdateEditor(menuPart, this);

            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();
                return View("NavigationItem", model);
            }

            return Content(T("Your {0} has been updated.", menuPart.TypeDefinition.DisplayName).Text);
        }

        public ActionResult CreateNavigationItem(int id, string type) {
            if (!Services.Authorizer.Authorize(Permissions.ManageMainMenu, T("Couldn't manage the main menu"))) {
                return new HttpUnauthorizedResult();
            }

            // create a new temporary menu item
            var menuPart = Services.ContentManager.New<MenuPart>(type);

            if (menuPart == null) {
                return HttpNotFound();
            }

            // load the menu
            var menu = Services.ContentManager.Get(id);

            if (menu == null) {
                return HttpNotFound();
            }

            try {
                // filter the content items for this specific menu
                menuPart.MenuPosition = Position.GetNext(_navigationManager.BuildMenu(menu));
                menuPart.Menu = menu;

                var model = Services.ContentManager.BuildEditor(menuPart, "Detail");

                model.Title = menu.As<PerspectivePart>().Title;

                return View("NavigationItem", model);
            }
            catch (Exception exception) {
                Logger.Error(T("Creating menu item failed: {0}", exception.Message).Text);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, (T("Creating menu item failed: {0}", exception.Message).Text));
            }
        }

        [HttpPost, ActionName("CreateNavigationItem")]
        public ActionResult CreateNavigationItemPOST(int id, string type) {
            if (!Services.Authorizer.Authorize(Permissions.ManageMainMenu, T("Couldn't manage the main menu"))) {
                return new HttpUnauthorizedResult();
            }

            var menuPart = Services.ContentManager.New<MenuPart>(type);

            if (menuPart == null) {
                return HttpNotFound();
            }

            // load the menu
            var menu = Services.ContentManager.Get(id);

            if (menu == null) {
                return HttpNotFound();
            }

            var model = Services.ContentManager.UpdateEditor(menuPart, this);

            menuPart.MenuPosition = Position.GetNext(_navigationManager.BuildMenu(menu));
            menuPart.Menu = menu;

            Services.ContentManager.Create(menuPart);

            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();
                return View("NavigationItem", model);
            }

            return Content(T("Your {0} has been added.", menuPart.TypeDefinition.DisplayName).Text);
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}