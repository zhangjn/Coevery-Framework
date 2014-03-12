using System;
using Coevery.DeveloperTools.Entities.Settings;

namespace Coevery.DeveloperTools.Fields.Settings {
    public class DateFieldSettings : FieldSettings {
        public DateTime? DefaultValue { get; set; }

        public DateFieldSettings() {
            DefaultValue = null;
        }
    }
}