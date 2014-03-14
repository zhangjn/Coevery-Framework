using Coevery.Core.Projections.Descriptors.Property;
using Coevery.Events;

namespace Coevery.Core.Projections.Services {
    public interface IPropertyProvider : IEventHandler {
        void Describe(DescribePropertyContext describe);
    }
}