﻿using System.Collections.Generic;
using Coevery.ContentManagement;

namespace Coevery.DeveloperTools.Projections.Descriptors.Filter {
    public class FilterContext {
        public FilterContext() {
            Tokens = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Tokens { get; set; }
        public dynamic State { get; set; }
        public IHqlQuery Query { get; set; }
    }
}