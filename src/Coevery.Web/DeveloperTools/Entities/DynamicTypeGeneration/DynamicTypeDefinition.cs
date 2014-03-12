using System.Collections.Generic;

namespace Coevery.DeveloperTools.Entities.DynamicTypeGeneration {
    public class DynamicTypeDefinition {
        public string Name { get; set; }
        public IEnumerable<DynamicFieldDefinition> Fields { get; set; }
    }
}