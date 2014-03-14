using Coevery.ContentManagement;
using Coevery.Core.Fields.Fields;
using Coevery.Core.Fields.Settings;
using Coevery.Core.Projections.PropertyEditors;

namespace Coevery.Core.Fields.Projections {
    public class CurrencyFieldFormatter : IContentFieldFormatter {
        public void SetFormat(ContentField field, dynamic formState) {
            var currencyField = field as CurrencyField;
            if (currencyField != null) {
                var settings = currencyField.PartFieldDefinition.Settings.GetModel<CurrencyFieldSettings>();
                if (formState.Format == null) {
                    formState.Format = "C" + settings.DecimalPlaces;
                }
            }
        }
    }
}