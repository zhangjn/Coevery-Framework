using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Settings.Metadata.Parts;
using Coevery.Data;
using Coevery.DeveloperTools.ListViewDesigner.Models;

namespace Coevery.DeveloperTools.ListViewDesigner.Handlers {
    public class GridInfoPartHandler : ContentHandler {
        private readonly ISettingsFormatter _settingsFormatter;
        private readonly IContentManager _contentManager;

        public GridInfoPartHandler(
            IRepository<GridInfoPartRecord> repository,
            ISettingsFormatter settingsFormatter,
            IContentManager contentManager) {
            _settingsFormatter = settingsFormatter;
            _contentManager = contentManager;
            Filters.Add(StorageFilter.For(repository));
            OnActivated<GridInfoPart>((context, part) => LazyLoadHandlers(part));
            OnPublished<ContentTypeDefinitionPart>((context, part) => PublishGridInfo(part));
            OnRemoved<ContentTypeDefinitionPart>((context, part) => RemoveGridInfo(part));
        }

        private void LazyLoadHandlers(GridInfoPart part) {
            part.GridSettingsField.Loader(value => _settingsFormatter.Parse(part.Record.Settings));
            part.GridSettingsField.Setter(value => {
                part.Record.Settings = _settingsFormatter.Parse(value);
                return value;
            });
        }

        private void PublishGridInfo(ContentTypeDefinitionPart part) {
            if (!part.Customized) {
                return;
            }

            var items = GetGridInfoParts(part.Name, VersionOptions.Latest);

            foreach (var item in items) {
                _contentManager.Publish(item.ContentItem);
            }
        }

        private void RemoveGridInfo(ContentTypeDefinitionPart part) {
            if (!part.Customized) {
                return;
            }

            var items = GetGridInfoParts(part.Name, VersionOptions.Latest);

            foreach (var item in items) {
                _contentManager.Remove(item.ContentItem);
            }
        }

        private IEnumerable<GridInfoPart> GetGridInfoParts(string entityName, VersionOptions options) {
            return _contentManager.Query<GridInfoPart, GridInfoPartRecord>(options)
                .Where(v => v.ItemContentType == entityName)
                .List();
        }
    }
}