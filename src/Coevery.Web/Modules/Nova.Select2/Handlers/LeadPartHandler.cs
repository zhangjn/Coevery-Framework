using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Nova.Select2.Models;

namespace Nova.Select2.Handlers {
    public class LeadPartHandler : ContentHandler {
        public LeadPartHandler(IRepository<LeadPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}