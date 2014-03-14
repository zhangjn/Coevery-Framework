using System;
using Coevery.ContentManagement;
using Coevery.Core.Fields.Fields;
using Coevery.Core.Projections.Providers.Properties;

namespace Coevery.Core.Fields.Projections {
    public class DateFieldValueProvider : ContentFieldValueProvider<DateField> {
        public override object GetValue(ContentItem contentItem, ContentField field) {
            var value = field.Storage.Get<DateTime?>(field.Name);
            if (!value.HasValue) {
                return null;
            }
            return value.Value.ToLocalTime();
        }
    }
    public class DatetimeFieldValueProvider : ContentFieldValueProvider<DatetimeField> {
        public override object GetValue(ContentItem contentItem, ContentField field) {
            var value = field.Storage.Get<DateTime?>(field.Name);
            if (!value.HasValue) {
                return null;
            }
            return value.Value.ToLocalTime();
        }
    }
}