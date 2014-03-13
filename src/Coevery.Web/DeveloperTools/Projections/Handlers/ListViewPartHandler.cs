using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.DeveloperTools.Projections.Models;

namespace Coevery.DeveloperTools.Projections.Handlers {
    public class ListViewPartHandler : ContentHandler {
        public ListViewPartHandler(IRepository<ListViewPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}