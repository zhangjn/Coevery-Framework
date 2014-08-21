using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.ContentManagement.Handlers;
using Coevery.Core.OptionSet.Fields;
using Coevery.Core.OptionSet.Helpers;
using Coevery.Core.OptionSet.Services;
using Coevery.Core.OptionSet.Settings;
using Coevery.Core.OptionSet.ViewModels;
using Coevery.Localization;
using JetBrains.Annotations;

namespace Coevery.Core.OptionSet.Drivers {
    [UsedImplicitly]
    public class OptionSetFieldDriver : ContentFieldDriver<OptionSetField> {
        private readonly IOptionSetService _optionSetService;
        public ICoeveryServices Services { get; set; }

        public OptionSetFieldDriver(
            ICoeveryServices services,
            IOptionSetService optionSetService) {
            _optionSetService = optionSetService;
            Services = services;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        private static string GetPrefix(ContentField field, ContentPart part) {
            return part.PartDefinition.Name + "." + field.Name;
        }

        private static string GetDifferentiator(OptionSetField field, ContentPart part) {
            return field.Name;
        }

        protected override DriverResult Display(ContentPart part, OptionSetField field, string displayType, dynamic shapeHelper) {
            return ContentShape("Fields_OptionSetField", GetDifferentiator(field, part),
                () => {
                    var settings = field.PartFieldDefinition.Settings.GetModel<OptionSetFieldSettings>();

                    return shapeHelper.Fields_OptionSetField(
                        ContentField: field,
                        Settings: settings);
                });
        }

        protected override DriverResult Editor(ContentPart part, OptionSetField field, dynamic shapeHelper) {
            return ContentShape("Fields_OptionSetField_Edit", GetDifferentiator(field, part), () => {
                var settings = field.PartFieldDefinition.Settings.GetModel<OptionSetFieldSettings>();

                var optionSet = _optionSetService.GetOptionSet(settings.OptionSetId);
                var optionItems = optionSet != null
                    ? _optionSetService.GetOptionItems(optionSet.Id).Where(t => !string.IsNullOrWhiteSpace(t.Name)).Select(t => t.CreateTermEntry()).ToList()
                    : new List<OptionItemEntry>(0);

                string value = field.Value != null ? field.Value.ToString() : string.Empty;
                optionItems.ForEach(t => t.IsChecked = t.Name == value);

                var viewModel = new OptionSetFieldViewModel {
                    Name = field.Name,
                    DisplayName = field.DisplayName,
                    OptionItems = optionItems,
                    Settings = settings,
                    Value = value,
                    OptionSetId = optionSet != null ? optionSet.Id : 0
                };

                return shapeHelper.EditorTemplate(TemplateName: "Fields/OptionSetField", Model: viewModel, Prefix: GetPrefix(field, part));
            });
        }

        protected override DriverResult Editor(ContentPart part, OptionSetField field, IUpdateModel updater, dynamic shapeHelper) {
            var viewModel = new OptionSetFieldViewModel {OptionItems = new List<OptionItemEntry>()};

            if (updater.TryUpdateModel(viewModel, GetPrefix(field, part), null, null)) {
                bool hasValue = !string.IsNullOrWhiteSpace(viewModel.Value);
                object value = null;
                if (hasValue) {
                    var property = part.GetType().GetProperty("Record").PropertyType.GetProperty(field.Name);
                    if (property != null) {
                        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        if (propertyType.IsEnum && Enum.IsDefined(propertyType, viewModel.Value)) {
                            value = Enum.Parse(propertyType, viewModel.Value);
                        }
                    }
                }

                field.Value = value;
                var settings = field.PartFieldDefinition.Settings.GetModel<OptionSetFieldSettings>();
                bool continued = true;
                if (settings.ListMode == ListMode.Dropdown && settings.IsUnique && hasValue) {
                    continued = HandleUniqueValue(part, field, updater);
                }
                if (continued && settings.Required && !hasValue) {
                    updater.AddModelError(GetPrefix(field, part), T("The field {0} is mandatory.", T(field.DisplayName)));
                }
            }

            return Editor(part, field, shapeHelper);
        }

        protected override void Exporting(ContentPart part, OptionSetField field, ExportContentContext context) {
            context.Element(field.FieldDefinition.Name + "." + field.Name).SetAttributeValue("Value", field.Value);
        }

        protected override void Importing(ContentPart part, OptionSetField field, ImportContentContext context) {
            context.ImportAttribute(field.FieldDefinition.Name + "." + field.Name, "Value", v => field.Value = v);
        }

        protected override void Describe(DescribeMembersContext context) {
            context.Member(null, typeof (string), null, T("The option value of the field."));
        }

        private bool HandleUniqueValue(ContentPart part, OptionSetField field, IUpdateModel updater) {
            var recordType = part.GetType().GetProperty("Record").PropertyType;
            Action<IAliasFactory> alias = x => x.ContentPartRecord(recordType);
            Action<IHqlExpressionFactory> notCurrentItem = x => x.Not(y => y.Eq("ContentItemRecord.Id", part.Id));
            Action<IHqlExpressionFactory> predicate = x => x.And(notCurrentItem, y => y.Eq(field.Name, field.Value));

            var count = Services.ContentManager.HqlQuery()
                .ForType(part.TypeDefinition.Name)
                .Where(alias, predicate)
                .Count();

            if (count > 0) {
                updater.AddModelError(GetPrefix(field, part), T("The field {0} value must be unique.", T(field.DisplayName)));
                return false;
            }
            return true;
        }
    }
}