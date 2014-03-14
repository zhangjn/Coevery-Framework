using Coevery.ContentManagement.Handlers;
using Coevery.Core.Projections.Models;
using Coevery.Data;

namespace Coevery.Core.Projections.Handlers {
    public class ListViewPartHandler : ContentHandler {
        public ListViewPartHandler(IRepository<ListViewPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}