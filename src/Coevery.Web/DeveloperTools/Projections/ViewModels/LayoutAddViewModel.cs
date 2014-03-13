using System.Collections.Generic;
using Coevery.DeveloperTools.Projections.Descriptors;
using Coevery.DeveloperTools.Projections.Descriptors.Layout;

namespace Coevery.DeveloperTools.Projections.ViewModels {
    public class LayoutAddViewModel {
        public int Id { get; set; }
        public IEnumerable<TypeDescriptor<LayoutDescriptor>> Layouts { get; set; }
    }
}
