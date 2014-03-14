using Coevery.Core.Projections.Descriptors.SortCriterion;
using Coevery.Events;

namespace Coevery.Core.Projections.Services {
    public interface ISortCriterionProvider : IEventHandler {
        void Describe(DescribeSortCriterionContext describe);
    }
}