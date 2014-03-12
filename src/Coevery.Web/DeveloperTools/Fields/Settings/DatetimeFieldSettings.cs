using System;
using Coevery.DeveloperTools.Entities.Settings;

namespace Coevery.DeveloperTools.Fields.Settings {
    public class DatetimeFieldSettings : FieldSettings {
        public DateTime? DefaultValue { get; set; }

        public DatetimeFieldSettings() {
            DefaultValue = null;
        }
    }
}