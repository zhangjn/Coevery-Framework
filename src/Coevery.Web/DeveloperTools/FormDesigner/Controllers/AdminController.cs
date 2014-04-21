using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Builders;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Entities.Models;
using Coevery.DeveloperTools.FormDesigner.Services;
using Coevery.Localization;
using Newtonsoft.Json;

namespace Coevery.DeveloperTools.FormDesigner.Controllers {
    public class AdminController : Controller {
        private readonly ILayoutManager _layoutManager;
        private readonly ISettingsFormatter _settingsFormatter;

        public AdminController(
            ICoeveryServices coeveryServices,
            ILayoutManager layoutManager, 
            ISettingsFormatter settingsFormatter) {
            _layoutManager = layoutManager;
            _settingsFormatter = settingsFormatter;
            Services = coeveryServices;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; private set; }
        public Localizer T { get; set; }

        public ActionResult Index(string id, string returnUrl) {
            if (string.IsNullOrEmpty(id)) {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var entityMetadataPart = Services.ContentManager
                .Query<EntityMetadataPart>(VersionOptions.Latest, "EntityMetadata")
                .List().FirstOrDefault(x => x.Name == id);
            if (entityMetadataPart == null) {
                return Content(T("The \"{0}\" does not exist in the database!", id).Text);
            }

            var typeBuilder = new ContentTypeDefinitionBuilder();
            typeBuilder.Named(entityMetadataPart.Name).DisplayedAs(entityMetadataPart.DisplayName);
            foreach (var pair in entityMetadataPart.EntitySetting) {
                typeBuilder.WithSetting(pair.Key, pair.Value);
            }
            var partBuilder = new ContentPartDefinitionBuilder();
            partBuilder.Named(entityMetadataPart.Name.ToPartName());
            foreach (var field in entityMetadataPart.FieldMetadataRecords) {
                string fieldTypeName = field.ContentFieldDefinitionRecord.Name;
                var settings = _settingsFormatter.Parse(field.Settings);
                partBuilder.WithField(field.Name, fieldBuilder => {
                    fieldBuilder.OfType(fieldTypeName);
                    foreach (var pair in settings) {
                        fieldBuilder.WithSetting(pair.Key, pair.Value);
                    }
                });
            }
            var partDefinition = partBuilder.Build();
            typeBuilder.WithPart(partDefinition, cfg => { });

            var contentTypeDefinition = typeBuilder.Build();
            var contentItem = Services.ContentManager.New(id, contentTypeDefinition);
            var contentPart = contentItem.Parts.First(x => x.PartDefinition.Name == partDefinition.Name);
            var viewModel = Services.New.ViewModel();
            viewModel.Layout = _layoutManager.GetLayout(contentTypeDefinition);
            viewModel.DisplayName = contentItem.TypeDefinition.DisplayName;
            viewModel.ContentPart = contentPart;

            return View((object) viewModel);
        }
    }
}