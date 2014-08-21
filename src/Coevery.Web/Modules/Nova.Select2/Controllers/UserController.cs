

using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Coevery;
using Coevery.UI.Notify;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Themes;
using Coevery.Localization;
using Coevery.Core.OptionSet.Services;
using Nova.Select2.Models;
using Nova.Select2.ViewModels;

namespace Nova.Select2.Controllers {
	[Themed]
    public class UserController : Controller , IUpdateModel{
        private readonly ITransactionManager _transactionManager;
		private readonly IOptionSetService _optionSetService;

        public UserController (ICoeveryServices services, 
			ITransactionManager transactionManager, 
            IOptionSetService optionSetService) {
            Services = services;
            _transactionManager = transactionManager;
			_optionSetService = optionSetService;
            T = NullLocalizer.Instance;
        }

		public ICoeveryServices Services { get; set; }
		public Localizer T { get; set; }

		public ActionResult Index() {
            return List();
        }

		public ActionResult List() {
            var contentItem = Services.ContentManager.New("User");
            contentItem.Weld(new UserPart());
            var model = Services.ContentManager.BuildDisplay(contentItem, "List");
            return View("List", model);
        }

		[HttpPost]
		public ActionResult List(int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc") {
	        var query = Services.ContentManager.Query<UserPart, UserPartRecord>();
	        var totalRecords = query.Count();
	        var records = query
	            .OrderBy(sortBy, sortOrder)
	            .Slice((page - 1)*pageSize, pageSize)
	            .Select(item => new UserListViewModel{
					Id = item.Record.ContentItemRecord.Id,
					VersionId = item.Record.Id,
				}).ToList();
	        var result = new {
	            page,
	            totalPages = totalRecords/pageSize,
	            totalRecords,
	            rows = records
	        };
	        return Json(result);
	    }

		private DateTime? SpecifyDateTimeKind(DateTime? utcDateTime) {
            if (utcDateTime != null)
                return DateTime.SpecifyKind(utcDateTime.Value, DateTimeKind.Utc);
	        return null;
	    }

		private string GetOptionSetFieldValue(int id, string fieldName) {
            var optionItems = _optionSetService.GetOptionItemsForContentItem(id, fieldName).ToList();
            var value = string.Join(", ", optionItems.Select(t => t.Name).ToArray());
	        return value;
	    }

        public ActionResult Detail(int id) {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null) {
                return HttpNotFound();
            }

            dynamic model = Services.ContentManager.BuildDisplay(contentItem, "Detail");
            return View((object) model);
        }

        public ActionResult Create() {
            var contentItem = Services.ContentManager.New("User");
            var model = Services.ContentManager.BuildEditor(contentItem, "Create");
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(string returnUrl) {
            var contentItem = Services.ContentManager.New("User");
            dynamic model = Services.ContentManager.UpdateEditor(contentItem, this, "Create");
            if (!ModelState.IsValid) {
                return View("Create", (object) model);
            }
            Services.ContentManager.Create(contentItem, VersionOptions.Draft);
            Services.ContentManager.Publish(contentItem);
            Services.Notifier.Information(T("User created"));
            return RedirectToAction("Edit", new { id = contentItem.Id, returnUrl });
        }

        public ActionResult Edit(int id) {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null) {
                return HttpNotFound();
            }

            dynamic model = Services.ContentManager.BuildEditor(contentItem, "Edit");
            return View((object)model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int id, string returnUrl) {
			var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null) {
                return HttpNotFound();
            }

            dynamic model = Services.ContentManager.UpdateEditor(contentItem, this, "Edit");
            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                return View("Edit", (object) model);
            }
            Services.Notifier.Information(T("User information updated"));
            return RedirectToAction("Edit", new {id, returnUrl});
        }

        public ActionResult Delete(int id) {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(List<int> selectedIds){
            try {
                var items = Services.ContentManager.Query().ForContentItems(selectedIds).List();
                foreach (var item in items) {
                    Services.ContentManager.Remove(item);
                }
				Services.Notifier.Information(T("Delete succeeded!"));
                return new HttpStatusCodeResult(HttpStatusCode.OK, T("Delete succeeded").Text);
            }
            catch {
			    Services.Notifier.Error(T("Delete failed!"));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, T("Delete failed").Text);
            }
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}

