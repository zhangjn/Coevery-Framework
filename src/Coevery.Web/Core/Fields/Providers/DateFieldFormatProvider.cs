using Coevery.ContentManagement;
using Coevery.Core.Fields.Fields;

namespace Coevery.Core.Fields.Providers {
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