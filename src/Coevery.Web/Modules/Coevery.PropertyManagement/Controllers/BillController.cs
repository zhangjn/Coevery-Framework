using System;
using System.Collections.Generic;
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
using Coevery.PropertyManagement.Extensions;
using Coevery.Security;
using Coevery.Themes;
using Coevery.UI.Notify;

namespace Coevery.PropertyManagement.Controllers {
    [Themed]
    public class BillController : Controller, IUpdateModel {
        private readonly IRepository<ContractPartRecord> _contractPartRecordRepository;
        private readonly IRepository<ContractHouseRecord> _contractHouseRecordRepository;
        private readonly IRepository<HousePartRecord> _housePartRecordRepository;
        private readonly IRepository<ChargeItemSettingCommonRecord> _chargeItemSettingCommonRecordRepository;
        private readonly IRepository<BillRecord> _repositoryBill;
        private readonly IBillServices _billServices;
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<ApartmentPartRecord> _apartmentRepository;

        public BillController(ICoeveryServices services,
            ITransactionManager transactionManager,
            IRepository<ContractHouseRecord> contractHouseRecordRepository,
            IRepository<BillRecord> repositoryBill,
            IBillServices billServices,
            IRepository<HousePartRecord> housePartRecordRepository,
            IRepository<ContractPartRecord> contractPartRecordRepository, 
            IRepository<ChargeItemSettingCommonRecord> chargeItemSettingCommonRecordRepository, 
            IRepository<ApartmentPartRecord> apartmentRepository) {
            Services = services;
            _transactionManager = transactionManager;
            _contractHouseRecordRepository = contractHouseRecordRepository;
            _repositoryBill = repositoryBill;
            _billServices = billServices;
            _housePartRecordRepository = housePartRecordRepository;
            _contractPartRecordRepository = contractPartRecordRepository;
            _chargeItemSettingCommonRecordRepository = chargeItemSettingCommonRecordRepository;
            _apartmentRepository = apartmentRepository;
            T = NullLocalizer.Instance;
        }

        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties,
            string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }

        public ActionResult Index() {
            return List();
        }

        public ActionResult List() {
            if (!Services.Authorizer.Authorize(Permissions.BillManage, T("没有账单查看权限")))
                return new HttpUnauthorizedResult();
            return View("List");
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc") {
            if (!Services.Authorizer.Authorize(Permissions.BillManage, T("没有账单查看权限")))
                return new HttpUnauthorizedResult();
            //查询数据，合同，房间，收费项目，收费标准，抄表数据录入
            var list = QueryBillRecords(input);

            int totalRecords = list.Count;
            list = list.Skip((page - 1)*pageSize)
                .Take(pageSize).ToList();
            var result = new {
                page,
                totalPages = Math.Ceiling((double)totalRecords/pageSize),
                totalRecords,
                rows = list
            };
            return Json(result);
        }

        private List<BillListViewModel> QueryBillRecords(FormCollection input) {
            IQueryable<ContractHouseRecord> query = _contractHouseRecordRepository.Table; //默认从合同房间里面去找

            //这里加过滤器的代码
            var options = new BillFilterOptions();
            UpdateModel(options, "Filter", input);
            if (!string.IsNullOrEmpty(options.ApartmentId))
            {
                int[] apartmentIdArray = Array.ConvertAll(options.ApartmentId.Split(','), s => Int32.Parse(s.Trim()));
                query = query.Where(x => apartmentIdArray.Contains(x.House.Apartment.Id));
              
            }
            else
            {
                options.BuildingId = string.Empty;
                options.HouseNumber = string.Empty;
            }
            if (!string.IsNullOrEmpty(options.BuildingId))
            {
                int[] buildingIdArray = Array.ConvertAll(options.BuildingId.Split(','), s => Int32.Parse(s.Trim()));
                query = query.Where(x => buildingIdArray.Contains(x.House.Building.Id));
            }
            else
            {
                options.HouseNumber = string.Empty;
            }
            if (!string.IsNullOrEmpty(options.HouseNumber)) {

                int[] houseNumberArray = Array.ConvertAll(options.HouseNumber.Split(','), s => Int32.Parse(s.Trim()));
                query = query.Where(x => houseNumberArray.Contains(x.House.Id));
            }

            if (!string.IsNullOrEmpty(options.RenterIds))
            {
                int[] renterIdArray = Array.ConvertAll(options.RenterIds.Split(','), s => Int32.Parse(s.Trim()));
                query = query.Where(c => renterIdArray.Contains(c.Contract.Renter.Id));
            }
            if (!string.IsNullOrEmpty(options.OwnerIds))
            {
                int[] ownerIdsArray = Array.ConvertAll(options.OwnerIds.Split(','), s => Int32.Parse(s.Trim()));
                query = query.Where(c => ownerIdsArray.Contains(c.House.Owner.Id));
            }
            if (!string.IsNullOrEmpty(options.ContractNumber)) {
                query = query.Where(c => c.Contract.Number.Contains(options.ContractNumber));
            }

            List<ContractHouseRecord> records = query.ToList();

            var list = _billServices.GenerateBillListViewModels(records).ToList();
            return list;
        }

