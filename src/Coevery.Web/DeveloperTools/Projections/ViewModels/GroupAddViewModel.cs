using System.Collections.Generic;

namespace Coevery.DeveloperTools.Projections.ViewModels {
    public class GroupAddViewModel {
        public int Id { get; set; }
        public IEnumerable<PropertyEntry> Properties { get; set; }
    }
}
