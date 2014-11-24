using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;
using Coevery.Themes;
using Coevery.ContentManagement;
using Coevery.UI.Notify;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class SupplierController : Controller, IUpdateModel
    {
        private readonly ITransactionManager _transactionManager;
      
        public SupplierController(ICoeveryServices services,
            ITransactionManager transactionManager
           )
        {
            Services = services;
            _transactionManager = transactionManager;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index()
        {
       
            return List();
        }

        public ActionResult List()
        {
            var contentItem = Services.ContentManager.New("Supplier");
            contentItem.Weld(new SupplierPart());
            var model = Services.ContentManager.BuildDisplay(contentItem, "List");
            return View("List", model);
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc")
        {
            var query = Services.ContentManager.Query<SupplierPart, SupplierPartRecord>();

            //Filter
            var options = new SupplierFilterOptions();
            UpdateModel(options, "FilterOptions", input);
            if (!String.IsNullOrWhiteSpace(options.Search))
            {
                query = query.Where(u => u.Name.Contains(options.Search));
            }

            if (!String.IsNullOrWhiteSpace(options.Contactor))
            {
                query = query.Where(u => u.Contactor.Contains(options.Contactor));
            }

            var totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1) * pageSize, pageSize)
                .Select(item => new SupplierListViewModel
                {
                    Id = item.Record.Id,
                    Name = item.Record.Name,
                    Address = item.Record.Address,
                    Contactor = item.Record.Contactor,
                    Tel = item.Record.Tel,
                    MobilePhone = item.Record.MobilePhone,
                    Email = item.Record.Email,
                    QQ = item.Record.QQ,
                    Fax = item.Record.Fax,
                    Description = item.Record.Description
                }).ToList();
            var result = new
            {
                page,
                totalPages = Math.Ceiling((double)totalRecords / pageSize),
                totalRecords,
                rows = records
            };
            return Json(result);
        }

        
        public ActionResult Create()
        {
           

            var contentItem = Services.ContentManager.New("Supplier");
            var model = Services.ContentManager.BuildEditor(contentItem, "Create");
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(string returnUrl)
        {
            
            var contentItem = Services.ContentManager.New("Supplier");
            dynamic model = Services.ContentManager.UpdateEditor(contentItem, this, "Create");
            if (!ModelState.IsValid)
            {
                return View("Create", (object)model);
            }
            Services.ContentManager.Create(contentItem, VersionOptions.Draft);
            Services.ContentManager.Publish(contentItem);
            Services.Notifier.Information(T("供应商创建成功!"));
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
          

            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null)
            {
                return HttpNotFound();
            }

            dynamic model = Services.ContentManager.BuildEditor(contentItem, "Edit");
            return View((object)model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int id, string returnUrl)
        {
         
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null)
            {
                return HttpNotFound();
            }

            dynamic model = Services.ContentManager.UpdateEditor(contentItem, this, "Edit");
            if (!ModelState.IsValid)
            {
                _transactionManager.Cancel();
                return View("Edit", (object)model);
            }
            Services.Notifier.Information(T("更新信息成功!"));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(List<int> selectedIds)
        {
         
            try
            {
                var items = Services.ContentManager.Query().ForContentItems(selectedIds).List();
                foreach (var item in items)
                {
                    Services.ContentManager.Remove(item);
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                Services.Notifier.Error(T("删除失败！"));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

      

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}