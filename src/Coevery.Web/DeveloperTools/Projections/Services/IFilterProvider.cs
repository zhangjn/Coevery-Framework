using Coevery.DeveloperTools.Projections.Descriptors.Filter;
using Coevery.Events;

namespace Coevery.DeveloperTools.Projections.Services {
    public interface IFilterProvider : IEventHandler {
        void Describe(DescribeFilterContext describe);
    }
}