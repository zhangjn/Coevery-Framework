﻿using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.Entities.Events;
using Coevery.DeveloperTools.Entities.Extensions;
using Coevery.DeveloperTools.Projections.Models;
using Coevery.DeveloperTools.Projections.Services;
using Coevery.DeveloperTools.Projections.ViewModels;

namespace Coevery.DeveloperTools.Projections.Handlers {
    public class ProjectionEntityEventsHandler : IEntityEvents {
        private readonly IProjectionService _projectionService;
        private readonly IContentManager _contentManager;
        private readonly IProjectionManager _projectionManager;

        public ProjectionEntityEventsHandler(
            IProjectionService projectionService,
            IProjectionManager projectionManager,
            IContentManager contentManager) {
            _projectionService = projectionService;
            _contentManager = contentManager;
            _projectionManager = projectionManager;
        }

        public void OnCreated(string entityName) {
            var fields = _projectionService.GetFieldDescriptors(entityName, -1).ToArray();

            var viewModel = new ProjectionEditViewModel {
                ItemContentType = entityName.ToPartName(),
                DisplayName = entityName + " DefaultView",
                IsDefault = true,
                Layout = _projectionManager.DescribeLayouts()
                    .SelectMany(descr => descr.Descriptors)
                    .FirstOrDefault(descr => descr.Category == "Grids" && descr.Type == "Default"),
                Fields = fields,
                PickedFields = fields.Select(f => new PropertyDescriptorViewModel {
                    Category = f.Category,
                    Type = f.Value,
                    Text = f.Text
                }),
                State = new Dictionary<string, string> {
                    {"PageRowCount", "50"}, {"SortedBy", string.Empty}, {"SortMode", string.Empty}
                }
            };
            _projectionService.EditPost(0, viewModel);
        }

        public void OnUpdating(string entityName) {
            _projectionService.UpdateViewOnEntityAltering(entityName);
        }

        public void OnDeleting(string entityName) {
            var projections = _contentManager.Query<ListViewPart, ListViewPartRecord>().Where(x => x.ItemContentType == entityName).List();
            foreach (var projection in projections) {
                _contentManager.Remove(projection.ContentItem);
            }
        }
    }
}