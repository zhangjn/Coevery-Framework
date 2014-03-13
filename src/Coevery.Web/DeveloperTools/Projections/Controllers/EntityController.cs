﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Coevery.ContentManagement;
using Coevery.ContentManagement.FieldStorage;
using Coevery.Core.Forms.Services;
using Coevery.Core.Tokens;
using Coevery.DeveloperTools.Entities.Events;
using Coevery.DeveloperTools.Entities.Extensions;
using Coevery.DeveloperTools.Projections.Descriptors.Property;
using Coevery.DeveloperTools.Projections.Models;
using Coevery.DeveloperTools.Projections.Services;
using Coevery.DeveloperTools.Projections.ViewModels;
using Coevery.UI.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coevery.DeveloperTools.Projections.Controllers {
    public class EntityController : ApiController {
        private readonly IContentManager _contentManager;
        private readonly IProjectionManager _projectionManager;
        private readonly ITokenizer _tokenizer;
        private readonly IGridService _gridService;
        private readonly IContentDefinitionExtension _contentDefinitionExtension;
        private readonly IEntityDataEvents _entityDataEvents;

        public EntityController(
            IContentManager iContentManager,
            ICoeveryServices coeveryServices,
            IProjectionManager projectionManager,
            IContentDefinitionExtension contentDefinitionExtension,
            ITokenizer tokenizer,
            IGridService gridService,
            IEntityDataEvents entityDataEvents) {
            _contentManager = iContentManager;
            Services = coeveryServices;
            _contentDefinitionExtension = contentDefinitionExtension;
            _projectionManager = projectionManager;
            _tokenizer = tokenizer;
            _gridService = gridService;
            _entityDataEvents = entityDataEvents;
        }

        public ICoeveryServices Services { get; private set; }

        public object Post(string id, ListQueryModel model) {
            if (string.IsNullOrEmpty(id)) {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }
            var part = GetProjectionPartRecord(model.ViewId);
            if (part == null) {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }
            id = _contentDefinitionExtension.GetEntityNameFromCollectionName(id);

            _gridService.GenerateSortCriteria(id, model.Sidx, model.Sord, part.Record.QueryPartRecord.Id);
            var totalRecords = _projectionManager.GetCount(part.Record.QueryPartRecord.Id);
            var pageSize = model.Rows;
            var totalPages = (int) Math.Ceiling((float) totalRecords / (float) pageSize);
            var pager = new Pager(Services.WorkContext.CurrentSite, model.Page, pageSize);
            var records = GetLayoutComponents(part, pager.GetStartIndex(), pager.PageSize);

            return new {
                totalPages = totalPages,
                page = model.Page,
                totalRecords = totalRecords,
                rows = records,
                //filterDescription = string.Empty
            };
        }

        public HttpResponseMessage Delete(string contentId) {
            var idList = contentId.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            string errorMessage = string.Empty;
            foreach (var idItem in idList) {
                var contentItem = _contentManager.Get(int.Parse(idItem), VersionOptions.Latest);
                var context = new DeletingEntityDataContext {ContentItem = contentItem};
                _entityDataEvents.OnDeleting(context);
                if (context.IsCancel) {
                    errorMessage += context.ErrorMessage;
                    continue;
                }
                _contentManager.Remove(contentItem);
            }
            return errorMessage == string.Empty
                ? Request.CreateResponse(HttpStatusCode.OK)
                : Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessage);
        }

        private ProjectionPart GetProjectionPartRecord(int viewId) {
            if (viewId == -1) {
                return null;
            }
            var projectionContentItem = _contentManager.Get(viewId, VersionOptions.Latest);
            var part = projectionContentItem.As<ProjectionPart>();
            return part;
        }

        private IEnumerable<JObject> GetLayoutComponents(ProjectionPart part, int skipCount, int pageCount) {
            // query
            var query = part.Record.QueryPartRecord;

            // applying layout
            var layout = part.Record.LayoutRecord;
            var tokens = new Dictionary<string, object> {{"Content", part.ContentItem}};
            var allFielDescriptors = _projectionManager.DescribeProperties().ToList();
            var fieldDescriptors = layout.Properties.OrderBy(p => p.Position).Select(p => allFielDescriptors.SelectMany(x => x.Descriptors).Select(d => new {Descriptor = d, Property = p}).FirstOrDefault(x => x.Descriptor.Category == p.Category && x.Descriptor.Type == p.Type)).ToList();
            fieldDescriptors = fieldDescriptors.Where(c => c != null).ToList();
            var tokenizedDescriptors = fieldDescriptors.Select(fd => new {fd.Descriptor, fd.Property, State = FormParametersHelper.ToDynamic(_tokenizer.Replace(fd.Property.State, tokens))}).ToList();

            // execute the query
            var contentItems = _projectionManager.GetContentItems(query.Id, skipCount, pageCount).ToList();

            // sanity check so that content items with ProjectionPart can't be added here, or it will result in an infinite loop
            contentItems = contentItems.Where(x => !x.Has<ProjectionPart>()).ToList();

            var layoutComponents = contentItems.Select(
                contentItem => {
                    var result = new JObject();
                    result["ContentId"] = contentItem.Id;
                    tokenizedDescriptors.ForEach(
                        d => {
                            var fieldContext = new PropertyContext {
                                State = d.State,
                                Tokens = tokens
                            };
                            var shape = d.Descriptor.Property(fieldContext, contentItem);
                            string text = (shape == null) ? string.Empty : shape.ToString();
                            var fieldName = d.Property.GetFieldName();
                            result[fieldName] = text;
                        });
                    if (layout.Category == "Grids" && layout.Type == "Tree") {
                        var parentFieldName = FormParametersHelper.FromString(layout.State)["ParentField"].GetFieldName();
                        var parentValue = GetParentId(contentItem, parentFieldName);
                        result["parent"] = parentValue;
                        result["expanded"] = true;
                    }
                    return result;
                }).ToList();
            return layout.Category == "Grids" && layout.Type == "Tree" ? layoutComponents.Select(record => {
                record["level"] = GetLevel(layoutComponents, record["parent"].Value<int?>());
                record["isLeaf"] = IsLeaf(layoutComponents, record["ContentId"].Value<int>());
                return record;
            })
                : layoutComponents;
        }

        private static bool IsLeaf(IEnumerable<JObject> contentItems, int currentId) {
            return !contentItems.Any(record => {
                var currentParent = record["parent"].Value<int?>();
                return currentParent.HasValue && currentParent.Value == currentId;
            });
        }

        private static int GetLevel(IEnumerable<JObject> contentItems, int? parentId) {
            var currentId = parentId;
            var level = 0;
            while (currentId.HasValue) {
                var parentItem = contentItems.FirstOrDefault(item => item["ContentId"].Value<int>() == currentId.Value);
                if (parentItem == null) {
                    throw new JsonReaderException();
                }
                currentId = parentItem["parent"].Value<int?>();
                level++;
            }
            return level;
        }

        private static int? GetParentId(ContentItem contentItem, string parentFieldName) {
            var entityPart = contentItem.Parts
                .FirstOrDefault(p => p.PartDefinition.Name == contentItem.ContentType.ToPartName());
            if (entityPart == null) {
                throw new InstanceNotFoundException("Entity part not found!");
            }
            var parentField = entityPart.Fields.FirstOrDefault(f => f.Name == parentFieldName);
            if (parentField == null) {
                throw new InstanceNotFoundException("Parent field not found!");
            }
            return parentField.Storage.Get<int?>();
        }
    }
}