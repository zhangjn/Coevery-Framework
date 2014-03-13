﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Instrumentation;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.Forms.Services;
using Coevery.DeveloperTools.Entities.Extensions;
using Coevery.DeveloperTools.Projections.Models;
using Coevery.DeveloperTools.Projections.ViewModels;
using Coevery.Localization;
using Coevery.UI.Notify;

namespace Coevery.DeveloperTools.Projections.Services {
    public class ProjectionService : IProjectionService {
        public const string DefaultLayoutCategory = "Grids";
        public const string DefaultLayoutType = "Default";
        private readonly IProjectionManager _projectionManager;
        private readonly IContentManager _contentManager;
        private readonly IFormManager _formManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public ProjectionService(
            ICoeveryServices services,
            IProjectionManager projectionManager,
            IContentManager contentManager,
            IFormManager formManager,
            IContentDefinitionManager contentDefinitionManager) {
            _projectionManager = projectionManager;
            _contentManager = contentManager;
            _formManager = formManager;
            Services = services;
            _contentDefinitionManager = contentDefinitionManager;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        public IEnumerable<PicklistItemViewModel> GetFieldDescriptors(string entityType, int projectionId) {
            var category = entityType.ToPartName() + "ContentFields";
            var fieldDescriptors = _projectionManager.DescribeProperties()
                .Where(x => x.Category == category).SelectMany(x => x.Descriptors)
                .Select(element => new PicklistItemViewModel {
                    Category = element.Category,
                    Text = element.Name.Text,
                    Value = element.Type
                }).ToList();
            if (projectionId <= 0) {
                return fieldDescriptors;
            }
            var selectedFields = _contentManager.Get<ProjectionPart>(projectionId).Record.LayoutRecord.Properties;
            var order = 0;
            foreach (var field in selectedFields) {
                var viewModel = fieldDescriptors.Find(model => model.Value == field.Type);
                viewModel.Selected = true;
                viewModel.Order = ++order;
            }
            return fieldDescriptors;
        }

        public ProjectionEditViewModel GetProjectionViewModel(int id) {
            var viewModel = new ProjectionEditViewModel();
            //Get Projection&QueryPart
            var projectionItem = _contentManager.Get(id, VersionOptions.Latest);
            var projectionPart = projectionItem.As<ProjectionPart>();
            var queryId = projectionPart.Record.QueryPartRecord.Id;
            var queryPart = _contentManager.Get<QueryPart>(queryId, VersionOptions.Latest);
            var layout = projectionPart.Record.LayoutRecord;
            var listViewPart = projectionItem.As<ListViewPart>();
            viewModel.Id = id;
            viewModel.ItemContentType = listViewPart.ItemContentType.ToPartName();
            viewModel.DisplayName = listViewPart.Name;
            viewModel.VisableTo = listViewPart.VisableTo;
            viewModel.IsDefault = listViewPart.IsDefault;
            //Get AllFields
            viewModel.Fields = GetFieldDescriptors(listViewPart.ItemContentType, id);
            //Layout related
            viewModel.LayoutId = layout.Id;
            viewModel.Layout = _projectionManager.DescribeLayouts()
                .SelectMany(descr => descr.Descriptors)
                .FirstOrDefault(descr => descr.Category == layout.Category && descr.Type == layout.Type);
            if (viewModel.Layout == null) {
                throw new InstanceNotFoundException(T("Layout not found!").Text);
            }
            viewModel.Form = _formManager.Build(viewModel.Layout.Form) ?? Services.New.EmptyForm();
            viewModel.State = FormParametersHelper.FromString(layout.State);
            viewModel.Form.Fields = viewModel.Fields;
            viewModel.Form.State = MergeDictionary(new[] {(Dictionary<string, string>)viewModel.Form.State, viewModel.State});

            return viewModel;
        }

        public string UpdateViewOnEntityAltering(string entityName) {
            var entityType = _contentDefinitionManager.GetTypeDefinition(entityName);
            var listViewParts = _contentManager.Query<ListViewPart, ListViewPartRecord>()
                .Where(record => record.ItemContentType == entityName).List();
            if (entityType == null || listViewParts == null || !listViewParts.Any()) {
                return "Invalid entity name!";
            }

            const string settingName = "TextFieldSettings.IsDisplayField";
            foreach (var view in listViewParts) {
                var projection = view.As<ProjectionPart>().Record;
                var layout = projection.LayoutRecord;
                var pickedFileds = (from field in layout.Properties
                    select new PropertyDescriptorViewModel {
                        Category = field.Category,
                        Type = field.Type,
                        Text = field.Description
                    }).ToArray();
                UpdateLayoutProperties(entityName.ToPartName(), layout, settingName, pickedFileds);
                var state = FormParametersHelper.FromString(layout.State);
                layout.State = FormParametersHelper.ToString(MergeDictionary(
                    new[] {state, GetLayoutState(projection.QueryPartRecord.Id, layout.Properties.Count, layout.Description)}));
            }
            return null;
        }

        public int EditPost(int id, ProjectionEditViewModel viewModel) {

            ListViewPart listViewPart;
            ProjectionPart projectionPart;
            QueryPart queryPart;
            if (id == 0) {
                listViewPart = _contentManager.New<ListViewPart>("ListViewPage");
                listViewPart.ItemContentType = viewModel.ItemContentType.RemovePartSuffix();
                queryPart = _contentManager.New<QueryPart>("Query");

                var layout = new LayoutRecord {
                    Category = viewModel.Layout.Category,
                    Type = viewModel.Layout.Type,
                    Description = viewModel.Layout.Description.Text,
                    Display = 1
                };

                queryPart.Layouts.Add(layout);
                projectionPart = listViewPart.As<ProjectionPart>();
                projectionPart.Record.LayoutRecord = layout;
                projectionPart.Record.QueryPartRecord = queryPart.Record;

                /*@todo: when layout is tree grid, need to do some extra logic*/
                var filterGroup = new FilterGroupRecord();
                queryPart.Record.FilterGroups.Add(filterGroup);
                var filterRecord = new FilterRecord {
                    Category = "Content",
                    Type = "ContentTypes",
                    Position = filterGroup.Filters.Count,
                    State = GetContentTypeFilterState(listViewPart.ItemContentType)
                };
                filterGroup.Filters.Add(filterRecord);

                _contentManager.Create(queryPart.ContentItem);
                _contentManager.Create(projectionPart.ContentItem);
            }
            else {
                listViewPart = _contentManager.Get<ListViewPart>(id, VersionOptions.Latest);
                projectionPart = listViewPart.As<ProjectionPart>();
                var queryId = projectionPart.Record.QueryPartRecord.Id;
                queryPart = _contentManager.Get<QueryPart>(queryId, VersionOptions.Latest);
            }

            listViewPart.VisableTo = viewModel.VisableTo;
            listViewPart.Name = viewModel.DisplayName;
            listViewPart.IsDefault = viewModel.IsDefault;
            queryPart.Name = listViewPart.ItemContentType + " - " + viewModel.DisplayName;

            //Post Selected Fields
            var layoutRecord = projectionPart.Record.LayoutRecord;

            var category = viewModel.ItemContentType + "ContentFields";
            const string settingName = "TextFieldSettings.IsDisplayField";
            try {
                UpdateLayoutProperties(viewModel.ItemContentType, layoutRecord, settingName, viewModel.PickedFields);
            }
            catch (Exception exception) {
                Services.Notifier.Add(NotifyType.Error, T(exception.Message));
            }
            layoutRecord.State = FormParametersHelper.ToString(
                MergeDictionary(new[] {
                    viewModel.State, GetLayoutState(queryPart.Id, layoutRecord.Properties.Count, layoutRecord.Description)
                }));
            if (viewModel.Layout.Category == DefaultLayoutCategory && viewModel.Layout.Type == DefaultLayoutType) {
                projectionPart.Record.ItemsPerPage = Convert.ToInt32(viewModel.State["PageRowCount"]);
                // sort
                queryPart.SortCriteria.Clear();
                if (!string.IsNullOrEmpty(viewModel.State["SortedBy"])) {
                    var sortCriterionRecord = new SortCriterionRecord {
                        Category = category,
                        Type = viewModel.State["SortedBy"],
                        Position = queryPart.SortCriteria.Count,
                        State = GetSortState(viewModel.State["SortedBy"], viewModel.State["SortMode"]),
                        Description = viewModel.State["SortedBy"] + " " + viewModel.State["SortMode"]
                    };
                    queryPart.SortCriteria.Add(sortCriterionRecord);
                }
            }
            return listViewPart.Id;
        }

        private void UpdateLayoutProperties(string partName, LayoutRecord layout, string settingName, IEnumerable<PropertyDescriptorViewModel> pickedFileds) {

            layout.Properties.Clear();
            foreach (var property in pickedFileds) {

                var propertyRecord = new PropertyRecord {
                    Category = property.Category,
                    Type = property.Type,
                    Description = property.Text,
                    Position = layout.Properties.Count,
                    State = FormParametersHelper.ToString(new Dictionary<string,string>())
                };
                layout.Properties.Add(propertyRecord);
            }
        }

        private static string GetContentTypeFilterState(string entityType) {
            const string format = @"<Form><Description></Description><ContentTypes>{0}</ContentTypes></Form>";
            return string.Format(format, entityType);
        }

        private static string GetSortState(string description, string sortMode) {
            const string format = @"<Form><Description>{0}</Description><Sort>{1}</Sort></Form>";
            return string.Format(format, description, sortMode == "Desc" ? "true" : "false");
        }

        private static IDictionary<string, string> GetLayoutState(int queryId, int columnCount, string descr) {
            return new Dictionary<string, string> {
                {"QueryId", queryId.ToString(CultureInfo.InvariantCulture)},
                {"Description", descr},
                {"Display", "1"},
                {"DisplayType", "Summary"},
                {"Columns", columnCount.ToString(CultureInfo.InvariantCulture)},
                {"GridId", string.Empty},
                {"GridClass", string.Empty},
                {"RowClass", string.Empty}
            };
        }

        private static IDictionary<string, string> MergeDictionary(IEnumerable<IDictionary<string, string>> dictionaries, bool useLast = true) {
            return dictionaries.SelectMany(pair => pair)
                .ToLookup(pair => pair.Key, pair => pair.Value)
                .ToDictionary(group => group.Key, group => useLast ? group.Last() : group.First());
        }
    }
}