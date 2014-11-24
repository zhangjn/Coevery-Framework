using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.FileSystems.AppData;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.Services;
using Coevery.PropertyManagement.ViewModels;
using Coevery.Themes;


namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class InventoryController : Controller
    {
        private readonly IVoucherNumberService _voucherNumberService;
        private readonly IInventoryService _inventoryService;
        private readonly IRepository<InventoryChangeRecord> _repositoryInventoryChange;

        public InventoryController(
            ICoeveryServices services,
            IInventoryService inventoryService,
            IRepository<InventoryChangeRecord> repositoryInventoryChange,
            IVoucherNumberService voucherNumberService)
        {
            Services = services;
            _inventoryService = inventoryService;
            _repositoryInventoryChange = repositoryInventoryChange;
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
        public ActionResult InventoryCreate(VoucherCreateViewModel model)
        {
            var details = model.InventoryDetailEntities;
            var user = Services.WorkContext.CurrentUser;
            var voucherNo = _voucherNumberService.GenerateNumber();
            ContentItem contentItem = Services.ContentManager.New("Voucher");
            var part = contentItem.As<VoucherPart>();
            VoucherPartRecord record = part.Record;
            record.VoucherNo = voucherNo;
            record.Operation = InventoryChangeOperation.入库;
            record.Date = DateTime.Now;
            record.OperatorId = user.Id;
            record.SupplierId = model.SupplierId;
            // record.Supplier = Services.ContentManager.Get<SupplierPart>(model.SupplierId).Record;
            Services.ContentManager.Create(contentItem, VersionOptions.Draft);
            Services.ContentManager.Publish(contentItem);
            foreach (var detail in details)
            {
                _inventoryService.IncreaseInventory(new InventoryInfo
                {
                    MaterialId = detail.MaterialId,
                    CostPrice = detail.CostPrice
                }, detail.Number);

                _repositoryInventoryChange.Create(new InventoryChangeRecord
                {
                    VoucherNo = voucherNo,
                    MaterialId = detail.MaterialId,
                    Operation = InventoryChangeOperation.入库,
                    CostPrice = detail.CostPrice,
                    Number = detail.Number,
                    Date = DateTime.Today
                    // OperatorId = user.Id
                });
            }

            return Json(Url.Action("InventoryCreateVoucherPreview", new {voucherNo = voucherNo}));
        }

        public ActionResult InventoryCreateVoucherPreview(string voucherNo)
        {
            var qMaterial =
                Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            var qInventoryChange =
                _repositoryInventoryChange.Table.Where(u => u.VoucherNo == voucherNo).OrderBy(u => u.Id).ToList();
            var inventoryCreateModel = new InventoryCreateListViewModel();
            inventoryCreateModel.VoucherNo = voucherNo;
            inventoryCreateModel.List = new List<HelpListViewModel>();
            for (int i = 0; i < qInventoryChange.Count; i++)
            {
                var helpModel = new HelpListViewModel();
                helpModel.Id = i + 1;
                var inventoryChange = qInventoryChange[i];
                if (qMaterial.ContainsKey(inventoryChange.MaterialId))
                {
                    var material = qMaterial[inventoryChange.MaterialId];
                    helpModel.Material_Name = material.Name;
                    helpModel.Material_Brand = material.Brand;
                    helpModel.Material_Model = material.Model;
                    helpModel.Material_Unit = material.Unit;
                    helpModel.Material_SerialNo = material.SerialNo;
                }
                helpModel.Number = qInventoryChange[i].Number;
                helpModel.CostPrice = qInventoryChange[i].CostPrice;
                inventoryCreateModel.List.Add(helpModel);
            }
            return View(inventoryCreateModel);
        }

        public ActionResult InventoryVoucherHistoryView(int id)
        {
            var qMaterial =Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            var qVoucher =Services.ContentManager.Query<VoucherPart, VoucherPartRecord>().List().FirstOrDefault(x => x.Id == id);
            var qSupplier =Services.ContentManager.Query<SupplierPart, SupplierPartRecord>().List().ToDictionary(x => x.Id);
            var qInventoryChange =_repositoryInventoryChange.Table.Where(u => u.VoucherNo == qVoucher.VoucherNo).OrderBy(u => u.Id).ToList();
            var inventoryCreateModel = new InventoryCreateListViewModel();
            inventoryCreateModel.VoucherNo = qVoucher.VoucherNo;
            inventoryCreateModel.List = new List<HelpListViewModel>();
            for (int i = 0; i < qInventoryChange.Count; i++)
            {
                var helpModel = new HelpListViewModel();
                helpModel.Id = i + 1;
                var inventoryChange = qInventoryChange[i];
                if (qMaterial.ContainsKey(inventoryChange.MaterialId))
                {
                    var material = qMaterial[inventoryChange.MaterialId];
                    helpModel.Material_Name = material.Name;
                    helpModel.Material_Brand = material.Brand;
                    helpModel.Material_Model = material.Model;
                    helpModel.Material_Unit = material.Unit;
                    helpModel.Material_SerialNo = material.SerialNo;
                }
                helpModel.Number = qInventoryChange[i].Number;
                helpModel.CostPrice = qInventoryChange[i].CostPrice;
                helpModel.SupplierName =qSupplier.ContainsKey(qVoucher.SupplierId)? qSupplier[qVoucher.SupplierId].Name:"";

                inventoryCreateModel.List.Add(helpModel);
            }
            return View(inventoryCreateModel);
        }

        public ActionResult List()
        {
            var suppliers = Services.ContentManager.Query<SupplierPart, SupplierPartRecord>().List()
                .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();
            suppliers.Insert(0, new SelectListItem());
            ViewBag.suppliers = suppliers;
            return View();
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
            var qSupplier =
                Services.ContentManager.Query<SupplierPart, SupplierPartRecord>().List().ToDictionary(x => x.Id);
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
            if (options.SupplierId.HasValue)
            {
                query = query.Where(u => u.SupplierId == options.SupplierId);
            }

            #endregion

            var totalRecords = query.Count();
            var records = query
                .Where(x => x.Operation == InventoryChangeOperation.入库)
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1)*pageSize, pageSize)
                .Select(item => new VoucherListViewModel
                {
                    Id = item.Record.Id,
                    VoucherNo = item.Record.VoucherNo,
                    Operation = item.Record.Operation.ToString(),
                    Date = item.Record.Date.ToString("MM/dd/yyyy"),
                    SupplierName =qSupplier.ContainsKey(item.SupplierId)? qSupplier[item.SupplierId].Name:"",
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
        public ActionResult SupplierDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc")
        {
            IContentQuery<SupplierPart, SupplierPartRecord> query =
                Services.ContentManager.Query<SupplierPart, SupplierPartRecord>();
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