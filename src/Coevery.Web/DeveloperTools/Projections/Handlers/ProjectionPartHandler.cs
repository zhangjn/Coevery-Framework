using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.DeveloperTools.Projections.Models;

namespace Coevery.DeveloperTools.Projections.Handlers {
    public class ProjectionPartHandler : ContentHandler {
        public ProjectionPartHandler(IRepository<ProjectionPartRecord> projecRepository) {
            Filters.Add(StorageFilter.For(projecRepository));
        }
    }
}