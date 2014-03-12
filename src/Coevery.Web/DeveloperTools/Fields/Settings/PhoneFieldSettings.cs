﻿using Coevery.DeveloperTools.Entities.Settings;

namespace Coevery.DeveloperTools.Fields.Settings {
    public class PhoneFieldSettings : FieldSettings {
        public string DefaultValue { get; set; }
        public bool IsUnique { get; set; }

        public PhoneFieldSettings() {
            DefaultValue = null;
        }
    }
}