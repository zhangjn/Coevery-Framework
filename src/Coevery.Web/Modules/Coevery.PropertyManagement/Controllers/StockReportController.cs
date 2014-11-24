using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Extensions;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.Services;
using Coevery.PropertyManagement.ViewModels;
using Coevery.Themes;
using NPOI.HSSF.UserModel;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class StockReportController : Controller, IUpdateModel
    {
        private readonly IRepository<InventoryChangeRecord> _inventoryChangeRepository;
        private readonly IStockReportService _stockReportService;
        private readonly IReportServices _reportServices;
        public StockReportController(ICoeveryServices services,
            IRepository<InventoryChangeRecord> inventoryChangeRepository, 
            IStockReportService stockReportService, 
            IReportServices reportServices)
        {
            Services = services;
            _inventoryChangeRepository = inventoryChangeRepository;
            _stockReportService = stockReportService;
            _reportServices = reportServices;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult StockInDetailList()
        {
            var materials = Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List()
             .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
            materials.Insert(0, new SelectListItem());
            ViewBag.materials = materials;
            return View("StockInDetailList");
        }

        [HttpPost]
        public ActionResult StockInDetailList(FormCollection input, int page = 1, int pageSize = 10,
            string sortBy = null, string sortOrder = "asc")
        {
            var qVoucher = Services.ContentManager.Query<VoucherPart, VoucherPartRecord>().List().ToDictionary(x => x.VoucherNo);
             var qSupplier =Services.ContentManager.Query<SupplierPart, SupplierPartRecord>().List().ToDictionary(x => x.Id);
            var options = new StockFilterOptions();
            UpdateModel(options, "Filter", input);
            Dictionary<int, MaterialPart> materials =
            Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            var query = _inventoryChangeRepository.Table.OrderBy(sortBy, sortOrder);
            if (options.BeginDate.HasValue)
            {
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            else
            {
                options.BeginDate = DateTime.Now;
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            if (options.EndDate.HasValue)
            {
                query = query.Where(x => x.Date <= options.EndDate);
            }
            if (options.MaterialId.HasValue)
            {
                query = query.Where(x => x.MaterialId == options.MaterialId);
            }
            var inventoryChangeRecords = query as IList<InventoryChangeRecord> ?? query.ToList();
            var totalRecords = inventoryChangeRecords.Count();
            var records = inventoryChangeRecords
                .Where(x => x.Operation == InventoryChangeOperation.入库)
                .Skip((page - 1)*pageSize)
                .Take(pageSize)
                .Select(item => new StockDetailListViewModel
                {
                    Id = item.Id,
                    VoucherNo = item.VoucherNo,
                    MaterialSerialNo = materials[item.MaterialId].SerialNo,
                    MaterialName = materials[item.MaterialId].Name,
                    MaterialUnit = materials[item.MaterialId].Unit,
                    CostPrice = item.CostPrice,
                    Number = item.Number,
                    SupplierName = qSupplier[qVoucher[item.VoucherNo].SupplierId].Name,
                    Date = item.Date.ToString("MM/dd/yyyy")
                    //Remark = item.Remark
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

        public ActionResult StockOutDetailList()
        {
            return View("StockOutDetailList");
        }

        [HttpPost]
        public ActionResult StockOutDetailList(FormCollection input, int page = 1, int pageSize = 10,
            string sortBy = null, string sortOrder = "asc")
        {
            var qVoucher = Services.ContentManager.Query<VoucherPart, VoucherPartRecord>().List().ToDictionary(x => x.VoucherNo);
            var qDepartment =Services.ContentManager.Query<DepartmentPart, DepartmentPartRecord>().List().ToDictionary(x => x.Id);
            var options = new StockFilterOptions();
            UpdateModel(options, "Filter", input);
            Dictionary<int, MaterialPart> materials =
             Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            var query = _inventoryChangeRepository.Table.OrderBy(sortBy, sortOrder);
            if (options.BeginDate.HasValue)
            {
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            else
            {
                options.BeginDate = DateTime.Now;
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            if (options.EndDate.HasValue)
            {
                query = query.Where(x => x.Date <= options.EndDate);
            }
            if (options.MaterialId.HasValue)
            {
                query = query.Where(x => x.MaterialId == options.MaterialId);
            }
            var inventoryChangeRecords = query as IList<InventoryChangeRecord> ?? query.ToList();
            var totalRecords = inventoryChangeRecords.Count();
            var records = inventoryChangeRecords
                .Where(x => x.Operation == InventoryChangeOperation.出库)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(item => new StockDetailListViewModel
                {
                    Id = item.Id,
                    VoucherNo = item.VoucherNo,
                    MaterialSerialNo = materials[item.MaterialId].SerialNo,
                    MaterialName = materials[item.MaterialId].Name,
                    MaterialUnit = materials[item.MaterialId].Unit,
                    CostPrice = item.CostPrice,
                    Number = item.Number,
                    DepartmentName = qDepartment[qVoucher[item.VoucherNo].DepartmentId].Name,
                    Date = item.Date.ToString("MM/dd/yyyy")
                    //Remark = item.Remark
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

        public ActionResult StockSumarryList()
        {
            return View("StockSumarryList");
        }

        [HttpPost]
        public ActionResult StockSumarryList(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
            var options = new StockFilterOptions();
            UpdateModel(options, "Filter", input);
            Dictionary<int, MaterialPart> materials =
             Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            var query = _inventoryChangeRepository.Table.OrderBy(sortBy, sortOrder);
            if (options.BeginDate.HasValue)
            {
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            else
            {
                options.BeginDate = DateTime.Now;
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            if (options.EndDate.HasValue)
            {
                query = query.Where(x => x.Date <= options.EndDate);
            }
            if (options.MaterialId.HasValue)
            {
                query = query.Where(x => x.MaterialId == options.MaterialId);
            }
            //入库总数和金额
            var inventoryChangeRecords = query as IList<InventoryChangeRecord> ?? query.ToList();
            var totalRecords = inventoryChangeRecords.Count();
            var records = inventoryChangeRecords
                .GroupBy(x => x.MaterialId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(item => new StockSummaryListViewModel
                {
                    Id = item.Key,
                    MaterialSerialNo = materials[item.Key].SerialNo,
                    MaterialName = materials[item.Key].Name,
                    MaterialUnit = materials[item.Key].Unit,
                    BeginningNumber = _stockReportService.GetBeginingAmountAndNumber(item.Key, options.BeginDate).Number,
                    BeginningAmount = _stockReportService.GetBeginingAmountAndNumber(item.Key, options.BeginDate).Amount,
                    StockInNumber = _stockReportService.GetStockAmountAndNumber(item.Key, options.BeginDate, options.EndDate,InventoryChangeOperation.入库, false).Number,
                    StockInAmount = _stockReportService.GetStockAmountAndNumber(item.Key, options.BeginDate, options.EndDate,InventoryChangeOperation.入库, false).Amount,
                    StockOutNumber = _stockReportService.GetStockAmountAndNumber(item.Key, options.BeginDate, options.EndDate, InventoryChangeOperation.出库, false).Number,
                    StockOutAmount = _stockReportService.GetStockAmountAndNumber(item.Key, options.BeginDate, options.EndDate, InventoryChangeOperation.出库, false).Amount
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

        public ActionResult StockInDetailExportExcel(FormCollection input, string gridColumns)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("采购入库明细");

            #region main grid

            var qVoucher = Services.ContentManager.Query<VoucherPart, VoucherPartRecord>().List().ToDictionary(x => x.VoucherNo);
            var qSupplier = Services.ContentManager.Query<SupplierPart, SupplierPartRecord>().List().ToDictionary(x => x.Id);
            var options = new StockFilterOptions();
            UpdateModel(options, "Filter", input);
            Dictionary<int, MaterialPart> materials =
            Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            var query = _inventoryChangeRepository.Table;
            if (options.BeginDate.HasValue)
            {
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            else
            {
                options.BeginDate = DateTime.Now;
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            if (options.EndDate.HasValue)
            {
                query = query.Where(x => x.Date <= options.EndDate);
            }
            if (options.MaterialId.HasValue)
            {
                query = query.Where(x => x.MaterialId == options.MaterialId);
            }
            var inventoryChangeRecords = query as IList<InventoryChangeRecord> ?? query.ToList();
            var records = inventoryChangeRecords
                .Where(x => x.Operation == InventoryChangeOperation.入库)
                .Select(item => new StockDetailListViewModel
                {
                    Id = item.Id,
                    VoucherNo = item.VoucherNo,
                    MaterialSerialNo = materials[item.MaterialId].SerialNo,
                    MaterialName = materials[item.MaterialId].Name,
                    MaterialUnit = materials[item.MaterialId].Unit,
                    CostPrice = item.CostPrice,
                    Number = item.Number,
                    SupplierName = qSupplier[qVoucher[item.VoucherNo].SupplierId].Name,
                    Date = item.Date.ToString("MM/dd/yyyy")
                }).ToList();

            #endregion
            _reportServices.FillExcel(gridColumns, records, sheet);
            //Save the Excel spreadsheet to a MemoryStream and return it to the client
            using (var exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                string saveAsFileName = string.Format("{0}-{1:d}.xls", "采购入库明细", DateTime.Now).Replace("/", "-");
                return File(exportData.ToArray(), "application/vnd.ms-excel", saveAsFileName);
            }
        }


        public ActionResult StockOutDetailExportExcel(FormCollection input, string gridColumns)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("采购出库明细");

            #region main grid

            var qVoucher = Services.ContentManager.Query<VoucherPart, VoucherPartRecord>().List().ToDictionary(x => x.VoucherNo);
            var qDepartment = Services.ContentManager.Query<DepartmentPart, DepartmentPartRecord>().List().ToDictionary(x => x.Id);
            var options = new StockFilterOptions();
            UpdateModel(options, "Filter", input);
            Dictionary<int, MaterialPart> materials =
             Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            var query = _inventoryChangeRepository.Table;
            if (options.BeginDate.HasValue)
            {
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            else
            {
                options.BeginDate = DateTime.Now;
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            if (options.EndDate.HasValue)
            {
                query = query.Where(x => x.Date <= options.EndDate);
            }
            if (options.MaterialId.HasValue)
            {
                query = query.Where(x => x.MaterialId == options.MaterialId);
            }
            var inventoryChangeRecords = query as IList<InventoryChangeRecord> ?? query.ToList();
            var records = inventoryChangeRecords
                .Where(x => x.Operation == InventoryChangeOperation.出库)
                .Select(item => new StockDetailListViewModel
                {
                    Id = item.Id,
                    VoucherNo = item.VoucherNo,
                    MaterialSerialNo = materials[item.MaterialId].SerialNo,
                    MaterialName = materials[item.MaterialId].Name,
                    MaterialUnit = materials[item.MaterialId].Unit,
                    CostPrice = item.CostPrice,
                    Number = item.Number,
                    DepartmentName = qDepartment[qVoucher[item.VoucherNo].DepartmentId].Name,
                    Date = item.Date.ToString("MM/dd/yyyy")
                }).ToList();

            #endregion
            _reportServices.FillExcel(gridColumns, records, sheet);
            //Save the Excel spreadsheet to a MemoryStream and return it to the client
            using (var exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                string saveAsFileName = string.Format("{0}-{1:d}.xls", "采购出库明细", DateTime.Now).Replace("/", "-");
                return File(exportData.ToArray(), "application/vnd.ms-excel", saveAsFileName);
            }
        }

        public ActionResult StockSumarryExportExcel(FormCollection input, string gridColumns)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("库存汇总");

            #region main grid

            var options = new StockFilterOptions();
            UpdateModel(options, "Filter", input);
            Dictionary<int, MaterialPart> materials =
             Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            var query = _inventoryChangeRepository.Table;
            if (options.BeginDate.HasValue)
            {
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            else
            {
                options.BeginDate = DateTime.Now;
                query = query.Where(x => x.Date >= options.BeginDate);
            }
            if (options.EndDate.HasValue)
            {
                query = query.Where(x => x.Date <= options.EndDate);
            }
            if (options.MaterialId.HasValue)
            {
                query = query.Where(x => x.MaterialId == options.MaterialId);
            }
            //入库总数和金额
            var inventoryChangeRecords = query as IList<InventoryChangeRecord> ?? query.ToList();
           
            var records = inventoryChangeRecords
                .GroupBy(x => x.MaterialId)
                .Select(item => new StockSummaryListViewModel
                {
                    Id = item.Key,
                    MaterialSerialNo = materials[item.Key].SerialNo,
                    MaterialName = materials[item.Key].Name,
                    MaterialUnit = materials[item.Key].Unit,
                    BeginningNumber = _stockReportService.GetBeginingAmountAndNumber(item.Key, options.BeginDate).Number,
                    BeginningAmount = _stockReportService.GetBeginingAmountAndNumber(item.Key, options.BeginDate).Amount,
                    StockInNumber = _stockReportService.GetStockAmountAndNumber(item.Key, options.BeginDate, options.EndDate, InventoryChangeOperation.入库, false).Number,
                    StockInAmount = _stockReportService.GetStockAmountAndNumber(item.Key, options.BeginDate, options.EndDate, InventoryChangeOperation.入库, false).Amount,
                    StockOutNumber = _stockReportService.GetStockAmountAndNumber(item.Key, options.BeginDate, options.EndDate, InventoryChangeOperation.出库, false).Number,
                    StockOutAmount = _stockReportService.GetStockAmountAndNumber(item.Key, options.BeginDate, options.EndDate, InventoryChangeOperation.出库, false).Amount
                }).ToList();

            #endregion
            _reportServices.FillExcel(gridColumns, records, sheet);
            //Save the Excel spreadsheet to a MemoryStream and return it to the client
            using (var exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                string saveAsFileName = string.Format("{0}-{1:d}.xls", "库存汇总", DateTime.Now).Replace("/", "-");
                return File(exportData.ToArray(), "application/vnd.ms-excel", saveAsFileName);
            }
        }

        [HttpPost]
        public ActionResult MaterialDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc")
        {
            IContentQuery<MaterialPart, MaterialPartRecord> query =
                Services.ContentManager.Query<MaterialPart, MaterialPartRecord>();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.Name.Contains(term));
            }
            int totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1) * pageSize, pageSize)
                .Select(item => new
                {
                    id = item.Record.ContentItemRecord.Id,
                    text = item.Name
                }).ToList();
            return Json(new { records, total = totalRecords });
        }
        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties,
            string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}