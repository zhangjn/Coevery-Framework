using System.Collections.Generic;

namespace Coevery.DeveloperTools.DynamicTypeGeneration {
    public class DynamicTypeDefinition {
        public string Name { get; set; }
        public IEnumerable<DynamicFieldDefinition> Fields { get; set; }
    }
}