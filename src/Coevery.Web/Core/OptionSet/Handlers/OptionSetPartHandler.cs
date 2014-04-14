using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.OptionSet.Models;
using Coevery.Core.OptionSet.Services;
using Coevery.Data;
using JetBrains.Annotations;

namespace Coevery.Core.OptionSet.Handlers {
    [UsedImplicitly]
    public class OptionSetPartHandler : ContentHandler {
        public OptionSetPartHandler(
            IRepository<OptionSetPartRecord> repository, 
            IOptionSetService optionSetService) {

            Filters.Add(StorageFilter.For(repository));
     
            OnLoading<OptionSetPart>((context, part) => part.OptionItemsField.Loader(x => optionSetService.GetOptionItems(part.Id)));
        }
    }
}