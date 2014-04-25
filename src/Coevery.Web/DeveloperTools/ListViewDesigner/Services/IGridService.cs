using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Entities.Models;
using Coevery.DeveloperTools.ListViewDesigner.Models;
using Coevery.DeveloperTools.ListViewDesigner.ViewModels;

namespace Coevery.DeveloperTools.ListViewDesigner.Services {
    public interface IGridService : IDependency {
        GridViewModel GetGridViewModel(string entityName);
        GridViewModel GetGridViewModel(int id);
        int Save(int id, GridViewModel viewModel);
    }

    public class GridService : IGridService {
        private readonly IContentManager _contentManager;
        private readonly ISettingsFormatter _settingsFormatter;

        public GridService(IContentManager contentManager, ISettingsFormatter settingsFormatter) {
            _contentManager = contentManager;
            _settingsFormatter = settingsFormatter;
        }

        public GridViewModel GetGridViewModel(string entityName) {
            var entityMetadataPart = _contentManager
                .Query<EntityMetadataPart, EntityMetadataRecord>(VersionOptions.Latest)
                .Where(x => x.Name == entityName)
                .List()
                .FirstOrDefault();

            if (entityMetadataPart == null) {
                return null;
            }

            var viewModel = new GridViewModel {
                ItemContentType = entityName,
                Fields = entityMetadataPart.FieldMetadataRecords.Select(x =>
                    new GridColumnViewModel {
                        Text = _settingsFormatter.Parse(x.Settings)["DisplayName"],
                        Value = x.Name
                    })
            };

            return viewModel;
        }

        public GridViewModel GetGridViewModel(int id) {
            var gridInfo = _contentManager.Get<GridInfoPart>(id, VersionOptions.Latest);

            if (gridInfo == null) {
                return null;
            }

            string typeName = gridInfo.ItemContentType;
            var entityMetadataPart = _contentManager
                .Query<EntityMetadataPart, EntityMetadataRecord>(VersionOptions.Latest)
                .Where(x => x.Name == typeName)
                .List()
                .FirstOrDefault();

            if (entityMetadataPart == null) {
                return null;
            }

            var settings = gridInfo.GridSettings;
            var columns = settings[GridInfoSettings.Columns].Split(';');

            var viewModel = new GridViewModel {
                Id = gridInfo.Id,
                ItemContentType = gridInfo.ItemContentType,
                DisplayName = gridInfo.DisplayName,
                Fields = entityMetadataPart.FieldMetadataRecords.Select(x =>
                    new GridColumnViewModel {
                        Text = _settingsFormatter.Parse(x.Settings)["DisplayName"],
                        Value = x.Name,
                        Selected = columns.Contains(x.Name)
                    }).ToList(),
                SortColumn = settings[GridInfoSettings.SortColumn],
                SortMode = settings[GridInfoSettings.SortMode],
                PageRowCount = int.Parse(settings[GridInfoSettings.PageRowCount])
            };

            int order = 0;
            foreach (var column in columns) {
                var field = viewModel.Fields.First(x => x.Value == column);
                field.Selected = true;
                field.Order = ++order;
            }
            return viewModel;
        }

        public int Save(int id, GridViewModel viewModel) {
            GridInfoPart gridInfo;
            if (id == 0) {
                gridInfo = _contentManager.New<GridInfoPart>("GridInfo");
                _contentManager.Create(gridInfo, VersionOptions.Draft);
                id = gridInfo.Id;
            }
            else {
                gridInfo = _contentManager.Get<GridInfoPart>(id, VersionOptions.DraftRequired);
            }

            gridInfo.DisplayName = viewModel.DisplayName;
            gridInfo.ItemContentType = viewModel.ItemContentType;

            var settings = gridInfo.GridSettings;
            settings[GridInfoSettings.Columns] = viewModel.SelectedColumns.Join(";");
            settings[GridInfoSettings.SortColumn] = viewModel.SortColumn ?? string.Empty;
            settings[GridInfoSettings.SortMode] = viewModel.SortMode ?? string.Empty;
            settings[GridInfoSettings.PageRowCount] = viewModel.PageRowCount.ToString();
            gridInfo.GridSettings = settings;

            return id;
        }
    }
}