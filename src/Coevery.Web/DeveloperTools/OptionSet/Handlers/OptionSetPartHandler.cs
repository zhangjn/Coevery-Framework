using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.Data;
using Coevery.DeveloperTools.OptionSet.Models;
using Coevery.DeveloperTools.OptionSet.Services;
using JetBrains.Annotations;

namespace Coevery.DeveloperTools.OptionSet.Handlers {
    [UsedImplicitly]
    public class OptionSetPartHandler : ContentHandler {
        public OptionSetPartHandler(
            IRepository<OptionSetPartRecord> repository, 
            IOptionSetService optionSetService,
            IContentDefinitionManager contentDefinitionManager) {

            Filters.Add(StorageFilter.For(repository));
     
            OnLoading<OptionSetPart>((context, part) => part.OptionItemsField.Loader(x => optionSetService.GetOptionItems(part.Id)));
        }
    }
}