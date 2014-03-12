﻿using Coevery.DeveloperTools.Entities.Settings;

namespace Coevery.DeveloperTools.Fields.Settings {
    public class CurrencyFieldSettings : FieldSettings {
        public int Length { get; set; }
        public int DecimalPlaces { get; set; }
        public decimal? DefaultValue { get; set; }

        public CurrencyFieldSettings() {
            Length = 18;
            DecimalPlaces = 0;
        }
    }
}