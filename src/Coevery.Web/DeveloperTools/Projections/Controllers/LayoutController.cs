﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Coevery.Data;
using Coevery.DeveloperTools.Projections.Descriptors.Property;
using Coevery.DeveloperTools.Projections.Models;
using Coevery.DeveloperTools.Projections.Services;
using Coevery.DeveloperTools.Projections.ViewModels;
using Coevery.DisplayManagement;
using Coevery.Localization;
using Coevery.Security;
using Coevery.UI.Admin;
using Coevery.UI.Notify;

namespace Coevery.DeveloperTools.Projections.Controllers {
    [ValidateInput(false), Admin]
    public class LayoutController : Controller {
        public LayoutController(
            ICoeveryServices services,
            IFormManager formManager,
            IShapeFactory shapeFactory,
            IProjectionManager projectionManager,
            IRepository<LayoutRecord> repository,
            IQueryService queryService) {
            Services = services;
            _formManager = formManager;
            _projectionManager = projectionManager;
            _repository = repository;
            _queryService = queryService;
            Shape = shapeFactory;
        }

        public ICoeveryServices Services { get; set; }
        private readonly IFormManager _formManager;
        private readonly IProjectionManager _projectionManager;
        private readonly IRepository<LayoutRecord> _repository;
        private readonly IQueryService _queryService;
        public Localizer T { get; set; }
        public dynamic Shape { get; set; }

        public ActionResult Add(int id) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage queries")))
                return new HttpUnauthorizedResult();

            var viewModel = new LayoutAddViewModel { Id = id, Layouts = _projectionManager.DescribeLayouts() };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage queries")))
                return new HttpUnauthorizedResult();

            var layout = _repository.Get(id);
            if(layout == null) {
                return HttpNotFound();
            }

            var queryId = layout.QueryPartRecord.Id;

            layout.QueryPartRecord.Layouts.Remove(layout);
            _repository.Delete(layout);

            Services.Notifier.Information(T("Layout deleted"));

            return RedirectToAction("Edit", "Admin", new { id = queryId });
        }

        public ActionResult Create(int id, string category, string type) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage queries")))
                return new HttpUnauthorizedResult();

            var layout = _projectionManager.DescribeLayouts().SelectMany(x => x.Descriptors).FirstOrDefault(x => x.Category == category && x.Type == type);

            if (layout == null) {
                return HttpNotFound();
            }

            // build the form, and let external components alter it
            var form = _formManager.Build(layout.Form) ?? Services.New.EmptyForm();

            var viewModel = new LayoutEditViewModel {
                QueryId = id,
                Layout = layout,
                Form = form
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(LayoutEditViewModel model, FormCollection formCollection) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage queries")))
                return new HttpUnauthorizedResult();

            // validating form values
            model.Layout = _projectionManager.DescribeLayouts().SelectMany(x => x.Descriptors).FirstOrDefault(x => x.Category == model.Category && x.Type == model.Type);

            _formManager.Validate(new ValidatingContext { FormName = model.Layout.Form, ModelState = ModelState, ValueProvider = ValueProvider });

            var form = _formManager.Build(model.Layout.Form) ?? Services.New.EmptyForm();
            _formManager.Bind(form, formCollection);

            model.Form = form;

            if (ModelState.IsValid) {
                var layoutRecord = new LayoutRecord { Category = model.Category, Type = model.Type };
                var query = _queryService.GetQuery(model.QueryId);
                query.Layouts.Add(layoutRecord);

                var dictionary = formCollection.AllKeys.ToDictionary(key => key, formCollection.Get);

                // save form parameters
                layoutRecord.State = FormParametersHelper.ToString(dictionary);
                layoutRecord.Description = model.Description;
                layoutRecord.Display = model.Display;
                layoutRecord.DisplayType = model.DisplayType;

                Services.Notifier.Information(T("Layout Created"));

                _repository.Create(layoutRecord);
                return RedirectToAction("Edit", new { id = layoutRecord.Id });
            }

            return View(model);
        }

        public ActionResult Edit(int id) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage queries")))
                return new HttpUnauthorizedResult();

            LayoutRecord layoutRecord = _repository.Get(id);
            
            if (layoutRecord == null) {
                return HttpNotFound();
            }

            var layoutDescriptor = _projectionManager.DescribeLayouts().SelectMany(x => x.Descriptors).FirstOrDefault(x => x.Category == layoutRecord.Category && x.Type == layoutRecord.Type);

            // build the form, and let external components alter it
            var form = _formManager.Build(layoutDescriptor.Form) ?? Services.New.EmptyForm();

