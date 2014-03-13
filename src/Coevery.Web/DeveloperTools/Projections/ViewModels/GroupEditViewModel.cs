using Coevery.DeveloperTools.Projections.Descriptors.Property;

namespace Coevery.DeveloperTools.Projections.ViewModels {
    public class GroupEditViewModel {
        public int Id { get; set; }
        public PropertyDescriptor Property { get; set; }
        public string Sort { get; set; }
    }
}
