using Coevery.ContentManagement;
using Coevery.Core.Fields.Fields;

namespace Coevery.Core.Fields.Providers {
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