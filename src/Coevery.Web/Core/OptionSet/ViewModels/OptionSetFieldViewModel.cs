﻿using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.Core.OptionSet.Settings;

namespace Coevery.Core.OptionSet.ViewModels {
    public class OptionSetFieldViewModel {
        public int OptionSetId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public OptionSetFieldSettings Settings { get; set; }
        public IList<OptionItemEntry> OptionItems { get; set; }
        public string Value { get; set; }
    }

    public class OptionItemEntry {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selectable { get; set; }
        public int Weight { get; set; }
        public bool IsChecked { get; set; }
        public int OptionSetId { get; set; }
        public ContentItem ContentItem { get; set; }
    }
}