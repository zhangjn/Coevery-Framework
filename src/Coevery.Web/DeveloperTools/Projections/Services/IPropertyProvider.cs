using Coevery.DeveloperTools.Projections.Descriptors.Property;
using Coevery.Events;

namespace Coevery.DeveloperTools.Projections.Services {
    public interface IPropertyProvider : IEventHandler {
        void Describe(DescribePropertyContext describe);
    }
}