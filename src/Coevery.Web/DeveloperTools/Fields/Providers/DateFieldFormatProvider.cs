using Coevery.ContentManagement;
using Coevery.DeveloperTools.Entities.Providers;
using Coevery.DeveloperTools.Fields.Fields;

namespace Coevery.DeveloperTools.Fields.Providers {
    public class DateFieldFormatProvider : IContentFieldFormatProvider {
        public void SetFormat(ContentField field, dynamic formState) {
            var dateField = field as DateField;
            if (dateField != null) {
                if (formState.Format == null) {
                    formState.Format = "d";
                }
            }
        }
    }
}