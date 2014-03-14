using System.Linq;
using Coevery.ContentManagement.Handlers;
using Coevery.Core.Projections.Models;
using Coevery.Data;
using Coevery.Localization;

namespace Coevery.Core.Projections.Handlers {
    public class QueryPartHandler : ContentHandler {

        public QueryPartHandler(IRepository<QueryPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));

            T = NullLocalizer.Instance;

            // create a default FilterGroup on creation
            OnPublishing<QueryPart>(CreateFilterGroup);

        }

        public Localizer T { get; set; }

        private static void CreateFilterGroup(PublishContentContext ctx, QueryPart part) {
            if (!part.FilterGroups.Any()) {
                part.FilterGroups.Add(new FilterGroupRecord());
            }
        }
    }
}