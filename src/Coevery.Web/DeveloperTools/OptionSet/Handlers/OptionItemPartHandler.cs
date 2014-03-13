using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.DeveloperTools.OptionSet.Models;

namespace Coevery.DeveloperTools.OptionSet.Handlers {
    public class OptionItemPartHandler : ContentHandler {
        public OptionItemPartHandler(IRepository<OptionItemPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
            OnInitializing<OptionItemPart>((context, part) => part.Selectable = true);
        }
    }
}