using System.Collections.Generic;

namespace Coevery.Core.Projections.ViewModels {
    public class GroupAddViewModel {
        public int Id { get; set; }
        public IEnumerable<PropertyEntry> Properties { get; set; }
    }
}
