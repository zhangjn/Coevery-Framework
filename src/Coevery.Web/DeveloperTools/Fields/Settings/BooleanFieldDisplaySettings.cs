﻿namespace Coevery.DeveloperTools.Fields.Settings {
    public class BooleanFieldDisplaySettings {
        public string OnLabel { get; set; }
        public string OffLabel { get; set; }
        public bool DefaultValue { get; set; }

        public BooleanFieldDisplaySettings() {
            OnLabel = "Yes";
            OffLabel = "No";
        }
    }
}
