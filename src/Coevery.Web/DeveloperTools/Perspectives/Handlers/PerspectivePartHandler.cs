using Coevery.ContentManagement.Handlers;
using Coevery.Data;
using Coevery.DeveloperTools.Perspectives.Models;

namespace Coevery.DeveloperTools.Perspectives.Handlers
{
    public class PerspectivePartHandler : ContentHandler {

        public PerspectivePartHandler(IRepository<PerspectivePartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
