﻿using System;
using System.Collections.Generic;

namespace Coevery.DeveloperTools.CodeGeneration.Services {
    public class DynamicDefinition {
        public string Name { get; set; }
        public IEnumerable<DynamicFieldDefinition> Fields { get; set; }
    }

    public class DynamicFieldDefinition {
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}