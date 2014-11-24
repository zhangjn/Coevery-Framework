using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.Services;
using Coevery.PropertyManagement.ViewModels;
using Coevery.Themes;
using Coevery.UI.Notify;
using Coevery.Users.Models;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class AdvancePaymentController : Controller, IUpdateModel
    {
        private readonly IVoucherNumberService _voucherNumberService;
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<CustomerPartRecord> _customerRepository;

        public AdvancePaymentController(ICoeveryServices services,
            ITransactionManager transactionManager,
            IRepository<CustomerPartRecord> customerRepository,
            IVoucherNumberService voucherNumberService)
        {
            Services = services;
            _transactionManager = transactionManager;
            _customerRepository = customerRepository;
            _voucherNumberService = voucherNumberService;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult CustomerAdvancePayment()
        {
            return View();
        }

        [HttpPost, ActionName("CustomerAdvancePayment")]
        public ActionResult CustomerAdvancePaymentPost(AdvancePaymentCreateViewModel model)
        {
            var contentItem = Services.ContentManager.New("AdvancePayment");
            var record = contentItem.As<AdvancePaymentPart>().Record;
            var Operator = (UserPart) Services.WorkContext.CurrentUser;
            record.Operator = Operator.Record;
            record.Customer = _customerRepository.Get(model.CustomerId); //缴费客户        
            record.PaidOn = DateTime.Now;
            record.Paid = model.Paid;
            model.LineItems.Select(x => new AdvancePaymentItemRecord
            {
                Amount = x.Amount,
                AdvancePaymentMethod = (AdvancePaymentMethodOption) x.AdvancePaymentMethodId,
                Description = x.Description,
                Payment = record
            }).ToList().ForEach(record.AdvancePaymentItems.Add);
            Services.ContentManager.Create(contentItem, VersionOptions.Draft);
            Services.ContentManager.Publish(contentItem);
            var id = contentItem.Id;

            #region update custome balance

            var customerRecord = _customerRepository.Get(model.CustomerId);
            customerRecord.CustomerBalance =customerRecord.CustomerBalance+ model.Paid;
            _customerRepository.Update(customerRecord);

            #endregion

            return Json(Url.Action("CustomerAdvancePaymentPreview", new {id = id}));
        }

        public ActionResult CustomerAdvancePaymentPreview(int id)
        {
            var record = Services.ContentManager.Get<AdvancePaymentPart>(id).Record;
            var advancePaymentVoucherModel = new AdvancePaymentVoucherViewModel();
            advancePaymentVoucherModel.VoucherNo = "";
            advancePaymentVoucherModel.CustomerName = record.Customer.Name;
            advancePaymentVoucherModel.List = new List<AdvancePaymentDetailViewModel>();
            for (int i = 0; i < record.AdvancePaymentItems.Count; i++)
            {
                var detailModel = new AdvancePaymentDetailViewModel
                {
                    Id = i + 1,
                    AdvancePaymentMethod = record.AdvancePaymentItems[i].AdvancePaymentMethod.ToString(),
                    Amount = record.AdvancePaymentItems[i].Amount,
                    Description = record.AdvancePaymentItems[i].Description
                };
                advancePaymentVoucherModel.List.Add(detailModel);
            }
            return View(advancePaymentVoucherModel);
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
            var query = Services.ContentManager.Query<AdvancePaymentPart, AdvancePaymentPartRecord>();

            #region Filter

            /*    var options = new InventoryFilterOptions();
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
        
            */

            #endregion
            var totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1)*pageSize, pageSize)
                .Select(item => new AdvancePaymentListViewModel
                {
                    Id = item.Record.Id,
                    Paid = item.Record.Paid,
                    PaidDate = item.Record.PaidOn.ToString("d"),
                    OperatorName = item.Record.Operator.UserName,
                    CustomerName = item.Record.Customer.Name
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
        public ActionResult CustomerDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc")
        {
            IContentQuery<CustomerPart, CustomerPartRecord> query =
                Services.ContentManager.Query<CustomerPart, CustomerPartRecord>();
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

        [HttpPost]
        public ActionResult GetCustomerBalance(int customerId)
        {
            var customerRecord = _customerRepository.Get(customerId);
            return Json(customerRecord.CustomerBalance);
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