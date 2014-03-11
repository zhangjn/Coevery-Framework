using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.DeveloperTools.Services;
using Coevery.DeveloperTools.ViewModels;
using Coevery.Localization;
using Coevery.Logging;
using Coevery.UI.Notify;
using Coevery.Utility.Extensions;

namespace Coevery.DeveloperTools.Controllers {
    public class FieldAdminController : Controller, IUpdateModel {
        private readonly IContentDefinitionService _contentDefinitionService;
        private readonly IContentDefinitionEditorEvents _contentDefinitionEditorEvents;
        private readonly IContentMetadataService _contentMetadataService;
        private readonly ISettingService _settingService;

        public FieldAdminController(
            ICoeveryServices coeveryServices,
            ISettingService settingService,
            IContentDefinitionService contentDefinitionService,
            IContentDefinitionEditorEvents contentDefinitionEditorEvents,
            IContentMetadataService contentMetadataService) {
            Services = coeveryServices;
            _settingService = settingService;
            _contentDefinitionService = contentDefinitionService;
            T = NullLocalizer.Instance;
            _contentDefinitionEditorEvents = contentDefinitionEditorEvents;
            _contentMetadataService = contentMetadataService;
        }

        public ICoeveryServices Services { get; private set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult ChooseFieldType(string id) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content part."))) {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new AddFieldViewModel {
                Fields = _contentDefinitionService.GetFields().OrderBy(x => x.TemplateName),
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

            var entity = _contentMetadataService.GetDraftEntity(id);
            if (entity == null) {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Content(string.Format("The entity with name \"{0}\" doesn't exist!", id));
            }

            viewModel.DisplayName = string.IsNullOrWhiteSpace(viewModel.DisplayName)
                ? String.Empty : viewModel.DisplayName.Trim();
            viewModel.Name = (viewModel.Name ?? viewModel.DisplayName).ToSafeName();

            if (String.IsNullOrWhiteSpace(viewModel.DisplayName)) {
                ModelState.AddModelError("DisplayName", T("The Display Name name can't be empty.").ToString());
            }

            if (String.IsNullOrWhiteSpace(viewModel.Name)) {
                ModelState.AddModelError("Name", T("The Technical Name can't be empty.").ToString());
            }

            if (viewModel.Name.ToLower() == "id") {
                ModelState.AddModelError("Name", T("The Field Name can't be any case of 'Id'.").ToString());
            }

            if (!_contentMetadataService.CheckFieldCreationValid(entity, viewModel.Name, viewModel.DisplayName)) {
                ModelState.AddModelError("Name", T("A field with the same name or displayName already exists.").ToString());
            }

            try {
                _contentMetadataService.CreateField(entity, viewModel, this);
                if (!ModelState.IsValid) {
                    Services.TransactionManager.Cancel();
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    var errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                        .Select(m => m.ErrorMessage).ToArray();
                    return Content(string.Concat(errors));
                }
            }
            catch (Exception ex) {
                var message = T("The \"{0}\" field was not added. {1}", viewModel.DisplayName, ex.Message);
                Services.Notifier.Information(message);
                Services.TransactionManager.Cancel();
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message.ToString());
            }

            Services.Notifier.Information(T("The \"{0}\" field has been added.", viewModel.DisplayName));

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
                result = _contentMetadataService.ConstructFieldName(entityName, displayName.ToSafeName()),
                version = version
            });
        }

        public ActionResult EditFields(string id, string fieldName) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content type."))) {
                return new HttpUnauthorizedResult();
            }
            var entity = _contentMetadataService.GetEntity(id);
            if (entity == null) {
                return HttpNotFound();
            }
            var field = entity.FieldMetadataRecords.FirstOrDefault(x => x.Name == fieldName);
            if (field == null) {
                return HttpNotFound();
            }
            var settings = _settingService.ParseSetting(field.Settings);
            var fieldDefinition = new ContentFieldDefinition(field.ContentFieldDefinitionRecord.Name);
            var viewModel = new EditPartFieldViewModel {
                Name = field.Name,
                DisplayName = settings["DisplayName"],
                Settings = settings,
                FieldDefinition = new EditFieldViewModel(fieldDefinition),
                Templates = _contentDefinitionEditorEvents.PartFieldEditor(new ContentPartFieldDefinition(fieldDefinition, field.Name, settings))
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("EditFields")]
        public ActionResult EditFieldsPost(EditPartFieldViewModel viewModel, string id) {
            if (!Services.Authorizer.Authorize(Permissions.EditContentTypes, T("Not allowed to edit a content type."))) {
                return new HttpUnauthorizedResult();
            }

            var entity = _contentMetadataService.GetDraftEntity(id);
            if (viewModel == null || entity == null) {
                return HttpNotFound();
            }
            var field = entity.FieldMetadataRecords.FirstOrDefault(x => x.Name == viewModel.Name);
            if (field == null) {
                return HttpNotFound();
            }

            // prevent null reference exception in validation
            viewModel.DisplayName = viewModel.DisplayName ?? String.Empty;

            // remove extra spaces
            viewModel.DisplayName = viewModel.DisplayName.Trim();

            if (String.IsNullOrWhiteSpace(viewModel.DisplayName)) {
                ModelState.AddModelError("DisplayName", T("The Display Name name can't be empty.").ToString());
            }

            bool displayNameExist = entity.FieldMetadataRecords.Any(t => {
                string displayName = _settingService.ParseSetting(t.Settings)["DisplayName"];
                return t.Name != viewModel.Name && String.Equals(displayName.Trim(), viewModel.DisplayName.Trim(), StringComparison.OrdinalIgnoreCase);
            });
            if (displayNameExist) {
                ModelState.AddModelError("DisplayName", T("A field with the same Display Name already exists.").ToString());
            }
            _contentMetadataService.UpdateField(field, viewModel.DisplayName, this);
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

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return base.TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}