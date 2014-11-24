using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.Handlers {
    public class CustomerPartHandler : ContentHandler {
        public CustomerPartHandler(IRepository<CustomerPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(DeleteRecordFilter.For(repository));
        }
    }
}