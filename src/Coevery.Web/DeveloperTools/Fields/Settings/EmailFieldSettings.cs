using Coevery.DeveloperTools.Entities.Settings;

namespace Coevery.DeveloperTools.Fields.Settings {
    public class EmailFieldSettings : FieldSettings {
        public string DefaultValue { get; set; }
        public bool IsUnique { get; set; }

        public EmailFieldSettings() {
            DefaultValue = null;
        }
    }
}