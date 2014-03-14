using System.Collections.Generic;
using Coevery.Core.Projections.Descriptors;
using Coevery.Core.Projections.Descriptors.Layout;

namespace Coevery.Core.Projections.ViewModels {
    public class LayoutAddViewModel {
        public int Id { get; set; }
        public IEnumerable<TypeDescriptor<LayoutDescriptor>> Layouts { get; set; }
    }
}
