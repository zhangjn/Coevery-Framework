using System.Collections.Generic;
using Coevery.DeveloperTools.Projections.Descriptors;
using Coevery.DeveloperTools.Projections.Descriptors.Property;

namespace Coevery.DeveloperTools.Projections.ViewModels {
    public class PropertyAddViewModel {
        public int Id { get; set; }
        public IEnumerable<TypeDescriptor<PropertyDescriptor>> Properties { get; set; }
    }
}
