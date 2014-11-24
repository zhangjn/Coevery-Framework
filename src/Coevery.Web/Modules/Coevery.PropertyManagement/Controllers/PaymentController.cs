using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.Security;
using Coevery.PropertyManagement.ViewModels;
using Coevery.Security;
using Coevery.Themes;
using Coevery.UI.Notify;
using Coevery.Users.Models;
using FluentNHibernate.Utils;
using NPOI.SS.Formula.Functions;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class PaymentController : Controller, IUpdateModel
    {
        private readonly IRepository<BillRecord> _billRepository;
        private readonly IRepository<ContractHouseRecord> _contractHouseRepository;
        private readonly IRepository<ContractChargeItemRecord> _contractChargeItemRepository;
        private readonly IRepository<PaymentLineItemPartRecord> _paymentLineItemRepository;
        private readonly IRepository<PaymentPartRecord> _paymentRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<PaymentMethodItemRecord> _paymentMethodItemRecordRepository;
        private readonly IRepository<CustomerPartRecord> _customerRepository;
        private readonly IRepository<ApartmentPartRecord> _apartmentRepository;

        public PaymentController(ICoeveryServices services,
            ITransactionManager transactionManager,
            IRepository<BillRecord> billRepository,
            IRepository<PaymentLineItemPartRecord> paymentLineItemRepository,
            IRepository<PaymentPartRecord> paymentRepository,
            IRepository<ContractHouseRecord> contractHouseRepository,
            IRepository<ContractChargeItemRecord> contractChargeItemRepository,
            IRepository<PaymentMethodItemRecord> paymentMethodItemRecordRepository, 
            IRepository<CustomerPartRecord> customerRepository, IRepository<ApartmentPartRecord> apartmentRepository)
        {
            Services = services;
            _transactionManager = transactionManager;
            _billRepository = billRepository;
            _contractHouseRepository = contractHouseRepository;
            _contractChargeItemRepository = contractChargeItemRepository;
            _paymentLineItemRepository = paymentLineItemRepository;
            _paymentRepository = paymentRepository;
            _paymentMethodItemRecordRepository = paymentMethodItemRecordRepository;
            _customerRepository = customerRepository;
            _apartmentRepository = apartmentRepository;
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
            if (!Services.Authorizer.Authorize(Permissions.BillManage, T("没有缴费权限！")))
                return new HttpUnauthorizedResult();
            var billModel = new PaymentListViewModel();
            List<SelectListItem> apartments = _apartmentRepository.Table.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            apartments.Insert(0, new SelectListItem());

            List<SelectListItem> customers = Services.ContentManager.Query<CustomerPart, CustomerPartRecord>()
                .List().Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();
            customers.Insert(0, new SelectListItem());
            billModel.ApartmentsList = apartments;
            billModel.CustomersList = customers;
            //临时性收费项目
            List<SelectListItem> incidentalChargeItems = Services.ContentManager.Query<ChargeItemSettingPart, ChargeItemSettingPartRecord>().List()
                                               .Where(c => c.ItemCategory == ItemCategoryOption.临时性收费项目)
                                               .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                                               .ToList();
            incidentalChargeItems.Insert(0,new SelectListItem());
            billModel.IncidentalChargeItemList = incidentalChargeItems;

            //选合同，确定是所有合同
            List<SelectListItem> incidentalContracts =
                Services.ContentManager.Query<ContractPart, ContractPartRecord>().List()
                    .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();
            incidentalContracts.Insert(0, new SelectListItem());
            billModel.IncidentalContractList = incidentalContracts;

            return View("List", billModel);
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
        if (!Services.Authorizer.Authorize(Permissions.BillManage, T("没有缴费权限！")))
                return new HttpUnauthorizedResult();
            var query = _billRepository.Table;
            var options = new PaymentFilterOptions();
            UpdateModel(options, "Filter", input);
            bool isHasValue = false;
            if (options.ApartmentId.HasValue)
            {
                query = query.Where(c => c.House.Apartment.Id == options.ApartmentId);
                isHasValue = true;
            }
            //if (options.BuildingId.HasValue)
            //{
            //    query = query.Where(c => c.House.Building.Id == options.BuildingId);
            //    isHasValue = true;
            //}
            //if (!string.IsNullOrEmpty(options.HouseNumber))
            //{
            //    query = query.Where(c => c.House.HouseNumber == options.HouseNumber); 
            //    isHasValue = true;
            //}
            //if (!string.IsNullOrEmpty(options.ContractNumber))
            //{
            //    query = query.Where(c => c.Contract.Number.Contains(options.ContractNumber.Trim()));
            //    isHasValue = true;
            //}

            var expenserId = options.ExpenserId ?? options.ExpenserIdByContract;
            var contractId = options.ContractId ?? options.ContractIdByContract;

            if (expenserId.HasValue)
            {
                query = query.Where(c => c.CustomerId == expenserId); 
                isHasValue = true;
            }
            if (contractId.HasValue)
            {
                query = query.Where(c => c.Contract.Id == contractId);
                isHasValue = true;
            }

            query = query.Where(c => c.Status == BillRecord.BillStatusOption.已出账单);
            var list = new List<PaymentListViewModel>();

            if (isHasValue)
            {
                list = GetPaymentListViewModels(query);
            }

            int totalRecords = list.Count;
            list = list.Skip((page - 1)*pageSize)
                .Take(pageSize).ToList();
            var result = new
            {
                page,
                totalPages = Math.Ceiling((double) totalRecords/pageSize),
                totalRecords,
                rows = list
            };
            return Json(result);
        }

        private List<PaymentListViewModel> GetPaymentListViewModels(IQueryable<BillRecord> query)
        {
            return query
                .Where(x => x.Status == BillRecord.BillStatusOption.已出账单)
                .Select(item => new PaymentListViewModel()
                {
                    Id = item.Id,
                    CustomerId = item.CustomerId,
                    ContractId = item.Contract.Id,
                    ContractNumber = item.Contract.Number,
                    HouseId = item.House.Id,
                    HouseNumber = item.House.HouseNumber,
                    ChargeItemSettingId = item.ChargeItem.Id,
                    ChargeItemSettingDescription = item.ChargeItem.Name,
                    BeginDate = item.StartDate.ToString("yyyy/MM/dd"),
                    EndDate = item.EndDate.ToString("yyyy/MM/dd"),
                    Year = item.Year,
                    Month = item.Month,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    Exempt = item.Exempt,
                    DelayCharge = CalculateDelayChargeAmount(item),
                    Status = item.Status.ToString(),
                    Notes = item.Notes,
                    ApartmentName = item.House.Apartment.Name,
                    BuildingName = item.House.Building.Name
                }).ToList();
        }

        //private string GetChargeItemSettingDescription(ChargeItemSettingCommonRecord record) {
        //    return string.Format("{0}-{1:f2}-{2:f2}", record.Name, record., record.UnitPrice);
        //}

        [HttpPost]
        public ActionResult Edit(PaymentListViewModel model) {
            var records = _billRepository.Table.Where(c => c.Id == model.Id);
            var record = records.Single();
            record.Amount = model.Amount;
            record.Notes = model.Notes;
            record.Exempt = model.Exempt;
            record.DelayCharge = model.DelayCharge;
            Services.Notifier.Information(T("修改费用成功！"));
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult AddTemporary(PaymentListViewModel model) {
            var billRecord = new BillRecord();
            billRecord.Contract = Services.ContentManager.Get<ContractPart>(model.ContractId).Record;
            billRecord.House = Services.ContentManager.Get<HousePart>(model.HouseId).Record;
            billRecord.CustomerId = model.CustomerId;//费用曾担人
            billRecord.Amount = model.Amount;

            // todo: 临时费用
            //billRecord.ChargeItemSettingId = model.ChargeItemSettingId;
            //billRecord.ChargeItemSettingDescription =
            //    Services.ContentManager.Get<ChargeItemSettingPart>(model.ChargeItemSettingId).Name;
                
            billRecord.StartDate = Convert.ToDateTime(model.BeginDate);
            billRecord.EndDate = Convert.ToDateTime(model.EndDate);//结束日期很远还需要显示账单吗？

            billRecord.Status = BillRecord.BillStatusOption.已出账单;//保存到bill表，显示为已出账单
            _billRepository.Create(billRecord);
            Services.Notifier.Information(T("添加临时费用成功！"));
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult GetHousesByContractId(int contractId)
        {
            var houses =_contractHouseRepository.Table.Where(c=>c.Contract.Id==contractId)
                        .Select(x => new { Value = x.House.Id.ToString(),Text = x.House.HouseNumber })
                        .ToList();
            if (houses.Count==0)
            {
                houses.Insert(0, new {Value = "", Text = ""});
            }
            return Json(houses);
        }

        [HttpPost]
        public ActionResult GetExpenserByHouseId(int houseId, int contractId)
        {
            var contractPart = Services.ContentManager.Get<ContractPart>(contractId);
            var housePart = Services.ContentManager.Get<HousePart>(houseId);
            //找费用承担人,可能是合同中的的客户，也肯是房间的业主
            var expenserList = new List<SelectListItem>();

            var itemOwner = new SelectListItem();
            itemOwner.Value = housePart.Owner.Id.ToString();
            itemOwner.Text = housePart.Owner.Name;
            expenserList.Add(itemOwner);
            var itemRenter = new SelectListItem();
            itemRenter.Value = contractPart.Renter.Id.ToString();
            itemRenter.Text = contractPart.Renter.Name;
            expenserList.Add(itemRenter);

            expenserList = expenserList.Distinct().ToList();
            if (expenserList.Count == 0)
            {
                expenserList.Insert(0, new SelectListItem());
            }
            return Json(expenserList);
        }


        [HttpPost]
        public ActionResult GetExpenserListByContractId(int contractId) {
            var contractPart = Services.ContentManager.Get<ContractPart>(contractId);//先找到合同
            var expenserList = new List<SelectListItem>();
            var itemRenter = new SelectListItem();
            itemRenter.Value = contractPart.Renter.Id.ToString();
            itemRenter.Text = contractPart.Renter.Name;
            expenserList.Add(itemRenter);
            List<SelectListItem> houseOwnerList = _contractHouseRepository.Table.Where(c => c.Contract.Id == contractId)
                                                  .Select(x => new SelectListItem{ Value = x.House.Owner.Id.ToString(), Text = x.House.Owner.Name })
                                                  .ToList();
            var chargeItemList = _contractChargeItemRepository.Table
                                 .Where(c => c.Contract.Id == contractId && c.ExpenserOption == (int)ExpenserOption.业主)
                                 .ToList();
            if (chargeItemList.Count>0)//有业主的收费项目要加上所有房间的业主
            {
                expenserList=expenserList.Union(houseOwnerList).Distinct().ToList();
            }
            expenserList.Insert(0, new SelectListItem { Value = "", Text = "" });
            return Json(expenserList);
        }

        [HttpPost]
        public ActionResult GetContractListByExpenserId(int expenserId) {
            var expenserPart = Services.ContentManager.Get<CustomerPart>(expenserId);//先找到费用承担人

            List<SelectListItem> contractList = Services.ContentManager.Query<ContractPart, ContractPartRecord>()
                              .Where(c => c.Renter.Id == expenserId)
                              .List()
                              .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                              .ToList();

            List<SelectListItem> contractHouseList = _contractHouseRepository.Table.Where(c => c.House.Owner.Id == expenserId)
                                                     .Select(x => new SelectListItem { Value = x.Contract.Id.ToString(), Text = x.Contract.Name })
                                                     .ToList();
            contractList = contractList.Union(contractHouseList).Distinct().ToList();

            contractList.Insert(0,new SelectListItem{Value = "",Text = ""});
            return Json(contractList);
        }

        [HttpPost]
        public ActionResult ContractDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc") {
                var query = Services.ContentManager.Query<ContractPart, ContractPartRecord>();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.Name.Contains(term));
            }
            var totalRecords = query.Count();
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

        public ActionResult CheckOut(int expenserId, int contractId, List<int> billIds)
        {
            var model = GetCheckOutViewModel(expenserId, contractId, billIds);
            return View("CheckOut", model);
        }

        private CheckOutViewModel GetCheckOutViewModel(int expenserId, int contractId, List<int> billIds)
        {
            var query = _billRepository.Table.Where(x => x.CustomerId == expenserId && x.Contract.Id == contractId);
            if (billIds != null && billIds.Any())
            {
                query = _billRepository.Table.Where(x => billIds.Contains(x.Id));
            }
            var models = GetPaymentListViewModels(query);
            var model = new CheckOutViewModel
            {
                List = models,
                ChargeDate = DateTime.Now.ToString("yyyy/MM/dd"),
                ChargeUser = Services.WorkContext.CurrentUser.UserName
            };
            if (models.Count > 0)
            {
                model.Expenser = Services.ContentManager.Get<CustomerPart>(models.First().CustomerId).Record; //传递参数用
                model.Contract = Services.ContentManager.Get<ContractPart>(models.First().ContractId).Record; //传递参数用
            }
            return model;
        }

        //PaymentMethod
        public ActionResult PaymentMethod(int expenserId, int contractId, List<int> billIds) 
        {
            var model = GetCheckOutViewModel(expenserId, contractId, billIds);
            return View(model);
        }

        [HttpPost, ActionName("PaymentMethod")]
        public ActionResult PaymentMethodPost(int expenserId, int contractId, List<int> billIds, List<PaymentMethodViewModel> paymentMethodList) {
            try
            {
                //判断是否缴清
                var sumMoney = Math.Round(paymentMethodList.Sum(x => x.Amount),2);
                var checkOutModel = GetCheckOutViewModel(expenserId, contractId, billIds);
                if (sumMoney < Math.Round(checkOutModel.TotalMoney,2))
                {
                    Services.Notifier.Information(T("数据错误！"));
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var contentItem = Services.ContentManager.New("Payment");
                var record = contentItem.As<PaymentPart>().Record;
                var Operator = (UserPart)Services.WorkContext.CurrentUser;
                record.Operator = Operator.Record;
                record.CustomerId = checkOutModel.Expenser; //费用承担者
                record.PaidOn = DateTime.Now;
                record.Paid = sumMoney;//会保存多余的费用
                //保存paymentLineItem表
                checkOutModel.List.ForEach(model =>
                {
                    var billRecord = _billRepository.Get(model.Id);
                    billRecord.Status = BillRecord.BillStatusOption.已结账单;
                    _billRepository.Update(billRecord);
                    var item = new PaymentLineItemPartRecord();
                    item.Bill = _billRepository.Get(model.Id);
                    item.Payment = record;
                    record.LineItems.Add(item);
                });
                Services.ContentManager.Create(contentItem, VersionOptions.Draft);
                Services.ContentManager.Publish(contentItem);
                //保存paymentMethod表
                paymentMethodList.ForEach(x =>
                {
                    var item = new PaymentMethodItemRecord();
                    item.Payment = record;
                    item.PaymentMethod = (PaymentMethodOption)x.PaymentMethodId;
                    if (x.PaymentMethodId == 5)
                    {
                        var customerRecord = _customerRepository.Get(expenserId);
                        customerRecord.CustomerBalance = customerRecord.CustomerBalance -x.Amount;
                        _customerRepository.Update(customerRecord);
                    }
                    item.Amount = x.Amount;
                    item.Description = x.Description;
                    _paymentMethodItemRecordRepository.Create(item);
                });
            }
            catch (Exception ex)
            {
                _transactionManager.Cancel();//Repository 怎么取消？Manager取消有用吗？
                 Services.Notifier.Error(T("数据异常！"));
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Services.Notifier.Information(T("收费成功！"));
            var redirect = Url.Action("Index");
            return Json(new { redirectUrl = redirect });
        }

        private decimal? CalculateDelayChargeAmount(BillRecord billRecord)
        {
            decimal? delayChargeAmount = 0;
            var intervalDays = 0;
            var record = billRecord.ChargeItem;
            var delayChargeCalculationMethod = DelayChargeCalculationMethodOption.手动计算滞纳金; //record.ChargeItem.DelayChargeCalculationMethod;
            var startCalculationDatetime = StartCalculationDatetimeOption.欠费开始时间; //record.ChargeItem.StartCalculationDatetime;
            var delayChargeDays = 0;//record.ChargeItem.DelayChargeDays;
            var delayChargeRatio = 0; //Convert.ToDecimal(record.ChargeItem.DelayChargeRatio);
            if (delayChargeCalculationMethod == DelayChargeCalculationMethodOption.自动计算滞纳金)
            {
                if (startCalculationDatetime == StartCalculationDatetimeOption.欠费开始时间)
                {
                    intervalDays = (DateTime.Now.Date - billRecord.StartDate.Date).Days -
                                   Convert.ToInt32(delayChargeDays);
                }
                if (startCalculationDatetime == StartCalculationDatetimeOption.欠费结束时间)
                {
                    intervalDays = (DateTime.Now.Date -billRecord.EndDate.Date).Days - 
                                   Convert.ToInt32(delayChargeDays);
                }
                if (intervalDays > 0)
                    delayChargeAmount = billRecord.Amount * delayChargeRatio * intervalDays;
            }
            if (delayChargeCalculationMethod == DelayChargeCalculationMethodOption.手动计算滞纳金 
                || billRecord.DelayCharge!=null)
            {
                delayChargeAmount = billRecord.DelayCharge;
            }
            return delayChargeAmount;
        }

        [HttpPost]
        public ActionResult ChargeHistoriyDetailList(int paymentId)
        {
            var query = _paymentLineItemRepository.Table;

            var records = query
                .Where(x => x.Bill.Status == BillRecord.BillStatusOption.已结账单
                            && x.Payment.Id == paymentId)
                .Select(item => new PaymentListViewModel()
                {
                    ContractNumber = item.Bill.Contract.Number,
                    HouseNumber = item.Bill.House.HouseNumber,
                    ChargeItemSettingDescription = item.Bill.ChargeItem.Name,
                    BeginDate = item.Bill.StartDate.ToString("yyyy/MM/dd"),
                    EndDate = item.Bill.EndDate.ToString("yyyy/MM/dd"),
                    Year = item.Bill.Year,
                    Month = item.Bill.Month,
                    Amount = item.Bill.Amount,
                    Exempt = item.Bill.Exempt,
                    DelayCharge =item.Bill.DelayCharge,
                    Status = item.Bill.Status.ToString(),
                    Notes = item.Bill.Notes,
                    ApartmentName = item.Bill.House.Apartment.Name,
                    BuildingName = item.Bill.House.Building.Name
                }).ToList();
            var name = _paymentRepository.Get(paymentId).Operator.UserName;
            var date = _paymentRepository.Get(paymentId).PaidOn.Value.ToString("yyyy/MM/dd");
            return Json(new { List = records, ChargeUser = name, ChargeDate = date });
        }

        [HttpPost]
        public ActionResult ChargeHistoryList(FormCollection input, int page = 1, int pageSize = 10,
            string sortBy = null,
            string sortOrder = "asc")
        {
            var query = _paymentRepository.Table;
            var options = new PaymentFilterOptions();
            UpdateModel(options, "Filter", input);
            bool isHasValue = false;
            if (options.ExpenserId.HasValue)
            {
                query = query.Where(c => c.CustomerId.Id == options.ExpenserId);
                isHasValue = true;
            }

            var list = new List<ChargeHistoryListViewModel>();
            if (isHasValue)
            {
                var records = query
                    .Select(item => new ChargeHistoryListViewModel()
                    {
                        PaymentId = item.Id,
                        PaidTime = item.PaidOn,
                        Paid = item.Paid,
                        Operator = item.Operator.UserName,
                        CustomerName = item.CustomerId.Name
                    }).ToList();
                list = records;
            }
            int totalRecords = list.Count;
            list = list.Skip((page - 1)*pageSize)
                .Take(pageSize).ToList();
            var result = new
            {
                page,
                totalPages = Math.Ceiling((double) totalRecords/pageSize),
                totalRecords,
                rows = list
            };
            return Json(result);
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