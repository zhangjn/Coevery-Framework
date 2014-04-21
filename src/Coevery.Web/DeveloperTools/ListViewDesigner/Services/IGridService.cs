using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Services;
using Coevery.Core.Common.Extensions;
using Coevery.Core.Entities.Models;
using Coevery.Core.Projections.Services;
using Coevery.Core.Projections.ViewModels;
using Coevery.DeveloperTools.ListViewDesigner.Models;

namespace Coevery.DeveloperTools.ListViewDesigner.Services {
    public interface IGridService : IDependency {
        ProjectionEditViewModel GetViewModel(string entityName, string category, string type);
        int Save(int id, ProjectionEditViewModel viewModel);
    }

    public class GridService : IGridService {
        private readonly IContentManager _contentManager;
        private readonly ISettingsFormatter _settingsFormatter;
        private readonly IProjectionManager _projectionManager;

        public GridService(IContentManager contentManager, ISettingsFormatter settingsFormatter, IProjectionManager projectionManager) {
            _contentManager = contentManager;
            _settingsFormatter = settingsFormatter;
            _projectionManager = projectionManager;
        }

        public ProjectionEditViewModel GetViewModel(string entityName, string category, string type) {
            var entityMetadataPart = _contentManager
                .Query<EntityMetadataPart, EntityMetadataRecord>(VersionOptions.Latest)
                .Where(x => x.Name == entityName)
                .List()
                .FirstOrDefault();

            if (entityMetadataPart == null) {
                return null;
            }

            var viewModel = new ProjectionEditViewModel {
                ItemContentType = entityName.ToPartName(),
                Fields = entityMetadataPart.FieldMetadataRecords.Select(x =>
                    new PicklistItemViewModel {
                        Text = _settingsFormatter.Parse(x.Settings)["DisplayName"],
                        Value = x.Name
                    }),
                Layout = _projectionManager.DescribeLayouts()
                    .SelectMany(descr => descr.Descriptors)
                    .FirstOrDefault(descr => descr.Category == category && descr.Type == type),
            };

            return viewModel;
        }

        public int Save(int id, ProjectionEditViewModel viewModel) {
            GridInfoPart gridInfo;
            if (id == 0) {
                gridInfo = _contentManager.New<GridInfoPart>("GridInfo");
                _contentManager.Create(gridInfo, VersionOptions.Draft);
            }
            else {
                gridInfo = _contentManager.Get<GridInfoPart>(id, VersionOptions.DraftRequired);
            }

            gridInfo.DisplayName = viewModel.DisplayName;
            gridInfo.ItemContentType = viewModel.ItemContentType;
            //gridInfo.Settings["Columns"] = viewModel.PickedFields.Select(x => x.Type).Join(";");


            return id;
        }
    }
}