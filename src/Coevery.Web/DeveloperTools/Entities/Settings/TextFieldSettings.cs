namespace Coevery.DeveloperTools.Settings {
    public class TextFieldSettings : FieldSettings {
        public int MaxLength { get; set; }
        public string PlaceHolderText { get; set; }
        public bool IsDisplayField { get; set; }
        public bool IsUnique { get; set; }

        public TextFieldSettings() {
            MaxLength = 255;
        }
    }
}
