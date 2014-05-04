using System.Linq;
using System.Web.Mvc;
using Coevery.ContentManagement.MetaData.Builders;
using Coevery.Core.Common.Extensions;
using Coevery.DeveloperTools.EntityManagement.Services;
using Coevery.DeveloperTools.FormDesigner.Services;
using Coevery.Localization;

namespace Coevery.DeveloperTools.FormDesigner.Controllers {
    public class AdminController : Controller {
        private readonly ILayoutManager _layoutManager;
        private readonly IEntityMetadataService _entityMetadataService;


        public AdminController(
            ICoeveryServices coeveryServices,
            ILayoutManager layoutManager,
            IEntityMetadataService entityMetadataService) {
            _layoutManager = layoutManager;
            _entityMetadataService = entityMetadataService;
            Services = coeveryServices;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; private set; }
        public Localizer T { get; set; }

        public ActionResult Index(string id, string returnUrl) {
            var entity = _entityMetadataService.GetEntity(id);
            if (entity == null) {
                return Content(T("The \"{0}\" does not exist in the database!", id).Text);
            }

            var typeBuilder = new ContentTypeDefinitionBuilder();
            typeBuilder.Named(entity.Name).DisplayedAs(entity.DisplayName);
            foreach (var pair in entity.EntitySettings) {
                typeBuilder.WithSetting(pair.Key, pair.Value);
            }
            var partBuilder = new ContentPartDefinitionBuilder();
            partBuilder.Named(entity.Name.ToPartName());
            var fieldDefinitions = _entityMetadataService.GetFields(id);
            foreach (var field in fieldDefinitions) {
                string fieldTypeName = field.FieldType;
                partBuilder.WithField(field.Name, fieldBuilder => {
                    fieldBuilder.OfType(fieldTypeName);
                    foreach (var pair in field.Settings) {
                        if (pair.Key == "Storage") {
                            continue;
                        }
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
            viewModel.ContentPart = contentPart;

            return View((object) viewModel);
        }
    }
}