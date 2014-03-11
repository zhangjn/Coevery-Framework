using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.Services;
using Coevery.DeveloperTools.Settings;
using Coevery.DeveloperTools.ViewModels;
using Coevery.Localization;
using Coevery.Logging;
using Coevery.Utility.Extensions;

namespace Coevery.DeveloperTools.Controllers {
    public class EntityAdminController : Controller, IUpdateModel {
        private readonly IContentDefinitionService _contentDefinitionService;
        private readonly IContentMetadataService _contentMetadataService;
        private readonly IEntityRecordEditorEvents _entityRecordEditorEvents;

        public EntityAdminController(
            ICoeveryServices coeveryServices,
            IContentDefinitionService contentDefinitionService,
            IContentMetadataService contentMetadataService,
            IEntityRecordEditorEvents entityRecordEditorEvents) {
            Services = coeveryServices;
            _contentDefinitionService = contentDefinitionService;
            T = NullLocalizer.Instance;
            _contentMetadataService = contentMetadataService;
            _entityRecordEditorEvents = entityRecordEditorEvents;
        }

        public ICoeveryServices Services { get; private set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult List(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Create() {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to create a content type."))) {
                return new HttpUnauthorizedResult();
            }
            var entityRecords = _entityRecordEditorEvents.FieldSettingsEditor().ToList();
            var fieldTypes = entityRecords.Select(x =>
                new SelectListItem {Text = x.FieldTypeDisplayName, Value = x.FieldTypeName});

            var typeViewModel = _contentDefinitionService.GetType(string.Empty);
            typeViewModel.Settings.Add("CollectionName", string.Empty);
            typeViewModel.Settings.Add("CollectionDisplayName", string.Empty);
            typeViewModel.FieldTypes = fieldTypes;
            typeViewModel.FieldTemplates = entityRecords;

            return View(typeViewModel);
        }

        public ActionResult EntityName(string displayName, int version) {
            return Json(new {
                result = _contentMetadataService.ConstructEntityName(displayName.ToSafeName()),
                version = version
            });
        }

        public ActionResult Publish(string id) {
            var entity = _contentMetadataService.GetEntity(id);
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

            viewModel.FieldLabel = string.IsNullOrWhiteSpace(viewModel.FieldLabel) ? String.Empty : viewModel.FieldLabel.Trim();
            viewModel.FieldName = (viewModel.FieldName ?? viewModel.FieldLabel).ToSafeName();

            if (String.IsNullOrWhiteSpace(viewModel.DisplayName)) {
                ModelState.AddModelError("DisplayName", T("The Display Name name can't be empty.").ToString());
            }

            if (String.IsNullOrWhiteSpace(viewModel.Name)) {
                ModelState.AddModelError("Name", T("The Content Type Id can't be empty.").ToString());
            }

            if (String.IsNullOrWhiteSpace(viewModel.FieldLabel)) {
                ModelState.AddModelError("DisplayName", T("The Field Label name can't be empty.").ToString());
            }

            if (String.IsNullOrWhiteSpace(viewModel.FieldName)) {
                ModelState.AddModelError("Name", T("The Field Name can't be empty.").ToString());
            }

            if (String.IsNullOrWhiteSpace(viewModel.FieldType)) {
                ModelState.AddModelError("Name", T("The FieldType can't be empty.").ToString());
            }

            if (!_contentMetadataService.CheckEntityCreationValid(viewModel.Name, viewModel.DisplayName, viewModel.Settings)) {
                ModelState.AddModelError("Name", T("A type with the same Name or DisplayName already exists.").ToString());
            }

            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                var temp = (from values in ModelState
                    from error in values.Value.Errors
                    select error.ErrorMessage).ToArray();
                return Content(string.Concat(temp));
            }

            _contentMetadataService.CreateEntity(viewModel, this);
            return Json(new {entityName = viewModel.Name});
        }

        public ActionResult Edit(string id) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content type."))) {
                return new HttpUnauthorizedResult();
            }

            var entity = _contentMetadataService.GetEntity(id);

            if (entity == null) {
                return HttpNotFound();
            }
            var viewModel = new EditTypeViewModel {
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                Settings = entity.EntitySetting
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(string id, EditTypeViewModel viewModel) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content type."))) {
                return new HttpUnauthorizedResult();
            }
            var entity = _contentMetadataService.GetDraftEntity(id);

            if (entity == null) {
                return HttpNotFound();
            }
            bool valid = _contentMetadataService.CheckEntityDisplayValid(id, viewModel.DisplayName, viewModel.Settings);
            if (!valid) {
                return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }
            entity.DisplayName = viewModel.DisplayName;
            entity.EntitySetting = viewModel.Settings;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Detail(string id) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content type."))) {
                return new HttpUnauthorizedResult();
            }

            var entity = _contentMetadataService.GetEntity(id);

            if (entity == null) {
                return HttpNotFound();
            }

            var viewModel = new EntityDetailViewModel {
                Id = entity.Id,
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                HasPublished = entity.HasPublished(),
                Settings = entity.EntitySetting
            };

            return View(viewModel);
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return base.TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}