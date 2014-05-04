using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Settings.Metadata.Parts;
using Coevery.Core.Settings.Metadata.Records;
using Coevery.Data;

namespace Coevery.Core.Settings.Metadata.Handlers {
    public class ContentTypeDefinitionPartHandler : ContentHandler {
        private readonly ISettingsFormatter _settingsFormatter;

        public ContentTypeDefinitionPartHandler(
            IRepository<ContentTypeDefinitionRecord> typeDefinitionRepository,
            ISettingsFormatter settingsFormatter) {
            _settingsFormatter = settingsFormatter;
            Filters.Add(new ActivatingFilter<ContentTypeDefinitionPart>("ContentTypeDefinition"));
            Filters.Add(StorageFilter.For(typeDefinitionRepository));

            OnActivated<ContentTypeDefinitionPart>((context, part) => LazyLoadHandlers(part));
        }

        private void LazyLoadHandlers(ContentTypeDefinitionPart part) {
            part._entitySettings.Loader(() => _settingsFormatter.Parse(part.Record.Settings));
            part._entitySettings.Setter(value => {
                part.Record.Settings = _settingsFormatter.Parse(value);
                return value;
            });
        }
    }
}