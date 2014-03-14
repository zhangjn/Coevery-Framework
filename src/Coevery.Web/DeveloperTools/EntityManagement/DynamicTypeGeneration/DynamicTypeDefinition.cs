using System.Collections.Generic;

namespace Coevery.DeveloperTools.EntityManagement.DynamicTypeGeneration {
    public class DynamicTypeDefinition {
        public string Name { get; set; }
        public IEnumerable<DynamicFieldDefinition> Fields { get; set; }
    }
}