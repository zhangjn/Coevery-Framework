using System;

namespace Coevery.Core.Fields.Settings {
    public class DatetimeFieldSettings : FieldSettings {
        public DateTime? DefaultValue { get; set; }

        public DatetimeFieldSettings() {
            DefaultValue = null;
        }
    }
}