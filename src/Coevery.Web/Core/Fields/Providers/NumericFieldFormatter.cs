using Coevery.ContentManagement;
using Coevery.Core.Fields.Fields;
using Coevery.Core.Fields.Settings;
using Coevery.Core.Projections.PropertyEditors;

namespace Coevery.Core.Fields.Providers {
    public class NumericFieldFormatter : IContentFieldFormatter {

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