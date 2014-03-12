using Coevery.DeveloperTools.Entities.Settings;

namespace Coevery.DeveloperTools.Fields.Settings {
    public class BooleanFieldSettings : FieldSettings {
        public BooleanDisplayMode SelectionMode { get; set; }
        public bool DefaultValue { get; set; }

        public BooleanFieldSettings() {
            SelectionMode = BooleanDisplayMode.Checkbox;
        }
    }

    public enum BooleanDisplayMode {
        Checkbox,
        Radiobutton
    }
}
