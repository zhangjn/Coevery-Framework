using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Coevery.PropertyManagement.Security;
using Coevery.PropertyManagement.Services;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;
using Coevery.Security;
using Coevery.Themes;
using Coevery.UI.Notify;
using Coevery.Users.Models;
using Coevery.PropertyManagement.Extensions;
using NPOI.HSSF.UserModel;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class StatementReportController : Controller, IUpdateModel
    {
        private readonly IRepository<BillRecord> _billRecordRepository; 

        private readonly IReportServices _reportServices;

        public StatementReportController(ICoeveryServices services,
            IRepository<BillRecord> billRecordRepository,
            IReportServices reportServices)
        {
            Services = services;
            _billRecordRepository = billRecordRepository;
            _reportServices = reportServices;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties,
            string[] excludeProperties)
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

            return View("List");
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc") {
            if (!Services.Authorizer.Authorize(Permissions.BillManage, T("没有账单查看权限")))
                return new HttpUnauthorizedResult();

            var query = QueryBillRecord(input).ToList();
            var list = _reportServices.GenerateStatementReportListViewModel(query).ToList();

            int totalRecords = list.Count;
            list = list.Skip((page - 1)*pageSize).Take(pageSize).ToList();
            var result = new {
                page,
                totalPages = Math.Ceiling((double) totalRecords/pageSize),
                totalRecords,
                rows = list
            };
            return Json(result);
        }

        [HttpPost]
        public ActionResult ChargeItemInContract(int contractId, int expenserId) {
            var chargeItems = _billRecordRepository.Table
                                                   .Where(b => b.Contract.Id == contractId && b.CustomerId == expenserId)
                                                   .Select(b => b.ChargeItem)
                                                   .Distinct();
            
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (ChargeItemSettingCommonRecord chargeItem in chargeItems) {
                items.Add(new SelectListItem() {
                    Text = chargeItem.Name, Value = chargeItem.Id.ToString()
                });
            }
            items.Insert(0, new SelectListItem(){Text = "", Value = ""});
            return Json(items);
        }

        public ActionResult ExportExcel(FormCollection input, string gridColumns, string gridSummaryColumns) {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("收费对账单");

            #region main grid

            var query = QueryBillRecord(input).ToList();
            var mainList = _reportServices.GenerateStatementReportListViewModel(query).ToList();

            _reportServices.FillExcel(gridColumns, mainList, sheet);

            #endregion

            //Save the Excel spreadsheet to a MemoryStream and return it to the client
            using (var exportData = new MemoryStream()) {
                workbook.Write(exportData);
                string saveAsFileName = string.Format("{0}-{1:d}.xls", "收费对账单", DateTime.Now).Replace("/", "-");
                return File(exportData.ToArray(), "application/vnd.ms-excel", saveAsFileName);
            }
        }

        private IEnumerable<BillRecord> QueryBillRecord(FormCollection input) {
            IQueryable<BillRecord> query = _billRecordRepository.Table;

            //这里加过滤器的代码
            var options = new BillFilterOptions();
            UpdateModel(options, "Filter", input);

            bool hasValue = false;

            #region 合同号，费用承担人查询
            
            if (!string.IsNullOrEmpty(options.ContractNumber)) {
                hasValue = true;
                query = query.Where(b => b.Contract.Id.ToString().Contains(options.ContractNumber));
            }
            if (options.ExpenserId.HasValue) {
                hasValue = true;
                query = query.Where(b => b.CustomerId == options.ExpenserId);
            }
            if (options.ChargeItemId.HasValue)
            {
                hasValue = true;
                query = query.Where(b => b.ChargeItem.Id == options.ChargeItemId);
            }

            if (options.BeginDate.HasValue)
            {
                query = query.Where(x => x.StartDate >= options.BeginDate);
            }
            if (options.EndDate.HasValue)
            {
                query = query.Where(x => x.EndDate <= options.EndDate);
            }

            #endregion 

            if (hasValue) {
                return query.ToList();
            }
            return new List<BillRecord>();
        }
    }
}