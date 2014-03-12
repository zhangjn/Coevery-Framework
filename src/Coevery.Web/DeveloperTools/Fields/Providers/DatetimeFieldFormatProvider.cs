using Coevery.ContentManagement;
using Coevery.DeveloperTools.Entities.Providers;
using Coevery.DeveloperTools.Fields.Fields;

namespace Coevery.DeveloperTools.Fields.Providers {
    public class DatetimeFieldFormatProvider : IContentFieldFormatProvider {
        public void SetFormat(ContentField field, dynamic formState) {
            var datetimeField = field as DatetimeField;
            if (datetimeField != null) {
                if (formState.Format == null) {
                    formState.Format = "g";
                }
            }
        }
    }
}