        [HttpPost]
        public ActionResult GenerateBill(FormCollection input) {
            var records = QueryBillRecords(input);
            var billRecords = records.Where(x => x.Amount != null).Select(x => new BillRecord {
                Contract = _contractPartRecordRepository.Get(x.ContractId),
                House = _housePartRecordRepository.Get(x.HouseId),
                ChargeItem = _chargeItemSettingCommonRecordRepository.Get(x.ChargeItemSettingId),
                CustomerId = x.CustomerId,
                Amount = x.Amount,
                StartDate = x.BeginDate,
                EndDate = x.EndDate,
                Year = x.EndDate.Year,
                Month = x.EndDate.Month,
                Quantity = x.Quantity,
                Status = BillRecord.BillStatusOption.已出账单
            });
            foreach (var record in billRecords) {
                _repositoryBill.Create(record);
            }
            Services.Notifier.Information(T("账单生成成功！"));
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [HttpPost]
        public ActionResult ApartmentDropdown(string term, int page = 1, int pageSize =10, string sortBy = null,
            string sortOrder = "asc") {
            var query = _apartmentRepository.Table;
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.Name.Contains(term));
            }
            var totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Select(item => new
                {
                    id = item.Id,
                    text = item.Name
                }).ToList();
            return Json(new { records, total = totalRecords });
        }
        [HttpPost]
        public ActionResult BuildingDropdown(string term, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
            var query = Services.ContentManager.Query<BuildingPart, BuildingPartRecord>();
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
        [HttpPost]
        public ActionResult BuildingDropdownById(string id, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
          
            var query = Services.ContentManager.Query<BuildingPart, BuildingPartRecord>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                int[] apartmentIdArray = Array.ConvertAll(id.Split(','), s => Int32.Parse(s.Trim()));
                query = query.Where(x => apartmentIdArray.Contains(x.Apartment));
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
        [HttpPost]
        public ActionResult HouseDropdown(string term, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
            var query = Services.ContentManager.Query<HousePart, HousePartRecord>();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.HouseNumber.Contains(term));
            }
            var totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1) * pageSize, pageSize)
                .Select(item => new
                {
                    id = item.Record.ContentItemRecord.Id,
                    text = item.HouseNumber
                }).ToList();
            return Json(new { records, total = totalRecords });
        }
        [HttpPost]
        public ActionResult HouseDropdownById(string id, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
           
            var query = Services.ContentManager.Query<HousePart, HousePartRecord>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                int[] buildingIdArray = Array.ConvertAll(id.Split(','), s => Int32.Parse(s.Trim()));
                query = query.Where(x => buildingIdArray.Contains(x.Building.Id));
            }
            var totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1) * pageSize, pageSize)
                .Select(item => new
                {
                    id = item.Record.ContentItemRecord.Id,
                    text = item.HouseNumber
                }).ToList();
            return Json(new { records, total = totalRecords });
        }
    }
}