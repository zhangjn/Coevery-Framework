using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Entities.Events;
using Coevery.DeveloperTools.ListViewDesigner.Models;
using Coevery.DeveloperTools.ListViewDesigner.Services;

namespace Coevery.DeveloperTools.ListViewDesigner.Handlers {
    public class GridInfoFieldEventsHandler : IFieldEvents {
        private readonly IContentManager _contentManager;
        private readonly IGridService _gridService;

        public GridInfoFieldEventsHandler(IContentManager contentManager, IGridService gridService) {
            _contentManager = contentManager;
            _gridService = gridService;
        }

        public void OnAdding(string entityName, string fieldName, SettingsDictionary settings) {}

        public void OnDeleting(string entityName, string fieldName) {
            var gridInfos = _contentManager.Query<GridInfoPart, GridInfoPartRecord>()
                .ForVersion(VersionOptions.DraftRequired)
                .Where(v => v.ItemContentType == entityName)
                .List();

            foreach (var gridInfo in gridInfos) {
                var settings = gridInfo.GridSettings;
                var columns = _gridService.ParseColumns(settings[GridInfoSettings.Columns]).Where(x => x != fieldName);
                settings[GridInfoSettings.Columns] = _gridService.ParseColumns(columns);
                gridInfo.GridSettings = settings;
            }
        }
    }
}