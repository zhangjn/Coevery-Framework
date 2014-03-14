using Coevery.Core.Projections.Descriptors.Property;

namespace Coevery.Core.Projections.ViewModels {
    public class GroupEditViewModel {
        public int Id { get; set; }
        public PropertyDescriptor Property { get; set; }
        public string Sort { get; set; }
    }
}
