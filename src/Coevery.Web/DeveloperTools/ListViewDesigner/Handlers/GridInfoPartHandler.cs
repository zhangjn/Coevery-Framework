using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Data;
using Coevery.DeveloperTools.ListViewDesigner.Models;

namespace Coevery.DeveloperTools.ListViewDesigner.Handlers {
    public class GridInfoPartHandler : ContentHandler {
        private readonly ISettingsFormatter _settingsFormatter;

        public GridInfoPartHandler(IRepository<GridInfoPartRecord> repository, ISettingsFormatter settingsFormatter) {
            _settingsFormatter = settingsFormatter;
            Filters.Add(StorageFilter.For(repository));
            OnActivated<GridInfoPart>((context, part) => LazyLoadHandlers(part));
        }

        private void LazyLoadHandlers(GridInfoPart part) {
            part.GridSettingsField.Loader(value => _settingsFormatter.Parse(part.Record.Settings));
            part.GridSettingsField.Setter(value => {
                part.Record.Settings = _settingsFormatter.Parse(value);
                return value;
            });
        }
    }
}