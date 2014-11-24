using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.Services;
using Coevery.PropertyManagement.ViewModels;
using Coevery.Themes;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class BillAllocationReportController : Controller, IUpdateModel
    {
        private readonly IReportServices _reportServices;
        private readonly IRepository<BillRecord> _billRecordRepository;
        private readonly IRepository<PaymentLineItemPartRecord> _paymentLineItemRepository;

        public BillAllocationReportController(ICoeveryServices services,
            IRepository<BillRecord> billRecordRepository,
            IReportServices reportServices,
            IRepository<PaymentLineItemPartRecord> paymentLineItemRepository)
        {
            Services = services;
            _billRecordRepository = billRecordRepository;
            _reportServices = reportServices;
            _paymentLineItemRepository = paymentLineItemRepository;
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
            return View("List");
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
            var query = QueryBillRecord(input).ToList();
            var list = _reportServices.GenerateBillAllocationReportListViewModel(query).ToList();

            int totalRecords = list.Count;
            list = list.Skip((page - 1)*pageSize).Take(pageSize).ToList();
            var result = new
            {
                page,
                totalPages = Math.Ceiling((double) totalRecords/pageSize),
                totalRecords,
                rows = list
            };
            return Json(result);
        }

        private IEnumerable<BillRecord> QueryBillRecord(FormCollection input)
        {
            IQueryable<BillRecord> query = null;
            var options = new BillFilterOptions();
            UpdateModel(options, "Filter", input);
            if (options.BeginDate.HasValue)
            {
                var month = options.BeginDate.Value.Month;
                query = (from b in _billRecordRepository.Table
                    join p in _paymentLineItemRepository.Table on b.Id equals p.Bill.Id
                    where b.Status == BillRecord.BillStatusOption.已结账单
                          && p.Payment.PaidOn.Value.Month == month
                    select b);
            }


            bool hasValue = false;

            #region 合同号，费用承担人查询

            if (!string.IsNullOrEmpty(options.ContractNumber))
            {
                hasValue = true;
                query = query.Where(b => b.Contract.Id.ToString().Contains(options.ContractNumber));
            }
            if (options.ExpenserId.HasValue)
            {
                hasValue = true;
                query = query.Where(b => b.CustomerId == options.ExpenserId);
            }
            if (options.ChargeItemId.HasValue)
            {
                hasValue = true;
                query = query.Where(b => b.ChargeItem.Id == options.ChargeItemId);
            }

            #endregion

            if (hasValue)
            {
                return query.ToList();
            }
            return new List<BillRecord>();
        }
    }
}