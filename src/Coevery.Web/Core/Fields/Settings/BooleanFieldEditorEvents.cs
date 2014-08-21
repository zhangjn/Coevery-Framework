using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.ViewModels;

namespace Coevery.Core.Fields.Settings {
    public class BooleanFieldEditorEvents : FieldEditorEvents {
        public override IEnumerable<TemplateViewModel> FieldTypeDescriptor() {
            var model = string.Empty;
            yield return DisplayTemplate(model, "Boolean", null);
        }

        public override void UpdateFieldSettings(string fieldType, string fieldName, SettingsDictionary settingsDictionary, IUpdateModel updateModel) {
            if (fieldType != "BooleanField") {
                return;
            }
            var model = new BooleanFieldSettings();
            if (updateModel.TryUpdateModel(model, "BooleanFieldSettings", null, null)) {
                UpdateSettings(model, settingsDictionary, "BooleanFieldSettings");
                settingsDictionary["BooleanFieldSettings.SelectionMode"] = model.SelectionMode.ToString();
                settingsDictionary["BooleanFieldSettings.DefaultValue"] = model.DefaultValue.ToString();
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition) {
            if (definition.FieldDefinition.Name == "BooleanField"
                || definition.FieldDefinition.Name == "BooleanFieldCreate") {
                var model = definition.Settings.GetModel<BooleanFieldSettings>();
                yield return DefinitionTemplate(model);
            }
            else if (definition.FieldDefinition.Name == "BooleanFieldDisplay") {
                var model = definition.Settings.GetModel<BooleanFieldDisplaySettings>();
                yield return DefinitionTemplate(model);
            }
        }
    }
}