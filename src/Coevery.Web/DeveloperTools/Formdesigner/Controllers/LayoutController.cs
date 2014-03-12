﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.DeveloperTools.Entities.Extensions;
using Coevery.DeveloperTools.Formdesigner.Models;
using Coevery.DeveloperTools.Formdesigner.Services;

namespace Coevery.DeveloperTools.Formdesigner.Controllers {
    public class LayoutController : ApiController {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ITemplateViewService _templateViewService;
        private const string AlwaysInLayoutKey = "TextFieldSettings.AlwaysInLayout";

        public LayoutController(IContentDefinitionManager contentDefinitionManager,
            ITemplateViewService templateViewService) {
            _contentDefinitionManager = contentDefinitionManager;
            _templateViewService = templateViewService;
        }

        // GET api/leads/lead/5
        public virtual object Get(string id) {
            var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(id);
            if (contentTypeDefinition == null) {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            string layout = contentTypeDefinition.Settings.ContainsKey("Layout")
                ? contentTypeDefinition.Settings["Layout"]
                : null;

            var layoutInfo = new {
                id,
                layout
            };
            return layoutInfo;
        }

        // POST api/metadata/field
        public virtual HttpResponseMessage Post(string id, ICollection<Section> data) {
            if (!ModelState.IsValid) {
                return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
            }

            var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(id);
            if (contentTypeDefinition == null) {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var layout = GetLayout(contentTypeDefinition, data);
            if (string.IsNullOrEmpty(layout)) {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
            if (contentTypeDefinition.Settings.ContainsKey("Layout")) {
                contentTypeDefinition.Settings["Layout"] = layout;
            }
            else {
                contentTypeDefinition.Settings.Add("Layout", layout);
            }
            _contentDefinitionManager.StoreTypeDefinition(contentTypeDefinition);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private string GetLayout(ContentTypeDefinition contentTypeDefinition, IEnumerable<Section> data) {
            //check field valid
            if (contentTypeDefinition.Parts.Any()) {
                var part = contentTypeDefinition.Parts.First(x => x.PartDefinition.Name == contentTypeDefinition.Name.ToPartName());
                var partFields = part.PartDefinition.Fields.ToList();
                var fields = data.SelectMany(x => x.Rows)
                    .SelectMany(x => x.Columns)
                    .Where(x => x.Field != null)
                    .Select(x => x.Field).ToList();
                if (partFields.Any(f => f.Settings.ContainsKey(AlwaysInLayoutKey)
                                        && bool.Parse(f.Settings[AlwaysInLayoutKey])
                                        && !fields.Select(x => x.FieldName).Contains(f.Name))) {
                    return string.Empty;
                }
                foreach (var field in fields) {
                    if (partFields.Any(x => x.Name == field.FieldName)) {
                        field.IsValid = true;
                    } else if (contentTypeDefinition.Parts.Any(x => x.PartDefinition.Name == field.FieldName)) {
                        field.IsValid = true;
                    }
                }
            }

            var viewData = new ViewDataDictionary {{"Layout", data}};
            string layout = _templateViewService
                .RenderView("FormDesigner", "FormTemplate", "FormDesignerLayout", viewData);
            return layout;
        }
    }
}