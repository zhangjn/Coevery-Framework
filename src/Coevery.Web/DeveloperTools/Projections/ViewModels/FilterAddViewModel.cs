using System.Collections.Generic;
using Coevery.DeveloperTools.Projections.Descriptors;
using Coevery.DeveloperTools.Projections.Descriptors.Filter;

namespace Coevery.DeveloperTools.Projections.ViewModels {
    public class FilterAddViewModel {
        public int Id { get; set; }
        public int Group { get; set; }

        public IEnumerable<TypeDescriptor<FilterDescriptor>> Filters { get; set; }
    }
}
