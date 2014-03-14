using Coevery.ContentManagement;
using Coevery.Core.Fields.Fields;
using Coevery.Core.Projections.PropertyEditors;

namespace Coevery.Core.Fields.Projections {
    public class DateFieldFormatter : IContentFieldFormatter {
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