﻿using Coevery.DeveloperTools.Entities.Settings;

namespace Coevery.DeveloperTools.Fields.Settings {
    public class MultilineTextFieldSettings : FieldSettings {
        public int MaxLength { get; set; }
        public string PlaceHolderText { get; set; }
        public int RowNumber { get; set; }
        public bool IsUnique { get; set; }

        public MultilineTextFieldSettings() {
            RowNumber = 3;
            MaxLength = 255;
        }
    }
}
