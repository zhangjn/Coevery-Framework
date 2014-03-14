using System.Collections.Generic;
using Coevery.Core.Projections.Descriptors;
using Coevery.Core.Projections.Descriptors.Property;

namespace Coevery.Core.Projections.ViewModels {
    public class PropertyAddViewModel {
        public int Id { get; set; }
        public IEnumerable<TypeDescriptor<PropertyDescriptor>> Properties { get; set; }
    }
}
