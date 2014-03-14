using System.Collections.Generic;
using Coevery.Core.Projections.Descriptors;
using Coevery.Core.Projections.Descriptors.Filter;

namespace Coevery.Core.Projections.ViewModels {
    public class FilterAddViewModel {
        public int Id { get; set; }
        public int Group { get; set; }

        public IEnumerable<TypeDescriptor<FilterDescriptor>> Filters { get; set; }
    }
}
