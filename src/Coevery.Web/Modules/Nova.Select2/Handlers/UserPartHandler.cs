using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Nova.Select2.Models;

namespace Nova.Select2.Handlers {
    public class UserPartHandler : ContentHandler {
        public UserPartHandler(IRepository<UserPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}