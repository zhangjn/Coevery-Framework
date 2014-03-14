using Coevery.ContentManagement.Handlers;
using Coevery.Core.OptionSet.Models;
using Coevery.Data;

namespace Coevery.Core.OptionSet.Handlers {
    public class OptionItemPartHandler : ContentHandler {
        public OptionItemPartHandler(IRepository<OptionItemPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
            OnInitializing<OptionItemPart>((context, part) => part.Selectable = true);
        }
    }
}