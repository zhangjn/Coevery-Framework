using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.Handlers {
    public class MeterTypePartHandler : ContentHandler {
        public MeterTypePartHandler(IRepository<MeterTypePartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(DeleteRecordFilter.For(repository));
        }
    }
}