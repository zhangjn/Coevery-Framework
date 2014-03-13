using System;

namespace Coevery.Core.Fields.Settings {
    public class DateFieldSettings : FieldSettings {
        public DateTime? DefaultValue { get; set; }

        public DateFieldSettings() {
            DefaultValue = null;
        }
    }
}