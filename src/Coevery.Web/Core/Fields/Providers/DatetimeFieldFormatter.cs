using Coevery.ContentManagement;
using Coevery.Core.Fields.Fields;
using Coevery.Core.Projections.PropertyEditors;

namespace Coevery.Core.Fields.Providers {
    public class DatetimeFieldFormatter : IContentFieldFormatter {
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