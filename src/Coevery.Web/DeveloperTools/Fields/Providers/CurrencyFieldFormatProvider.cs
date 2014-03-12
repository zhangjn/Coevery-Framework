using Coevery.ContentManagement;
using Coevery.DeveloperTools.Entities.Providers;
using Coevery.DeveloperTools.Fields.Fields;
using Coevery.DeveloperTools.Fields.Settings;

namespace Coevery.DeveloperTools.Fields.Providers {
    public class CurrencyFieldFormatProvider : IContentFieldFormatProvider {
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