using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Nova.Select2.Models;

namespace Nova.Select2.Handlers {
    public class OpportunityPartHandler : ContentHandler {
        public OpportunityPartHandler(IRepository<OpportunityPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}