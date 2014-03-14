using Coevery.Core.Projections.Descriptors.Filter;
using Coevery.Events;

namespace Coevery.Core.Projections.Services {
    public interface IFilterProvider : IEventHandler {
        void Describe(DescribeFilterContext describe);
    }
}