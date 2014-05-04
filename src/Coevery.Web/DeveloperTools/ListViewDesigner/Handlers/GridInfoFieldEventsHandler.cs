using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Entities.Events;
using Coevery.DeveloperTools.ListViewDesigner.Models;
using System.Collections.Generic;

namespace Coevery.DeveloperTools.ListViewDesigner.Handlers {
    public class GridInfoFieldEventsHandler : IFieldEvents {
        private readonly IContentManager _contentManager;

        public GridInfoFieldEventsHandler(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public void OnAdding(string entityName, string fieldName, SettingsDictionary settings) {}

        public void OnDeleting(string entityName, string fieldName) {
            var gridInfos = _contentManager.Query<GridInfoPart, GridInfoPartRecord>()
                .ForVersion(VersionOptions.DraftRequired)
                .Where(v => v.ItemContentType == entityName)
                .List();

            foreach (var gridInfo in gridInfos) {
                var settings = gridInfo.GridSettings;
                settings[GridInfoSettings.Columns] = settings[GridInfoSettings.Columns].Split(';').Where(x => x != fieldName).Join(";");
                gridInfo.GridSettings = settings;
            }
        }
    }
}