﻿

using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Coevery;
using Coevery.Security;
using Coevery.UI.Notify;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Themes;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;

namespace Coevery.PropertyManagement.Controllers
{
	[Themed]
    public class MeterTypeController : Controller , IUpdateModel{
        private readonly ITransactionManager _transactionManager;

        public MeterTypeController (ICoeveryServices services, 
			ITransactionManager transactionManager) {
            Services = services;
            _transactionManager = transactionManager;
            T = NullLocalizer.Instance;
        }

		public ICoeveryServices Services { get; set; }
		public Localizer T { get; set; }

		public ActionResult Index() {
            return List();
        }

		public ActionResult List() {
            var contentItem = Services.ContentManager.New("MeterType");
            if (!Services.Authorizer.Authorize(StandardPermissions.View, contentItem, T("没有仪表类型查看权限！")))
                return new HttpUnauthorizedResult();
            contentItem.Weld(new MeterTypePart());
            var model = Services.ContentManager.BuildDisplay(contentItem, "List");
            return View("List", model);
        }

		[HttpPost]
		public ActionResult List(int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc") {
            if (!Services.Authorizer.Authorize(StandardPermissions.View, Services.ContentManager.New("MeterType"), T("没有仪表类型查看权限！")))
                return new HttpUnauthorizedResult();
	        var query = Services.ContentManager.Query<MeterTypePart, MeterTypePartRecord>();
	        var totalRecords = query.Count();
		    var records = query
		        .OrderBy(sortBy, sortOrder)
		        .Slice((page - 1)*pageSize, pageSize)
		        .Select(item => new MeterTypeListViewModel {
		            Id = item.Record.ContentItemRecord.Id,
		            VersionId = item.Record.Id,
		            Name = item.Record.Name,
		            Unit = item.Record.Unit,
		            Description = item.Record.Description,
		        }).ToList();
	        var result = new {
	            page,
	            totalPages = Math.Ceiling((double)totalRecords/pageSize),
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


        public ActionResult Detail(int id) {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null) {
                return HttpNotFound();
            }

            dynamic model = Services.ContentManager.BuildDisplay(contentItem, "Detail");
            return View((object) model);
        }

        public ActionResult Create() {
            var contentItem = Services.ContentManager.New("MeterType");
            if (!Services.Authorizer.Authorize(StandardPermissions.Create, contentItem, T("没有仪表类型创建权限！")))
                return new HttpUnauthorizedResult();
            var model = Services.ContentManager.BuildEditor(contentItem, "Create");
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(string returnUrl) {
            var contentItem = Services.ContentManager.New("MeterType");
            if (!Services.Authorizer.Authorize(StandardPermissions.Create, contentItem, T("没有仪表类型创建权限！")))
                return new HttpUnauthorizedResult();
            dynamic model = Services.ContentManager.UpdateEditor(contentItem, this, "Create");
            if (!ModelState.IsValid) {
                return View("Create", (object) model);
            }
            var meterTypes = Services.ContentManager.Query<MeterTypePart, MeterTypePartRecord>().List().ToList();
            var meterType = contentItem.As<MeterTypePart>().Record;
            if (meterTypes.Any(m => m.Name.Trim() == meterType.Name.Trim()))
            {
                Services.Notifier.Information(T("不能创建相同的仪表！"));
                return View("Create", (object)model);
            }
            Services.ContentManager.Create(contentItem, VersionOptions.Draft);
            Services.ContentManager.Publish(contentItem);
            Services.Notifier.Information(T("创建仪表成功！"));
            return RedirectToAction("List");
        }

        public ActionResult Edit(int id) {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null) {
                return HttpNotFound();
            }
            if (!Services.Authorizer.Authorize(StandardPermissions.Edit, contentItem, T("没有仪表类型编辑权限！")))
                return new HttpUnauthorizedResult();
            dynamic model = Services.ContentManager.BuildEditor(contentItem, "Edit");
            return View((object)model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int id, string returnUrl) {
			var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null) {
                return HttpNotFound();
            }
            if (!Services.Authorizer.Authorize(StandardPermissions.Edit, contentItem, T("没有仪表类型编辑权限！")))
                return new HttpUnauthorizedResult();
            dynamic model = Services.ContentManager.UpdateEditor(contentItem, this, "Edit");
            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                return View("Edit", (object) model);
            }
            //编辑页面跟别人比较，排除自己
            var meterTypes = Services.ContentManager.Query<MeterTypePart, MeterTypePartRecord>().List()
                             .Where(m => m.Id != id)
                             .ToList();
            var meterType = contentItem.As<MeterTypePart>().Record;
            if (meterTypes.Any(m => m.Name.Trim() == meterType.Name.Trim()))
            {
                Services.Notifier.Information(T("该仪表名称已存在，修改失败！"));
                _transactionManager.Cancel();
                return View("Edit", (object)model);
            }
            Services.Notifier.Information(T("更新仪表成功！"));
            return RedirectToAction("List");
        }


        [HttpPost]
        public ActionResult Delete(List<int> selectedIds){
            if (!Services.Authorizer.Authorize(StandardPermissions.Delete, Services.ContentManager.New("MeterType"), T("没有仪表类型删除权限！")))
                return new HttpUnauthorizedResult();
            try {
                var items = Services.ContentManager.Query().ForContentItems(selectedIds).List();
                foreach (var item in items) {
                    Services.ContentManager.Remove(item);
                }
				Services.Notifier.Information(T("删除成功！"));
                return new HttpStatusCodeResult(HttpStatusCode.OK, T("删除成功！").Text);
            }
            catch {
                Services.Notifier.Error(T("删除失败！"));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, T("删除失败！").Text);
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
