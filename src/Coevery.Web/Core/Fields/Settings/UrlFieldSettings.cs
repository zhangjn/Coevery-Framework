namespace Coevery.Core.Fields.Settings {
    public class UrlFieldSettings : FieldSettings {
        public string DefaultValue { get; set; }
        public bool IsUnique { get; set; }

        public UrlFieldSettings() {
            DefaultValue = null;
        }
    }
}