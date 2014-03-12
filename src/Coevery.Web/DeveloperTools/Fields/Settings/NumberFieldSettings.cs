using Coevery.DeveloperTools.Entities.Settings;

namespace Coevery.DeveloperTools.Fields.Settings {
    public class NumberFieldSettings : FieldSettings {
        public int Length { get; set; }
        public int DecimalPlaces { get; set; }
        public double? DefaultValue { get; set; }

        public NumberFieldSettings() {
            Length = 18;
            DecimalPlaces = 0;
        }
    }
}