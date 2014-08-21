﻿using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.ViewModels;

namespace Coevery.Core.Fields.Settings {
    public class CurrencyFieldEditorEvents : FieldEditorEvents {
        public override IEnumerable<TemplateViewModel> FieldTypeDescriptor() {
            var model = string.Empty;
            yield return DisplayTemplate(model, "Currency", null);
        }

        public override void UpdateFieldSettings(string fieldType, string fieldName, SettingsDictionary settingsDictionary, IUpdateModel updateModel) {
            if (fieldType != "CurrencyField") {
                return;
            }
            var model = new CurrencyFieldSettings();
            if (updateModel.TryUpdateModel(model, "CurrencyFieldSettings", null, null)) {
                UpdateSettings(model, settingsDictionary, "CurrencyFieldSettings");
                settingsDictionary["CurrencyFieldSettings.Length"] = model.Length.ToString("D");
                settingsDictionary["CurrencyFieldSettings.DecimalPlaces"] = model.DecimalPlaces.ToString("D");
                settingsDictionary["CurrencyFieldSettings.DefaultValue"] = model.DefaultValue.ToString();
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition) {
            if (definition.FieldDefinition.Name == "CurrencyField"
                || definition.FieldDefinition.Name == "CurrencyFieldCreate") {
                var model = definition.Settings.GetModel<CurrencyFieldSettings>();
                yield return DefinitionTemplate(model);
            }
        }
    }
}