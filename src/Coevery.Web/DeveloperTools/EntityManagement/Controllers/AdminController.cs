using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.DeveloperTools.EntityManagement.Services;
using Coevery.DeveloperTools.EntityManagement.ViewModels;
using Coevery.Localization;
using Coevery.Utility.Extensions;
using System.Data.Entity.Design.PluralizationServices;

namespace Coevery.DeveloperTools.EntityManagement.Controllers {
    public class AdminController : Controller, IUpdateModel {
        private readonly IContentDefinitionEditorEvents _contentDefinitionEditorEvents;
        private readonly IEntityMetadataService _entityMetadataService;

        public AdminController(
            ICoeveryServices coeveryServices,
            IContentDefinitionEditorEvents contentDefinitionEditorEvents,
            IEntityMetadataService entityMetadataService) {
            Services = coeveryServices;
            T = NullLocalizer.Instance;
            _contentDefinitionEditorEvents = contentDefinitionEditorEvents;
            _entityMetadataService = entityMetadataService;
        }

        public ICoeveryServices Services { get; private set; }
        public Localizer T { get; set; }

        #region Entity Methods

        public ActionResult List(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Create() {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to create a content type."))) {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new EditTypeViewModel();
            viewModel.Settings.Add("CollectionDisplayName", string.Empty);
            return View(viewModel);
        }

        public ActionResult EntityName(string entityName, int version) {
            var pluralService = PluralizationService.CreateService(new CultureInfo("en-US"));
            return Json(new {
                pluralName = pluralService.Pluralize(entityName),
                version = version
            });
        }

        [HttpPost]
        public ActionResult Publish(string id) {
            var entity = _entityMetadataService.GetEntity(id);
            Services.ContentManager.Publish(entity.ContentItem);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(EditTypeViewModel viewModel) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to create a content type."))) {
                return new HttpUnauthorizedResult();
            }

            viewModel.DisplayName = string.IsNullOrWhiteSpace(viewModel.DisplayName) ? String.Empty : viewModel.DisplayName.Trim();
            viewModel.Name = (viewModel.Name ?? viewModel.DisplayName).ToSafeName();

            if (String.IsNullOrWhiteSpace(viewModel.DisplayName)) {
                ModelState.AddModelError("DisplayName", T("The Display Name name can't be empty.").ToString());
            }

            if (String.IsNullOrWhiteSpace(viewModel.Name)) {
                ModelState.AddModelError("Name", T("The Content Type Id can't be empty.").ToString());
            }

            var existEntity = _entityMetadataService.GetEntity(viewModel.Name);
            if (existEntity != null) {
                ModelState.AddModelError("Name", T("A type with the same Name already exists.").ToString());
            }

            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                var temp = (from values in ModelState
                    from error in values.Value.Errors
                    select error.ErrorMessage).ToArray();
                return Content(string.Concat(temp));
            }

            _entityMetadataService.CreateEntity(viewModel.Name, viewModel.DisplayName, viewModel.Settings);
            return Json(new {entityName = viewModel.Name});
        }

