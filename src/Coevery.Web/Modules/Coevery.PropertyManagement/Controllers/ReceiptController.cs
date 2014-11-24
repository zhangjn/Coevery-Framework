using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Coevery.PropertyManagement.Security;
using Coevery.PropertyManagement.Services;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;
using Coevery.Security;
using Coevery.Themes;
using NPOI.HSSF.UserModel;
using Coevery.PropertyManagement.Extensions;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class ReceiptController : Controller, IUpdateModel
    {
        private readonly IRepository<PaymentPartRecord> _paymentRecordRepository;
        private readonly IRepository<ApartmentPartRecord> _apartmentRepository;
        
        private readonly IReportServices _reportServices;
        private readonly ITransactionManager _transactionManager;

        public ReceiptController(ICoeveryServices services,
            ITransactionManager transactionManager,
            IReportServices reportServices, 
            IRepository<PaymentPartRecord> paymentRecordRepository, 
            IRepository<ApartmentPartRecord> apartmentRepository)
        {
            Services = services;
            _transactionManager = transactionManager;

            _reportServices = reportServices;
            _paymentRecordRepository = paymentRecordRepository;
            _apartmentRepository = apartmentRepository;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }

        public ActionResult Index()
        {
            return List();
        }

        public ActionResult List()
        {
            if (!Services.Authorizer.Authorize(Permissions.BillManage, T("没有账单查看权限")))
                return new HttpUnauthorizedResult();
            //BillListViewModel model=new BillListViewModel();
            List<SelectListItem> apartments = _apartmentRepository.Table
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            apartments.Insert(0, new SelectListItem());
            ViewBag.Apartments = apartments;

            List<SelectListItem> customers = Services.ContentManager.Query<CustomerPart, CustomerPartRecord>().List()
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
            customers.Insert(0, new SelectListItem());
            ViewBag.Customers = customers;

            return View("List");
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc") {
            if (!Services.Authorizer.Authorize(Permissions.BillManage, T("没有账单查看权限")))
                return new HttpUnauthorizedResult();

            var list = QueryReceiptRecords(input);
            decimal? totalReceivedMoney = list.Sum(l => l.ChargeItemAmount);

            int totalRecords = list.Count;
            list = list.Skip((page - 1)*pageSize)
                       .Take(pageSize).ToList();
            var result = new {
                page,
                totalPages = Math.Ceiling((double) totalRecords/pageSize),
                totalRecords,
                rows = list,
                totalAmount = totalReceivedMoney.Value.ToString("c")
            };
            return Json(result);
        }

        private List<ReceiptListViewModel> QueryReceiptRecords(FormCollection input) {
            var query = _paymentRecordRepository.Table.Where(x => x.ContentItemRecord.Versions.Any(i => i.Published));

            //这里加过滤器的代码
            var options = new BillFilterOptions();
            UpdateModel(options, "Filter", input);

            if (options.BeginDate.HasValue) {
                query = query.Where(x => x.PaidOn >= options.BeginDate);
            }
            if (options.EndDate.HasValue) {
                query = query.Where(x => x.PaidOn <= options.EndDate);
            }

            var bills = query.SelectMany(x => x.LineItems).Select(x => x.Bill).ToList();

            return _reportServices.GenerateReceiptListViewModels(bills).ToList();
        }

        [HttpPost]
        public ActionResult ReceivedMoneySummary(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc") {
            if (!Services.Authorizer.Authorize(Permissions.BillManage, T("没有账单查看权限")))
                return new HttpUnauthorizedResult();

            var list = QueryReceivedSummaryRecord(input);

            int totalRecords = list.Count;
            list = list.Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
            var result = new
            {
                page,
                totalPages = Math.Ceiling((double)totalRecords / pageSize),
                totalRecords,
                rows = list
            };
            return Json(result);
        }

        private List<BillReportSummaryViewModel> QueryReceivedSummaryRecord(FormCollection input) {
            var query = _paymentRecordRepository.Table.Where(x => x.ContentItemRecord.Versions.Any(i => i.Published));

            //这里加过滤器的代码
            var options = new BillFilterOptions();
            UpdateModel(options, "Filter", input);

            if (options.BeginDate.HasValue)
            {
                query = query.Where(x => x.PaidOn >= options.BeginDate);
            }
            if (options.EndDate.HasValue)
            {
                query = query.Where(x => x.PaidOn <= options.EndDate);
            }

            return _reportServices.GenerateReceiptReportSummaryViewModel(query.ToList()).ToList();
        }

        public ActionResult ExportExcel(FormCollection input, string gridColumns, string gridSummaryColumns)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("收款明细表");

            #region main grid

            var mainList = QueryReceiptRecords(input);
            _reportServices.FillExcel(gridColumns, mainList, sheet);

            #endregion

            var row = sheet.CreateRow(sheet.LastRowNum + 1);
            row.CreateCell(0).SetCellValue("合计");
            row.CreateCell(1).SetCellValue(mainList.Sum(x => x.ChargeItemAmount));
            sheet.CreateRow(sheet.LastRowNum + 1);

            #region Summary Grid

            var summaryList = QueryReceivedSummaryRecord(input);
            _reportServices.FillExcel(gridSummaryColumns, summaryList, sheet);

            #endregion

            //Save the Excel spreadsheet to a MemoryStream and return it to the client
            using (var exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                string saveAsFileName = string.Format("{0}-{1:d}.xls", "收款明细表", DateTime.Now).Replace("/", "-");
                return File(exportData.ToArray(), "application/vnd.ms-excel", saveAsFileName);
            }
        }

    }
}