using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.DeveloperTools.ListViewDesigner.Models;

namespace Coevery.DeveloperTools.ListViewDesigner.Handlers {
    public class GridInfoPartHandler : ContentHandler {
        public GridInfoPartHandler(IRepository<GridInfoPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}