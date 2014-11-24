using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Coevery.Data;
using Coevery.FileSystems.AppData;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;
using Coevery.Themes;
using Coevery.UI.Notify;
using Coevery.ContentManagement;


namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class MaterialController : Controller, IUpdateModel
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<InventoryRecord> _inventoryRepository;
       // private readonly IMaterialListViewService _materialListViewService;
        private readonly IAppDataFolder _appDataFolder;
        private const string MaterialFileName = "MaterialNo.txt";

        public MaterialController(ICoeveryServices services,
            ITransactionManager transactionManager,
           // IMaterialListViewService materialListViewService,
            IAppDataFolder appDataFolder, 
            IRepository<InventoryRecord> inventoryRepository)
        {
            Services = services;
            _transactionManager = transactionManager;
           // _materialListViewService = materialListViewService;
            _appDataFolder = appDataFolder;
            _inventoryRepository = inventoryRepository;
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
            var contentItem = Services.ContentManager.New("Material");
            contentItem.Weld(new MaterialPart());
            var model = Services.ContentManager.BuildDisplay(contentItem, "List");
            return View("List", model);
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc")
        {
            var query = Services.ContentManager.Query<MaterialPart, MaterialPartRecord>();

            #region Filter

            var options = new MaterialFilterOptions();
            UpdateModel(options, "FilterOptions", input);
            if (!String.IsNullOrWhiteSpace(options.SerialNo))
            {
                query = query.Where(u => u.SerialNo.Contains(options.SerialNo));
            }

            if (!String.IsNullOrWhiteSpace(options.Name))
            {
                query = query.Where(u => u.Name.Contains(options.Name));
            }

            if (!String.IsNullOrWhiteSpace(options.Brand))
            {
                query = query.Where(u => u.Brand.Contains(options.Brand));
            }

            if (!String.IsNullOrWhiteSpace(options.Model))
            {
                query = query.Where(u => u.Model.Contains(options.Model));
            }

            if (!String.IsNullOrWhiteSpace(options.Unit))
            {
                query = query.Where(u => u.Unit.Contains(options.Unit));
            }

            #endregion

            var totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1) * pageSize, pageSize)
                .Select(item => new MaterialListViewModel
                {
                    
                    Id = item.Record.Id,
                    SerialNo = item.Record.SerialNo,
                    Name = item.Record.Name,
                    Brand = item.Record.Brand,
                    Model = item.Record.Model,
                    Unit = item.Record.Unit,
                    Remark = item.Record.Remark
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

        [HttpPost]
        public ActionResult StockOutMaterialList(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc")
        {
            var query = Services.ContentManager.Query<MaterialPart, MaterialPartRecord>();

            #region Filter

            var options = new MaterialFilterOptions();
            UpdateModel(options, "FilterOptions", input);
            if (!String.IsNullOrWhiteSpace(options.SerialNo))
            {
                query = query.Where(u => u.SerialNo.Contains(options.SerialNo));
            }

            if (!String.IsNullOrWhiteSpace(options.Name))
            {
                query = query.Where(u => u.Name.Contains(options.Name));
            }

            if (!String.IsNullOrWhiteSpace(options.Brand))
            {
                query = query.Where(u => u.Brand.Contains(options.Brand));
            }

            if (!String.IsNullOrWhiteSpace(options.Model))
            {
                query = query.Where(u => u.Model.Contains(options.Model));
            }

            if (!String.IsNullOrWhiteSpace(options.Unit))
            {
                query = query.Where(u => u.Unit.Contains(options.Unit));
            }

            #endregion

            var inventory = _inventoryRepository.Table.ToList();
            int[] ids = inventory.Select(x => x.MaterialId).ToArray();
            var totalRecords = query.Count();
            var records = query
                .Where(x => ids.Contains(x.Id))
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1) * pageSize, pageSize)
                .Select(item => new MaterialListViewModel
                {

                    Id = item.Record.Id,
                    SerialNo = item.Record.SerialNo,
                    Name = item.Record.Name,
                    Brand = item.Record.Brand,
                    Model = item.Record.Model,
                    Unit = item.Record.Unit,
                    Remark = item.Record.Remark,
                    StockPrice = inventory.FirstOrDefault(x => x.MaterialId == item.Record.Id).CostPrice
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
           
            int count = 0;
            if (_appDataFolder.FileExists(MaterialFileName))
            {
                int.TryParse(_appDataFolder.ReadFile(MaterialFileName), out count);
            }

            var materials = Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToList();
            string serialNo;
            while (true)
            {
                count++;
                serialNo = count.ToString("D8");
                bool exist = materials.Any(x => x.SerialNo == serialNo);
                if (!exist)
                {
                    break;
                }
            }
            var contentItem = Services.ContentManager.New<MaterialPart>("Material");
            contentItem.SerialNo = serialNo;
            var model = Services.ContentManager.BuildEditor(contentItem, "Create");
            _appDataFolder.CreateFile(MaterialFileName, (count - 1).ToString());
         
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(string returnUrl)
        {
            
            var contentItem = Services.ContentManager.New<MaterialPart>("Material");
            dynamic model = Services.ContentManager.UpdateEditor(contentItem, this, "Create");
            var serialNo = contentItem.SerialNo;
            var count = Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().Where(x => x.SerialNo == serialNo).Count();
            if (count > 0)
            {
                _transactionManager.Cancel();
                Services.Notifier.Error(T("已经存在相同的材料编号！"));
                return View("Create", (object)model);
            }
           
            Services.ContentManager.Create(contentItem, VersionOptions.Published);
            Services.Notifier.Information(T("新建材料成功！"));

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
           
            var contentItem = Services.ContentManager.Get<MaterialPart>(id, VersionOptions.Latest);
            if (contentItem == null)
            {
                return HttpNotFound();
            }

            dynamic model = Services.ContentManager.UpdateEditor(contentItem, this, "Edit");
            var serialNo = contentItem.SerialNo;
            var count = Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().Where(x => x.SerialNo == serialNo).Count();
            if (count > 1)
            {
                _transactionManager.Cancel();
                Services.Notifier.Error(T("已经存在相同的材料编号！"));
                return View("Edit", (object)model);
            }
          
            Services.Notifier.Information(T("修改材料成功！"));
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