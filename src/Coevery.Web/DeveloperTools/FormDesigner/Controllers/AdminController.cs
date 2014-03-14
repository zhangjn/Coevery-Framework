﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.Common.Extensions;
using Coevery.DeveloperTools.EntityManagement.Services;
using Coevery.DeveloperTools.FormDesigner.ViewModels;
using Coevery.Localization;
using Coevery.Logging;

namespace Coevery.DeveloperTools.FormDesigner.Controllers {
    public class AdminController : Controller {
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentMetadataService _contentMetadataService;

        public AdminController(
            ICoeveryServices coeveryServices,
            IContentMetadataService contentMetadataService,
            IContentManager contentManager, IContentDefinitionManager contentDefinitionManager) {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _contentMetadataService = contentMetadataService;
            Services = coeveryServices;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; private set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult Index(string id, string returnUrl) {
            if (string.IsNullOrEmpty(id)) {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            if (!_contentMetadataService.CheckEntityPublished(id)) {
                return Content(T("The \"{0}\" hasn't been published!", id).Text);
            }
            var contentItem = _contentManager.New(id);
            dynamic model = _contentManager.BuildEditor(contentItem);
            var contentTypeDefinition = contentItem.TypeDefinition;
            string layout = contentTypeDefinition.Settings.ContainsKey("Layout")
                ? contentTypeDefinition.Settings["Layout"]
                : string.Empty;
            layout = string.IsNullOrEmpty(layout)
                ? "<div fd-section=\"\" section-columns=\"1\" section-columns-width=\"6:6\" section-title=\"Sample Title\"><div fd-row=\"\"><div fd-column=\"\"></div></div></div>"
                : layout;
            var viewModel = Services.New.ViewModel();
            viewModel.Layout = layout;
            viewModel.DisplayName = contentItem.TypeDefinition.DisplayName;
            var templates = new List<dynamic>();
            var fields = _contentDefinitionManager.GetPartDefinition(id.ToPartName()).Fields
                .Select(x => new FieldViewModel {DisplayName = x.DisplayName, Name = x.Name})
                .ToList();
            foreach (var item in model.Content.Items) {
                if (item.TemplateName != null && item.TemplateName.StartsWith("Fields/")) {
                    templates.Add(item);
                }
                else if (item.TemplateName == "Parts/Relationship.Edit") {
                    templates.Add(item);
                    var name = item.ContentPart.GetType().Name;
                    fields.Add(new FieldViewModel {
                        Name = name,
                        DisplayName = _contentDefinitionManager.GetPartDefinition(name).Settings["DisplayName"]
                    });
                }
            }
            viewModel.Templates = templates;
            viewModel.Fields = fields;

            return View((object) viewModel);
        }
    }
}