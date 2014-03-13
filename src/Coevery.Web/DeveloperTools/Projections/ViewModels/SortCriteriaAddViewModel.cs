using System.Collections.Generic;
using Coevery.DeveloperTools.Projections.Descriptors;
using Coevery.DeveloperTools.Projections.Descriptors.SortCriterion;

namespace Coevery.DeveloperTools.Projections.ViewModels {
    public class SortCriterionAddViewModel {
        public int Id { get; set; }
        public IEnumerable<TypeDescriptor<SortCriterionDescriptor>> SortCriteria { get; set; }
    }
}
