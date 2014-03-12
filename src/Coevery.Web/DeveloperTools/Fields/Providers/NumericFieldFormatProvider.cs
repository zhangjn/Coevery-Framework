using Coevery.ContentManagement;
using Coevery.DeveloperTools.Entities.Providers;
using Coevery.DeveloperTools.Fields.Fields;
using Coevery.DeveloperTools.Fields.Settings;

namespace Coevery.DeveloperTools.Fields.Providers {
    public class NumericFieldFormatProvider : IContentFieldFormatProvider {

        public void SetFormat(ContentField field, dynamic formState) {
            var numberField = field as NumberField;
            if (numberField != null) {
                var settings = numberField.PartFieldDefinition.Settings.GetModel<NumberFieldSettings>();
                if (formState.Format == null) {
                    formState.Format = "F" + settings.DecimalPlaces;
                }
            }
        }
    }
}