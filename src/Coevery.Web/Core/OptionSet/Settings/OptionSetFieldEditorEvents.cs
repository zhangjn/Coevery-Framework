﻿using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Builders;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.ViewModels;
using Coevery.Core.Fields.Settings;
using Coevery.Core.OptionSet.Models;
using Coevery.Core.OptionSet.Services;
using Coevery.Localization;

namespace Coevery.Core.OptionSet.Settings {
    public class OptionSetFieldEditorEvents : FieldEditorEvents {
        private readonly IContentManager _contentManager;
        private readonly IOptionSetService _optionSetService;

        public OptionSetFieldEditorEvents(
            IContentManager contentManager,
            IOptionSetService optionSetService) {
            _optionSetService = optionSetService;
            _contentManager = contentManager;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override IEnumerable<TemplateViewModel> FieldTypeDescriptor() {
            var model = string.Empty;
            yield return DisplayTemplate(model, "OptionSet", null);
        }

        public override void UpdateFieldSettings(string fieldType, string fieldName, SettingsDictionary settingsDictionary, IUpdateModel updateModel) {
            if (fieldType != "OptionSetField") {
                return;
            }
            var model = new OptionSetFieldSettings();
            if (updateModel.TryUpdateModel(model, "OptionSetFieldSettings", null, null)) {
                UpdateSettings(model, settingsDictionary, "OptionSetFieldSettings");
                if (model.OptionSetId == 0) {
                    model.OptionSetId = CreateOptionSetPart(fieldName, model);
                    if (model.OptionSetId <= 0) {
                        updateModel.AddModelError("OptionSet", T("No items inputted"));
                        return;
                    }
                }
                settingsDictionary["OptionSetFieldSettings.OptionSetId"] = model.OptionSetId.ToString("D");
                settingsDictionary["OptionSetFieldSettings.ListMode"] = model.ListMode.ToString();
                settingsDictionary["OptionSetFieldSettings.IsUnique"] = model.IsUnique.ToString();
            }
        }

        public override void UpdateFieldSettings(ContentPartFieldDefinitionBuilder builder, SettingsDictionary settingsDictionary) {
            if (builder.FieldType != "OptionSetField") {
                return;
            }

            var model = settingsDictionary.TryGetModel<OptionSetFieldSettings>();
            if (model != null) {
                UpdateSettings(model, builder, "OptionSetFieldSettings");
                builder.WithSetting("OptionSetFieldSettings.OptionSetId", model.OptionSetId.ToString());
                builder.WithSetting("OptionSetFieldSettings.ListMode", model.ListMode.ToString());
                builder.WithSetting("OptionSetFieldSettings.IsUnique", model.IsUnique.ToString());
            }
        }

        public override void FieldDeleted(string fieldType, string fieldName, SettingsDictionary settingsDictionary) {
            if (fieldType != "OptionSetField") {
                return;
            }
            var model = settingsDictionary.TryGetModel<OptionSetFieldSettings>();
            if (model != null) {
                var optionSet = _optionSetService.GetOptionSet(model.OptionSetId);
                if (optionSet != null) {
                    _optionSetService.DeleteOptionSet(optionSet);
                }
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition) {
            if (definition.FieldDefinition.Name == "OptionSetField") {
                var model = definition.Settings.GetModel<OptionSetFieldSettings>();
                if (model.OptionSetId == 0) {
                    yield return DefinitionTemplate(model);
                }
                else {
                    var optionItems = _contentManager.Query<OptionItemPart, OptionItemPartRecord>()
                        .Where(x => x.OptionSetId == model.OptionSetId)
                        .List();
                    var options = optionItems.Aggregate(string.Empty, (current, next) =>
                        string.IsNullOrEmpty(current) ? next.Name : current + System.Environment.NewLine + next.Name);
                    model.Options = options;
                    var templateName = model.GetType().Name + ".Edit";
                    yield return DefinitionTemplate(model, templateName, model.GetType().Name);
                }
            }
        }

        private int CreateOptionSetPart(string name, OptionSetFieldSettings model) {
            var options = (!String.IsNullOrWhiteSpace(model.Options)) ?
                model.Options.Split(new[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) : null;

            if (options == null) {
                return -1;
            }

            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
            optionSetPart.Name = name;

            _contentManager.Create(optionSetPart, VersionOptions.Published);

            foreach (var option in options) {
                var term = _contentManager.New<OptionItemPart>("OptionItem");
                term.OptionSetId = optionSetPart.Id;
                term.Name = option;
                _contentManager.Create(term, VersionOptions.Published);
            }
            return optionSetPart.Id;
        }
    }
}