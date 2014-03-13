using Coevery.DeveloperTools.Projections.Descriptors.Layout;
using Coevery.Events;

namespace Coevery.DeveloperTools.Projections.Services {
    public interface ILayoutProvider : IEventHandler {
        void Describe(DescribeLayoutContext describe);
    }
}