            var viewModel = new LayoutEditViewModel {
                Id = id, 
                QueryId = layoutRecord.QueryPartRecord.Id,
                Category = layoutDescriptor.Category,
                Type = layoutDescriptor.Type,
                Description = layoutRecord.Description,
                Display = layoutRecord.Display,
                DisplayType = String.IsNullOrWhiteSpace(layoutRecord.DisplayType) ? "Summary" : layoutRecord.DisplayType,
                Layout = layoutDescriptor, 
                Form = form
            };
            
            // bind form with existing values
            var parameters = FormParametersHelper.FromString(layoutRecord.State);
            _formManager.Bind(form, new DictionaryValueProvider<string>(parameters, CultureInfo.InvariantCulture));

            #region Load Fields

            var fieldEntries = new List<PropertyEntry>();
            var allFields = _projectionManager.DescribeProperties().SelectMany(x => x.Descriptors);

            foreach (var field in layoutRecord.Properties) {
                var fieldCategory = field.Category;
                var fieldType = field.Type;

                var f = allFields.FirstOrDefault(x => fieldCategory == x.Category && fieldType == x.Type);
                if (f != null) {
                    fieldEntries.Add(
                        new PropertyEntry {
                            Category = f.Category,
                            Type = f.Type,
                            PropertyRecordId = field.Id,
                            DisplayText = String.IsNullOrWhiteSpace(field.Description) ? f.Display(new PropertyContext {State = FormParametersHelper.ToDynamic(field.State)}).Text : field.Description,
                            Position = field.Position
                        });
                }
            }

            viewModel.Properties = fieldEntries.OrderBy(f => f.Position);
            viewModel.Groups = layoutRecord.Groups.Select(g => {
                var groupEntry = new PropertyGroupEntry();
                var fieldEntry = fieldEntries.FirstOrDefault(f => f.PropertyRecordId == g.GroupProperty.Id);
                groupEntry.DisplayText = fieldEntry == null ? string.Empty : fieldEntry.DisplayText;
                groupEntry.Position = g.Position;
                groupEntry.Sort = g.Sort;
                groupEntry.LayoutGroupRecordId = g.Id;
                groupEntry.GroupPropertyId = g.GroupProperty.Id;
                return groupEntry;
            }).OrderBy(g => g.Position).ToList();

            #endregion

            return View(viewModel);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(LayoutEditViewModel model, FormCollection formCollection) {

            // validating form values
            var layout = _projectionManager.DescribeLayouts().SelectMany(x => x.Descriptors).FirstOrDefault(x => x.Category == model.Category && x.Type == model.Type);
            _formManager.Validate(new ValidatingContext { FormName = layout.Form, ModelState = ModelState, ValueProvider = ValueProvider });

            var form = _formManager.Build(layout.Form) ?? Services.New.EmptyForm();
            _formManager.Bind(form, formCollection);

            model.Layout = layout;
            model.Form = form;
            var layoutRecord = _repository.Get(model.Id);

            if (ModelState.IsValid) {

                var dictionary = formCollection.AllKeys.ToDictionary(key => key, formCollection.Get);

                // save form parameters
                layoutRecord.State = FormParametersHelper.ToString(dictionary);
                layoutRecord.Description = model.Description;
                layoutRecord.Display = model.Display;
                layoutRecord.DisplayType = model.DisplayType;
                //layoutRecord.GroupProperty = layoutRecord.Properties.FirstOrDefault(x => x.Id == model.GroupPropertyId);

                Services.Notifier.Information(T("Layout Saved"));

                return RedirectToAction("Edit", new { id = layoutRecord.Id });
            }

            #region Load Fields

            var fieldEntries = new List<PropertyEntry>();
            var allFields = _projectionManager.DescribeProperties().SelectMany(x => x.Descriptors);

            foreach (var field in layoutRecord.Properties) {
                var fieldCategory = field.Category;
                var fieldType = field.Type;

                var f = allFields.FirstOrDefault(x => fieldCategory == x.Category && fieldType == x.Type);
                if (f != null) {
                    fieldEntries.Add(
                        new PropertyEntry {
                            Category = f.Category,
                            Type = f.Type,
                            PropertyRecordId = field.Id,
                            DisplayText = String.IsNullOrWhiteSpace(field.Description) ? f.Display(new PropertyContext { State = FormParametersHelper.ToDynamic(field.State) }).Text : field.Description,
                            Position = field.Position
                        });
                }
            }

            model.Properties = fieldEntries.OrderBy(f => f.Position);
            #endregion

            return View(model);
        }
    }
}