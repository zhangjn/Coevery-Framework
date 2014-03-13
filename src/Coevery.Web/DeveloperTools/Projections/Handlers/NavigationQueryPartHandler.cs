using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.DeveloperTools.Projections.Models;

namespace Coevery.DeveloperTools.Projections.Handlers {
    public class NavigationQueryPartHandler : ContentHandler {
        public NavigationQueryPartHandler(IRepository<NavigationQueryPartRecord> navigationQueryRepository) {
            Filters.Add(StorageFilter.For(navigationQueryRepository));
        }
    }
}