        public ActionResult Edit(string id) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content type."))) {
                return new HttpUnauthorizedResult();
            }

            var entity = _entityMetadataService.GetEntity(id);

            if (entity == null) {
                return HttpNotFound();
            }
            var viewModel = new EditTypeViewModel {
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                Settings = entity.EntitySettings
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(string id, EditTypeViewModel viewModel) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content type."))) {
                return new HttpUnauthorizedResult();
            }

            if (string.IsNullOrWhiteSpace(viewModel.DisplayName)) {
                return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            var entity = _entityMetadataService.GetEntity(id, VersionOptions.DraftRequired);
            if (entity == null) {
                return HttpNotFound();
            }

            entity.DisplayName = viewModel.DisplayName;
            var entitySettings = entity.EntitySettings;
            foreach (var setting in  viewModel.Settings) {
                entitySettings[setting.Key] = setting.Value;
            }
            entity.EntitySettings = entitySettings;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Detail(string id) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content type."))) {
                return new HttpUnauthorizedResult();
            }

            var entity = _entityMetadataService.GetEntity(id);

            if (entity == null) {
                return HttpNotFound();
            }

            var viewModel = new EditTypeViewModel {
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                Settings = entity.EntitySettings
            };

            return View(viewModel);
        }

        #endregion

        #region Field Methods

        public ActionResult ChooseFieldType(string id) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content part."))) {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new AddFieldViewModel {
                Fields = _contentDefinitionEditorEvents.FieldTypeDescriptor().OrderBy(x => x.TemplateName),
            };

            return View(viewModel);
        }

        public ActionResult FillFieldInfo(string id, string fieldTypeName) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content part."))) {
                return new HttpUnauthorizedResult();
            }
            var contentFieldDefinition = new ContentFieldDefinition(fieldTypeName);
            var definition = new ContentPartFieldDefinition(contentFieldDefinition, string.Empty, new SettingsDictionary());
            var templates = _contentDefinitionEditorEvents.PartFieldEditor(definition);

            var viewModel = new AddFieldViewModel {
                FieldTypeName = fieldTypeName,
                TypeTemplates = templates,
                AddInLayout = true
            };

            return View(viewModel);
        }

        public ActionResult ConfirmFieldInfo(string id) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content part."))) {
                return new HttpUnauthorizedResult();
            }
            return View();
        }

        [HttpPost, ActionName("FillFieldInfo")]
        public ActionResult FillFieldInfoPost(string id, AddFieldViewModel viewModel) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content part."))) {
                return new HttpUnauthorizedResult();
            }

            if (String.IsNullOrWhiteSpace(viewModel.DisplayName)) {
                ModelState.AddModelError("DisplayName", T("The Display Name name can't be empty.").ToString());
            }

            if (String.IsNullOrWhiteSpace(viewModel.Name)) {
                ModelState.AddModelError("Name", T("The Technical Name can't be empty.").ToString());
            }

            if (viewModel.Name.ToLower() == "id") {
                ModelState.AddModelError("Name", T("The Field Name can't be any case of 'Id'.").ToString());
            }

            viewModel.DisplayName = viewModel.DisplayName.Trim();
            viewModel.Name = viewModel.Name.ToSafeName();

            _entityMetadataService.AddField(id, viewModel.Name, viewModel.DisplayName, viewModel.FieldTypeName, viewModel.AddInLayout, this);
            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                var errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                    .Select(m => m.ErrorMessage).ToArray();
                return Content(string.Concat(errors));
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult FieldName(bool creatingEntity, string entityName, string displayName, int version) {
            if (creatingEntity) {
                return Json(new {
                    result = displayName.ToSafeName(),
                    version = version
                });
            }
            return Json(new {
                result = displayName.ToSafeName(),
                version = version
            });
        }

        public ActionResult EditFields(string id, string fieldName) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content type."))) {
                return new HttpUnauthorizedResult();
            }
            var field = _entityMetadataService.GetFields(id).FirstOrDefault(x => x.Name == fieldName);
            if (field == null) {
                return HttpNotFound();
            }
            var fieldDefinition = new ContentFieldDefinition(field.FieldType);
            var viewModel = new EditPartFieldViewModel {
                Name = field.Name,
                DisplayName = field.DisplayName,
                Settings = field.Settings,
                FieldDefinition = new EditFieldViewModel(fieldDefinition),
                Templates = _contentDefinitionEditorEvents.PartFieldEditor(new ContentPartFieldDefinition(fieldDefinition, field.Name, field.Settings))
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("EditFields")]
        public ActionResult EditFieldsPost(EditPartFieldViewModel viewModel, string id) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content type."))) {
                return new HttpUnauthorizedResult();
            }

            if (String.IsNullOrWhiteSpace(viewModel.DisplayName)) {
                ModelState.AddModelError("DisplayName", T("The Display Name name can't be empty.").ToString());
            }

            _entityMetadataService.UpdateField(id, viewModel.Name, viewModel.DisplayName.Trim(), this);

            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                var temp = (from values in ModelState
                    from error in values.Value.Errors
                    select error.ErrorMessage).ToArray();
                return Content(string.Concat(temp));
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Fields() {
            return View();
        }

        #endregion

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return base.TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}