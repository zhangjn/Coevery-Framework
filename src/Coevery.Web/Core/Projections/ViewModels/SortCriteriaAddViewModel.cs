using System.Collections.Generic;
using Coevery.Core.Projections.Descriptors;
using Coevery.Core.Projections.Descriptors.SortCriterion;

namespace Coevery.Core.Projections.ViewModels {
    public class SortCriterionAddViewModel {
        public int Id { get; set; }
        public IEnumerable<TypeDescriptor<SortCriterionDescriptor>> SortCriteria { get; set; }
    }
}
