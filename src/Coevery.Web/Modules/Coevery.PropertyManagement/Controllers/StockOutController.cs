using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.Services;
using Coevery.PropertyManagement.ViewModels;
using Coevery.Themes;
using Coevery.UI.Notify;
using Newtonsoft.Json;
using Coevery.ContentManagement;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class StockOutController : Controller
    {
        private readonly IRepository<InventoryChangeRecord> _inventoryChangeRepository;
        private readonly IRepository<InventoryRecord> _inventoryRepository;
        private readonly IInventoryService _inventoryService;
        private readonly IRepository<MaterialPartRecord> _materialRepository;
        private readonly IVoucherNumberService _voucherNumberService;

        public StockOutController(ICoeveryServices services,
            IRepository<InventoryRecord> inventoryRepository,
            IRepository<MaterialPartRecord> materialRepository,
            IInventoryService inventoryService,
            IRepository<InventoryChangeRecord> inventoryChangeRepository,
            IVoucherNumberService voucherNumberService
            )
        {
            Services = services;
            _inventoryRepository = inventoryRepository;
            _materialRepository = materialRepository;
            _inventoryService = inventoryService;
            _inventoryChangeRepository = inventoryChangeRepository;
            _voucherNumberService = voucherNumberService;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult Create()
        {
            var user = Services.WorkContext.CurrentUser;
            var voucherNo = _voucherNumberService.DispayGenerateNumber();
            var createView = new VoucherCreateViewModel();
            createView.OperatorName = user.UserName;
            createView.VoucherNo = voucherNo;
            return View(createView);
        }


        [HttpPost]
        public ActionResult StockOutCreate(VoucherCreateViewModel model)
        {
            var details = model.InventoryDetailEntities;
            bool isValid = true;
            int[] ids = details.Select(x => x.MaterialId).ToArray();
            Dictionary<int, InventoryRecord> inventoryRecords = _inventoryRepository.Table
                .Where(x => ids.Contains(x.MaterialId))
                .Distinct()
                .ToDictionary(x => x.MaterialId);
            foreach (var detail in details)
            {
                InventoryRecord inventory = inventoryRecords[detail.MaterialId];
                if (inventory.Number < detail.Number)
                {
                    isValid = false;
                    break;
                }
            }
            if (!isValid)
            {
                Services.Notifier.Information(T("库存数量不够！"));
                return Json(new {Result = false});
            }
            var user = Services.WorkContext.CurrentUser;
            var voucherNo = _voucherNumberService.GenerateNumber();
            ContentItem contentItem = Services.ContentManager.New("Voucher");
            var part = contentItem.As<VoucherPart>();
            VoucherPartRecord record = part.Record;
            record.VoucherNo = voucherNo;
            record.Operation = InventoryChangeOperation.出库;
            record.Date = DateTime.Now;
            record.OperatorId = user.Id;
            record.DepartmentId = model.DepartmentId;
            record.Remark = model.Remark;
            Services.ContentManager.Create(contentItem, VersionOptions.Draft);
            Services.ContentManager.Publish(contentItem);
            foreach (var detail in details)
            {
                InventoryRecord inventory = inventoryRecords[detail.MaterialId];
                detail.CostPrice = inventory.Amount/inventory.Number;
                _inventoryService.DecreaseInventory(detail.MaterialId, detail.Number, detail.CostPrice);
                _inventoryChangeRepository.Create(new InventoryChangeRecord
                {
                    VoucherNo = voucherNo,
                    MaterialId = detail.MaterialId,
                    Operation = InventoryChangeOperation.出库,
                    CostPrice = detail.CostPrice,
                    Number = detail.Number,
                    Date = DateTime.Today,
                    OperatorId = user.Id
                });
            }
            return Json(new {Result = true, Url = Url.Action("StockOutVoucherPreview", new {voucherNo})});
        }

        public ActionResult StockOutVoucherPreview(string voucherNo)
        {
            List<InventoryChangeRecord> queryInventoryChange =
                _inventoryChangeRepository.Table.Where(u => u.VoucherNo == voucherNo).OrderBy(u => u.Id).ToList();
            var model = new InventoryCreateListViewModel
            {
                VoucherNo = voucherNo,
                List = new List<HelpListViewModel>()
            };
            Dictionary<int, MaterialPart> materials =
                Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            for (int i = 0; i < queryInventoryChange.Count; i++)
            {
                if (materials.ContainsKey(queryInventoryChange[i].MaterialId))
                {
                    MaterialPart material = materials[queryInventoryChange[i].MaterialId];
                    model.List.Add(new HelpListViewModel
                    {
                        Id = i + 1,
                        Material_Name = material.Name,
                        Material_Brand = material.Brand,
                        Material_Model = material.Model,
                        Material_SerialNo = material.SerialNo,
                        Material_Unit = material.Unit,
                        CostPrice = queryInventoryChange[i].CostPrice,
                        Number = queryInventoryChange[i].Number
                    });
                }
            }
            return View(model);
        }

        public ActionResult StockOutVoucherHistoryView(int id)
        {
            var qVoucher =
                Services.ContentManager.Query<VoucherPart, VoucherPartRecord>().List()
                    .FirstOrDefault(x => x.Id == id);
            List<InventoryChangeRecord> queryInventoryChange =
                _inventoryChangeRepository.Table.Where(u => u.VoucherNo == qVoucher.VoucherNo)
                    .OrderBy(u => u.Id)
                    .ToList();
            var qDepartment =
                Services.ContentManager.Query<DepartmentPart, DepartmentPartRecord>().List().ToDictionary(x => x.Id);
            var model = new InventoryCreateListViewModel
            {
                VoucherNo = qVoucher.VoucherNo,
                List = new List<HelpListViewModel>()
            };
            Dictionary<int, MaterialPart> materials =
                Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            for (int i = 0; i < queryInventoryChange.Count; i++)
            {
                if (materials.ContainsKey(queryInventoryChange[i].MaterialId))
                {
                    MaterialPart material = materials[queryInventoryChange[i].MaterialId];
                    model.List.Add(new HelpListViewModel
                    {
                        Id = i + 1,
                        Material_Name = material.Name,
                        Material_Brand = material.Brand,
                        Material_Model = material.Model,
                        Material_SerialNo = material.SerialNo,
                        Material_Unit = material.Unit,
                        CostPrice = queryInventoryChange[i].CostPrice,
                        Number = queryInventoryChange[i].Number,
                        DepartmentName =qDepartment.Count>0? qDepartment[qVoucher.DepartmentId].Name:""
                    });
                }
            }
            return View(model);
        }

        public ActionResult List()
        {
            var departments= Services.ContentManager.Query<DepartmentPart, DepartmentPartRecord>().List()
               .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
            departments.Insert(0, new SelectListItem());
            ViewBag.departments = departments;
            return View();
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
            var qDepartment =
               Services.ContentManager.Query<DepartmentPart, DepartmentPartRecord>().List().ToDictionary(x => x.Id);
            var query = Services.ContentManager.Query<VoucherPart, VoucherPartRecord>();
            #region Filter
            var options = new InventoryFilterOptions();
            UpdateModel(options, "FilterOptions", input);
            if (options.BeginDate.HasValue)
            {
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            if (options.EndDate.HasValue)
            {
                query = query.Where(x => x.Date <= options.EndDate);
            }
            if (!String.IsNullOrWhiteSpace(options.VoucherNo))
            {
                query = query.Where(u => u.VoucherNo.Contains(options.VoucherNo));
            }
            if (options.DepartmentId.HasValue)
            {
                query = query.Where(u => u.DepartmentId == options.DepartmentId);
            }

            #endregion

            var totalRecords = query.Count();
            var records = query
                .Where(x => x.Operation == InventoryChangeOperation.出库)
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1)*pageSize, pageSize)
                .Select(item => new VoucherListViewModel
                {
                    Id = item.Record.Id,
                    VoucherNo = item.Record.VoucherNo,
                    Operation = item.Record.Operation.ToString(),
                    Date = item.Record.Date.ToString("MM/dd/yyyy"),
                    DepartmentName =qDepartment.Count>0? qDepartment[item.DepartmentId].Name:"",
                    Remark = item.Record.Remark
                }).ToList();
            var result = new
            {
                page,
                totalPages = Math.Ceiling((double) totalRecords/pageSize),
                totalRecords,
                rows = records
            };
            return Json(result);
        }

        [HttpPost]
        public ActionResult DepartmentDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc")
        {
            IContentQuery<DepartmentPart, DepartmentPartRecord> query =
                Services.ContentManager.Query<DepartmentPart, DepartmentPartRecord>();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.Name.Contains(term));
            }
            int totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1)*pageSize, pageSize)
                .Select(item => new
                {
                    id = item.Record.ContentItemRecord.Id,
                    text = item.Name
                }).ToList();
            return Json(new {records, total = totalRecords});
        }
    }
}