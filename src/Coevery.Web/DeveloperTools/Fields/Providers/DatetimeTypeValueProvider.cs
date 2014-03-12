using System;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.Entities.Providers;
using Coevery.DeveloperTools.Fields.Fields;

namespace Coevery.DeveloperTools.Fields.Providers {
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