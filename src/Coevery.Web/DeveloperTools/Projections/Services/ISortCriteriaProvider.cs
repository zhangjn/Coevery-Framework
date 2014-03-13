using Coevery.DeveloperTools.Projections.Descriptors.SortCriterion;
using Coevery.Events;

namespace Coevery.DeveloperTools.Projections.Services {
    public interface ISortCriterionProvider : IEventHandler {
        void Describe(DescribeSortCriterionContext describe);
    }
}