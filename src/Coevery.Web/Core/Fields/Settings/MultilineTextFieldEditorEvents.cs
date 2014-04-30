using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.ViewModels;

namespace Coevery.Core.Fields.Settings {
    public class MultilineTextFieldEditorEvents : FieldEditorEvents {
        public override IEnumerable<TemplateViewModel> FieldTypeDescriptor() {
            var model = string.Empty;
            yield return DisplayTemplate(model, "MultilineText", null);
        }

        public override void UpdateFieldSettings(string fieldType, string fieldName, SettingsDictionary settingsDictionary, IUpdateModel updateModel) {
            if (fieldType != "MultilineTextField") {
                return;
            }
            var model = new MultilineTextFieldSettings();
            if (updateModel.TryUpdateModel(model, "MultilineTextFieldSettings", null, null)) {
                UpdateSettings(model, settingsDictionary, "MultilineTextFieldSettings");
                settingsDictionary["MultilineTextFieldSettings.RowNumber"] = model.RowNumber.ToString();
                settingsDictionary["MultilineTextFieldSettings.MaxLength"] = model.MaxLength.ToString();
                settingsDictionary["MultilineTextFieldSettings.PlaceHolderText"] = model.PlaceHolderText;
                settingsDictionary["MultilineTextFieldSettings.IsUnique"] = model.IsUnique.ToString();
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition) {
            if (definition.FieldDefinition.Name == "MultilineTextField") {
                var model = definition.Settings.GetModel<MultilineTextFieldSettings>();
                yield return DefinitionTemplate(model);
            }
        }
    }
}