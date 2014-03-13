using Coevery.ContentManagement;
using Coevery.DeveloperTools.Projections.Models;
using Coevery.Events;

namespace Coevery.DeveloperTools.Projections.Services {
    public interface IQueryCriteriaProvider : IEventHandler {
        void Apply(QueryContext context);
    }

    public class QueryContext {
        public QueryPartRecord QueryPartRecord { get; set; }
        public IHqlQuery Query { get; set; }
        public string ContentTypeName { get; set; }
    }
}