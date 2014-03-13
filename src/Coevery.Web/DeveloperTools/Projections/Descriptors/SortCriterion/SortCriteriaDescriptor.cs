﻿using System;
using Coevery.Localization;

namespace Coevery.DeveloperTools.Projections.Descriptors.SortCriterion {
    public class SortCriterionDescriptor {
        public string Category { get; set; }
        public string Type { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public Action<SortCriterionContext> Sort { get; set; }
        public string Form { get; set; }
        public Func<SortCriterionContext, LocalizedString> Display { get; set; }
    }
}