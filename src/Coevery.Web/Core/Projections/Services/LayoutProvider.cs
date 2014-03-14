using Coevery.Core.Projections.Descriptors.Layout;
using Coevery.Events;

namespace Coevery.Core.Projections.Services {
    public interface ILayoutProvider : IEventHandler {
        void Describe(DescribeLayoutContext describe);
    }
}