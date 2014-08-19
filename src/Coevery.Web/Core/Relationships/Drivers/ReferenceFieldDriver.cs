using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Relationships.Fields;
using Coevery.Core.Relationships.Models;
using Coevery.Core.Relationships.Settings;
using Coevery.Localization;

namespace Coevery.Core.Relationships.Drivers {
    public class ReferenceFieldDriver : ContentFieldDriver<ReferenceField> {
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionQuery _contentDefinitionQuery;

        private const string TemplateName = "Fields/Reference.Edit";

        public ReferenceFieldDriver(IContentManager contentManager, 
            IContentDefinitionQuery contentDefinitionQuery) {
            _contentManager = contentManager;
            _contentDefinitionQuery = contentDefinitionQuery;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        private static string GetPrefix(ContentField field, ContentPart part) {
            return part.PartDefinition.Name + "." + field.Name;
        }

        private static string GetDifferentiator(ReferenceField field, ContentPart part) {
            return field.Name;
        }

        protected override DriverResult Display(ContentPart part, ReferenceField field, string displayType, dynamic shapeHelper) {
            var settings = field.PartFieldDefinition.Settings.GetModel<ReferenceFieldSettings>();
            int? value = field.Value;
            string title = string.Empty;
            if (value.HasValue && field.ContentItem != null) {
                string partName = settings.ContentTypeName.ToPartName();
                var referenceItem = field.ContentItem.ContentItem;
                var contentPart = referenceItem.Parts.First(x => x.PartDefinition.Name == partName);
                var displayField = contentPart.Fields.First(x => x.Name == settings.DisplayFieldName);
                title = displayField.Storage.Get<dynamic>(null);
            }

            return ContentShape("Fields_Reference", GetDifferentiator(field, part),
                () => shapeHelper.Fields_Reference(DisplayAsLink: settings.DisplayAsLink, ContentField: field, Title: title));
        }

        protected override DriverResult Editor(ContentPart part, ReferenceField field, dynamic shapeHelper) {
            return ContentShape("Fields_Reference_Edit", GetDifferentiator(field, part),
                () => {
                    var settings = field.PartFieldDefinition.Settings.GetModel<ReferenceFieldSettings>();
                    var relatedTypeDefinition = _contentDefinitionQuery.GetTypeDefinition(settings.ContentTypeName);
                    string partName = settings.ContentTypeName.ToPartName();
                    var fieldValue = field.Value;
                    var selectedText = string.Empty;
                    if (fieldValue.HasValue) {
                        var contentItem = _contentManager.Get(fieldValue.Value);
                        var contentPart = contentItem.Parts.First(x => x.PartDefinition.Name == partName);
                        var displayField = contentPart.Fields.First(x => x.Name == settings.DisplayFieldName);
                        selectedText = displayField.Storage.Get<dynamic>(null);
                    }

                    var model = new ReferenceFieldViewModel {
                        ContentId = field.Value,
                        Field = field,
                        SelectedText = selectedText,
                        RelatedEntityDefinition = relatedTypeDefinition
                    };
                    return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: model, Prefix: GetPrefix(field, part));
                });
        }

        protected override DriverResult Editor(ContentPart part, ReferenceField field, IUpdateModel updater, dynamic shapeHelper) {
            var viewModel = new ReferenceFieldViewModel();
            if (updater.TryUpdateModel(viewModel, GetPrefix(field, part), null, null)) {
                var settings = field.PartFieldDefinition.Settings.GetModel<ReferenceFieldSettings>();

                if (settings.IsUnique && viewModel.ContentId.HasValue) {
                    HandleUniqueValue(part, field, viewModel.ContentId, updater);
                }
                if (settings.Required && viewModel.ContentId <= 0) {
                    updater.AddModelError(GetPrefix(field, part), T("The field {0} is mandatory.", T(field.DisplayName)));
                }
                field.Value = viewModel.ContentId;
            }
            return Editor(part, field, shapeHelper);
        }

        protected override void Importing(ContentPart part, ReferenceField field, ImportContentContext context) {
            context.ImportAttribute(field.FieldDefinition.Name + "." + field.Name, "Value", v => field.Value = int.Parse(v), () => field.Value = (int?) null);
        }

        protected override void Exporting(ContentPart part, ReferenceField field, ExportContentContext context) {
            context.Element(field.FieldDefinition.Name + "." + field.Name).SetAttributeValue("Value", field.Value.HasValue ? field.Value.Value.ToString(CultureInfo.InvariantCulture) : String.Empty);
        }

        protected override void Describe(DescribeMembersContext context) {
            context.Member(null, typeof (int?), null, T("The content item id referenced by this field."));
        }

        private void HandleUniqueValue(ContentPart part, ReferenceField field, int? value, IUpdateModel updater) {
            var recordType = part.GetType().GetProperty("Record").PropertyType;
            Action<IAliasFactory> alias = x => x.ContentPartRecord(recordType);
            Action<IHqlExpressionFactory> notCurrentItem = x => x.Not(y => y.Eq("ContentItemRecord", part.Id));
            Action<IHqlExpressionFactory> predicate = x => x.And(notCurrentItem, y => y.Eq(field.Name, value));

            var count = _contentManager.HqlQuery()
                .ForType(part.TypeDefinition.Name)
                .Where(alias, predicate)
                .Count();

            if (count > 0) {
                updater.AddModelError(GetPrefix(field, part), T("The field {0} value must be unique.", T(field.DisplayName)));
            }
        }
    }
}