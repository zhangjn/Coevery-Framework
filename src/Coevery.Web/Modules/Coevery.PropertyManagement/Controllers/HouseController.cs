using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Localization;
using Coevery.PropertyManagement.Extensions;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;
using Coevery.Security;
using Coevery.Themes;
using Coevery.UI.Notify;
using Coevery.Users.Models;
using NHibernate.Linq;
using NHibernate.Mapping;
using NHibernate.Util;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class HouseController : Controller, IUpdateModel
    {
        private readonly IRepository<ContractPartRecord> _contractRepository;
        private readonly IRepository<ContractHouseRecord> _repositoryContractHouse;
        private readonly IRepository<HouseChargeItemRecord> _repositoryHouseChargeItem;
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<ApartmentPartRecord> _apartmentRepository;
        private readonly IRepository<BuildingPartRecord> _buildingRepository;
        private readonly IRepository<CustomerPartRecord> _customerRepository;
        private readonly IRepository<UserPartRecord> _userRepository;
        public HouseController(ICoeveryServices services,
            ITransactionManager transactionManager,
            IRepository<HouseChargeItemRecord> repositoryHouseChargeItem,
            IRepository<ContractChargeItemRecord> repositoryContractChargeItem,
            IRepository<ContractPartRecord> contractRepository,
            IRepository<ContractHouseRecord> repositoryContractHouse,
            IRepository<ApartmentPartRecord> apartmentRepository, 
            IRepository<BuildingPartRecord> buildingRepository,
            IRepository<CustomerPartRecord> customerRepository, 
            IRepository<UserPartRecord> userRepository)
        {
            Services = services;
            _transactionManager = transactionManager;
            _repositoryHouseChargeItem = repositoryHouseChargeItem;
            _contractRepository = contractRepository;
            _repositoryContractHouse = repositoryContractHouse;
            _apartmentRepository = apartmentRepository;
            _buildingRepository = buildingRepository;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
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

        [HttpPost]
        public ActionResult Import()
        {
            var model = new ImportViewModel();
            var result = new List<ImportListModel>();
            var file = Request.Files["importFile"];
            var apartmentRecords = _apartmentRepository.Table;
            var buildingRecords = _buildingRepository.Table;
            var customerRecords = _customerRepository.Table;
            var userRecords = _userRepository.Table;
            if (file == null)
            {

                Services.Notifier.Information(T("导入失败！"));
                return List();
            }

            var workbook = new HSSFWorkbook(file.InputStream);
            var sheet = workbook[0];
            bool isFirst = true;
            try
            {
                foreach (HSSFRow row in sheet) {
                    if (isFirst) {
                        if (row.PhysicalNumberOfCells < 9)
                        {
                            result.Add(new ImportListModel()
                            {
                                RowId = 0,
                                FieldName = "表头",
                                ErrorMessage = "内容格式有误"
                            });
                            break;
                        }
                        isFirst = false;
                        if (row.Cells[0].StringCellValue != "楼盘"
                            || row.Cells[1].StringCellValue != "楼宇"
                            || row.Cells[2].StringCellValue != "房间号"
                            || row.Cells[3].StringCellValue != "业主姓名"
                            || row.Cells[4].StringCellValue != "专管业务员"
                            || row.Cells[5].StringCellValue != "建筑面积"
                            || row.Cells[6].StringCellValue != "套内面积"
                            || row.Cells[7].StringCellValue != "公摊面积"
                            || row.Cells[8].StringCellValue != "房间状态"
                            ) {
                            result.Add(new ImportListModel() {
                                RowId = 0,
                                FieldName = "表头",
                                ErrorMessage = "内容格式有误"
                            });
                            model.List = result;
                            return View(model);
                        }
                        continue;
                    }
                    var apartmentId = 0;
                    var buildingId = 0;
                    var ownerId = 0;
                    var officerId = 0;
                    var apartmentName = row.GetCellValue<string>(0);
                    var buildingName = row.GetCellValue<string>(1);
                    var houseNumber = row.GetCellValue<string>(2);
                    var ownerName = row.GetCellValue<string>(3);
                    var officerName = row.GetCellValue<string>(4);
                    var buildingArea = row.GetCellValue<double?>(5);
                    var insideArea = row.GetCellValue<double?>(6);
                    var poolArea = row.GetCellValue<double?>(7);
                    var apartmentIdArray = apartmentRecords.Where(x => x.Name == apartmentName)
                        .Select(x => x.Id)
                        .ToArray();
                    if (apartmentIdArray.Length > 0)
                        apartmentId = apartmentIdArray[0];
                    else {
                        result.Add(new ImportListModel() {
                            RowId = row.RowNum,
                            FieldName = "楼盘--" + apartmentName,
                            ErrorMessage = "数据库中不存在此楼盘"
                        });
                        continue;
                    }

                    var buildingIdArray = buildingRecords.Where(x => x.Name == buildingName)
                        .Select(x => x.Id)
                        .ToArray();
                    if (buildingIdArray.Length > 0)
                        buildingId = buildingIdArray[0];
                    else {
                        result.Add(new ImportListModel() {
                            RowId = row.RowNum,
                            FieldName = "楼宇--" + buildingName,
                            ErrorMessage = "数据库中不存在此楼宇"
                        });
                        continue;
                    }
                    var ownerIdArray = customerRecords.Where(x => x.Name == ownerName)
                        .Select(x => x.Id)
                        .ToArray();
                    if (ownerIdArray.Length > 0)
                        ownerId = ownerIdArray[0];
                    else {
                        result.Add(new ImportListModel() {
                            RowId = row.RowNum,
                            FieldName = "业主--" + ownerName,
                            ErrorMessage = "数据库中不存在此业主"
                        });
                        continue;
                    }
                    var officerIdArray = userRecords.Where(x => x.UserName == officerName)
                        .Select(x => x.Id)
                        .ToArray();
                    if (officerIdArray.Length > 0)
                        officerId = officerIdArray[0];
                    else {
                        result.Add(new ImportListModel() {
                            RowId = row.RowNum,
                            FieldName = "专管员--" + officerName,
                            ErrorMessage = "数据库中不存在此专管员"
                        });
                        continue;
                    }
                    var houses = Services.ContentManager.Query<HousePart, HousePartRecord>()
                        .Where(h => h.Apartment.Id == apartmentId
                                    && h.Building.Id == buildingId
                                    && h.HouseNumber == houseNumber)
                        .List();
                    var houseParts = houses as IList<HousePart> ?? houses.ToList();
                    if (houseParts.Any(h => h.HouseNumber == houseNumber)) {
                        var id = houseParts.Select(x => x.Id).ToArray()[0];
                        ContentItem contentItem = Services.ContentManager.Get(id);
                        var part = contentItem.As<HousePart>();
                        HousePartRecord record = part.Record;
                        record.BuildingArea = buildingArea;
                        record.InsideArea = insideArea;
                        record.PoolArea = poolArea;
                        part.OfficerId = officerId;
                        Services.ContentManager.Create(contentItem, VersionOptions.Draft);
                        Services.ContentManager.Publish(contentItem);
                    }
                    else {
                        ContentItem contentItem = Services.ContentManager.New("House");
                        var part = contentItem.As<HousePart>();
                        HousePartRecord record = part.Record;
                        part.ApartmentId = apartmentId;
                        part.BuildingId = buildingId;
                        part.OwnerId = ownerId;
                        record.HouseNumber = houseNumber;
                        record.HouseStatus = HouseStatusOption.空置;
                        record.BuildingArea = buildingArea;
                        record.InsideArea = insideArea;
                        record.PoolArea = poolArea;
                        part.OfficerId = officerId;
                        Services.ContentManager.Create(contentItem, VersionOptions.Draft);
                        Services.ContentManager.Publish(contentItem);
                    }
                }
                model.List = result;
            }
            catch (Exception ex)
            {
                result.Add(new ImportListModel()
                {
                    RowId = 0,
                    ErrorMessage = ex.Message
                });
                model.List = result;
                return View(model);
            }

            if (result.Count > 0)
                return View(model);

            Services.Notifier.Information(T("导入成功！"));
            return List();
        }

        [HttpGet]
        public ActionResult DownLoadTemplate()
        {
            var path = Server.MapPath("~/App_Data/房间数据模板.xls");
            var name = Path.GetFileName(path);
            return File(path, "application/zip-x-compressed", name);
        }
        public ActionResult Index()
        {
            return List();
        }

        public ActionResult List()
        {
            if (
                !Services.Authorizer.Authorize(StandardPermissions.View, Services.ContentManager.New("House"),
                    T("没有房间查看权限！")))
                return new HttpUnauthorizedResult();
            List<SelectListItem> apartments = _apartmentRepository.Table
                .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();
            apartments.Insert(0, new SelectListItem());

            List<SelectListItem> buildings = Services.ContentManager.Query<BuildingPart, BuildingPartRecord>().List()
                .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();
            buildings.Insert(0, new SelectListItem());

            List<SelectListItem> chargeItems =
                Services.ContentManager.Query<ChargeItemSettingPart, ChargeItemSettingPartRecord>().List()
                    .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();
            chargeItems.Insert(0, new SelectListItem());

            List<SelectListItem> houseNumbers = Services.ContentManager.Query<HousePart, HousePartRecord>().List()
                .Select(c => new SelectListItem {Text = c.HouseNumber, Value = c.HouseNumber}).Distinct().ToList();
            houseNumbers.Insert(0, new SelectListItem());

            ViewData["Apartments"] = apartments; //楼盘
            ViewData["Buildings"] = buildings; //楼宇
            ViewData["HouseNumbers"] = houseNumbers; //房号
            ViewData["ChargeItems"] = chargeItems; //收费项目
            ContentItem contentItem = Services.ContentManager.New("House");
            contentItem.Weld(new HousePart());
            dynamic model = Services.ContentManager.BuildDisplay(contentItem, "List");
            return View("List", model);
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
            if (
                !Services.Authorizer.Authorize(StandardPermissions.View, Services.ContentManager.New("House"),
                    T("没有房间查看权限！")))
                return new HttpUnauthorizedResult();
            IContentQuery<HousePart, HousePartRecord> query =
                Services.ContentManager.Query<HousePart, HousePartRecord>();
            Dictionary<int, ApartmentPartRecord> apartmentRecords =
                _apartmentRepository.Table.ToDictionary(x => x.Id);
            Dictionary<int, BuildingPart> buildingRecords =
                Services.ContentManager.Query<BuildingPart, BuildingPartRecord>().List().ToDictionary(x => x.Id);

            //这里加过滤器的代码
            var options = new HouseFilterOptions();
            UpdateModel(options, "Filter", input);
            if (options.ApartmentId.HasValue)
            {
                query = query.Where(c => c.Apartment.Id == options.ApartmentId);
            }
            if (options.BuildingId.HasValue)
            {
                query = query.Where(c => c.Building.Id == options.BuildingId);
            }
            if (!string.IsNullOrEmpty(options.HouseNumber))
            {
                query = query.Where(c => c.HouseNumber.Contains(options.HouseNumber));
            }
            if (options.HouseStatusId.HasValue)
            {
                query = query.Where(c => c.HouseStatus == (HouseStatusOption) options.HouseStatusId);
            }
            if (!string.IsNullOrEmpty(options.CustomerName))
            {
                List<int> customerIds =
                    Services.ContentManager.Query<CustomerPart, CustomerPartRecord>()
                        .Where(c => c.Name.Contains(options.CustomerName))
                        .List()
                        .Select(x => x.Id)
                        .ToList();
                query = query.Where(x => customerIds.Contains(x.Owner.Id));
            }
            if (options.BuildingArea.HasValue)
            {
                query = query.Where(c => c.BuildingArea >= options.BuildingArea);
            }
            if (options.ChargeItemSettingId.HasValue)
            {
                List<int> houseIds =
                    _repositoryHouseChargeItem.Table
                        .Where(c => c.Id == options.ChargeItemSettingId.Value)
                        .ToList()
                        .Select(m => m.House.Id)
                        .ToList();
                query = query.Where(c => houseIds.Contains(c.Id));
            }

            var houseList = query.List();
            var totalBuildingArea = houseList.Sum(x => x.BuildingArea);
            var totalInsideArea = houseList.Sum(x => x.InsideArea);
            var totalPoolArea = houseList.Sum(x => x.PoolArea);
            int totalRecords = query.Count();
            List<HouseListViewModel> records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1)*pageSize, pageSize)
                .Select(item => new HouseListViewModel
                {
                    Id = item.Record.ContentItemRecord.Id,
                    VersionId = item.Record.Id,
                    Apartment = item.Record.Apartment.Id,
                    Building = item.Record.Building.Id,
                    HouseNumber = item.Record.HouseNumber,
                    InsideArea = item.Record.InsideArea,
                    BuildingArea = item.Record.BuildingArea,
                    PoolArea = item.Record.PoolArea,
                    HouseStatus = item.Record.HouseStatus.ToString(),//GetHouseStatus(item.Record.Id)
                    OwnerName = item.Record.Owner != null ? item.Record.Owner.Name : null,
                    OfficerName = item.Record.Officer != null ? item.Record.Officer.UserName : null,
                    Contact = item.Record.Owner != null ? item.Record.Owner.Phone : null,
                    ChargeItemNames = string.Join(@"\", item.Record.ChargeItems.Select(x => x.Name)),
                    MeterItemNames = string.Join(@"\", item.Record.MeterTypeItems.Select(i => i.MeterType.Name))
                }).ToList();


            foreach (HouseListViewModel record in records)
            {
                if (record.Apartment.HasValue && apartmentRecords.ContainsKey(record.Apartment.Value))
                {
                    record.Apartment_Name = apartmentRecords[record.Apartment.Value].Name;
                }
                if (record.Building.HasValue && buildingRecords.ContainsKey(record.Building.Value))
                {
                    record.Building_Name = buildingRecords[record.Building.Value].Name;
                }
            }
        

            var result = new
            {
                page,
                totalPages = Math.Ceiling((double) totalRecords/pageSize),
                totalRecords,
                rows = records,
                totalBuildingArea,
                totalInsideArea,
                totalPoolArea
            };
            return Json(result);
        }

        public string GetHouseStatus(int id)
        {
            string str = HouseStatusOption.空置.ToString();//默认为空置状态
            List<ContractPartRecord> contracts =
                _contractRepository.Table.Where(c => c.Houses.Any(m => m.House.Id == id)).ToList();
            contracts.ForEach(m =>
            {
                if (m.ContractStatus!=ContractStatusOption.终止)
                {
                    str = m.HouseStatus.ToString(); //合同还在有效期，房间状态为合同中房间的状态
                }
            });
            return str;
        }

        private DateTime? SpecifyDateTimeKind(DateTime? utcDateTime)
        {
            if (utcDateTime != null)
                return DateTime.SpecifyKind(utcDateTime.Value, DateTimeKind.Utc);
            return null;
        }

        [HttpPost]
        public ActionResult BuildingDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc")
        {
            IContentQuery<BuildingPart, BuildingPartRecord> query =
                Services.ContentManager.Query<BuildingPart, BuildingPartRecord>();
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
        public ActionResult ApartmentDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc") {
            var query = _apartmentRepository.Table;
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.Name.Contains(term));
            }
            int totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Skip((page - 1)*pageSize).Take(pageSize)
                .Select(item => new
                {
                    id = item.Id,
                    text = item.Name
                }).ToList();
            return Json(new {records, total = totalRecords});
        }

        [HttpPost]
        public ActionResult OwnerDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
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

        public ActionResult Detail(int id)
        {
            ContentItem contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null)
            {
                return HttpNotFound();
            }

            dynamic model = Services.ContentManager.BuildDisplay(contentItem, "Detail");
            return View((object) model);
        }

        public ActionResult Create()
        {
            if (
                !Services.Authorizer.Authorize(StandardPermissions.Create, Services.ContentManager.New("House"),
                    T("没有房间创建权限！")))
                return new HttpUnauthorizedResult();
            List<SelectListItem> apartments = _apartmentRepository.Table
                .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();

            List<SelectListItem> buildings = Services.ContentManager.Query<BuildingPart, BuildingPartRecord>().List()
                .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();

            List<SelectListItem> customers = Services.ContentManager.Query<CustomerPart, CustomerPartRecord>().List()
                .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();

            List<SelectListItem> chargeItems =
                Services.ContentManager.Query<ChargeItemSettingPart, ChargeItemSettingPartRecord>().List()
                    .Where(x => x.ItemCategory != ItemCategoryOption.临时性收费项目)
                    .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()})
                    .ToList();

            List<SelectListItem> meterItems = Services.ContentManager.Query<MeterTypePart, MeterTypePartRecord>().List()
                .Select(it => new SelectListItem
                {
                    Text = it.Name,
                    Value = it.Id.ToString()
                }).ToList();

            List<SelectListItem> officerItems = Services.ContentManager.Query<UserPart, UserPartRecord>().List()
                .Select(it => new SelectListItem
                {
                    Text = it.UserName,
                    Value = it.Id.ToString()
                }).ToList();

            var model = new HouseCreateViewModel();
            model.ApartmentListItems = apartments; //楼盘列表
            model.BuildingListItems = buildings; //楼宇列表
            model.OwnerListItems = customers; //业主列表

            model.ChargeListItems = chargeItems; //项目收费标准列表
            model.MeterListItems = meterItems; //仪表列表
            model.ExpenserListItems = customers;
            model.OfficerListItems = officerItems;
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(HouseCreateViewModel model)
        {
            ContentItem contentItem = Services.ContentManager.New("House"); //新建一个House类型的contentItemd对象
            if (!Services.Authorizer.Authorize(StandardPermissions.Create, contentItem, T("没有房间创建权限！")))
                return new HttpUnauthorizedResult();
            Services.ContentManager.Create(contentItem, VersionOptions.Draft); //创建一个草稿对象
            var part = contentItem.As<HousePart>();
            HousePartRecord record = part.Record;
            //房间唯一性验证
            List<HousePart> houses = Services.ContentManager.Query<HousePart, HousePartRecord>().List().ToList();

            if (
                houses.Any(
                    h =>
                        h.Apartment.Id == model.ApartmentId && h.Building.Id == model.BuildingId &&
                        h.HouseNumber.Trim() == model.HouseNumber.Trim()))
            {
                //Services.Notifier.Information(T("不能创建相同的房间！"));
                return Json(new {redirectUrl = ""}); //返回一个空url，页面上根据这个值弹出提示信息
            }

            //1、对House的基础数据赋值
            part.ApartmentId = model.ApartmentId;
            part.BuildingId = model.BuildingId;
            part.OwnerId = model.OwnerId;
            record.HouseNumber = model.HouseNumber;
            record.HouseStatus = HouseStatusOption.空置; //房间默认为空置状态
            record.BuildingArea = model.BuildingArea;
            record.InsideArea = model.InsideArea;
            record.PoolArea = model.PoolArea;
            part.OfficerId = model.OfficerId;

            //2、对House的两个Items赋值
            model.ChargeItemEntities.ForEach(it =>
            {
                var chargeItemRecord = new HouseChargeItemRecord();
                chargeItemRecord.ChargeItemSetting =
                    Services.ContentManager.Get<ChargeItemSettingPart>(it.ChargeItemSettingId).Record;
                chargeItemRecord.ItemCategory = it.ItemCategory;
                chargeItemRecord.ChargeItemSetting.CopyPropertiesTo(chargeItemRecord, "Id");
                chargeItemRecord.CalculationMethod = it.CalculationMethod;
                chargeItemRecord.MeterTypeId = it.MeterType;
                chargeItemRecord.UnitPrice = it.UnitPrice;
                chargeItemRecord.MeteringMode = it.MeteringMode;
                chargeItemRecord.Money = it.Money;
                chargeItemRecord.CustomFormula = it.CustomFormula;
                chargeItemRecord.ChargingPeriod = it.ChargingPeriod;
                chargeItemRecord.ExpenserOption = it.ExpenserOptionId;
                chargeItemRecord.BeginDate = Convert.ToDateTime(it.BeginDate);
                if (!string.IsNullOrEmpty(it.EndDate))
                {
                    chargeItemRecord.EndDate = Convert.ToDateTime(it.EndDate);
                }
                chargeItemRecord.Description = it.Description;
                chargeItemRecord.House = record;
                record.ChargeItems.Add(chargeItemRecord);
            });
            model.MeterTypeItemEntities.ForEach(m =>
            {
                var meterItem = new HouseMeterRecord();
                meterItem.MeterType = Services.ContentManager.Get<MeterTypePart>(m.MeterTypeItemId).Record;
                meterItem.MeterNumber = m.MeterNumber;
                meterItem.Ratio = m.Ratio;
                meterItem.House = record;
                record.MeterTypeItems.Add(meterItem);
            });


            Services.ContentManager.Publish(contentItem);
            Services.Notifier.Information(T("房间创建成功！"));
            string redirect = Url.Action("Index");
            return Json(new {redirectUrl = redirect});
        }

        public ActionResult Edit(int id)
        {
            if (
                !Services.Authorizer.Authorize(StandardPermissions.Edit, Services.ContentManager.New("House"),
                    T("没有房间编辑权限！")))
                return new HttpUnauthorizedResult();
            var part = Services.ContentManager.Get<HousePart>(id);
            HousePartRecord house = part.Record; //根据Id获得房间实体
            List<SelectListItem> apartments = _apartmentRepository.Table
                .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();

            List<SelectListItem> buildings = Services.ContentManager.Query<BuildingPart, BuildingPartRecord>().List()
                .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();

            List<SelectListItem> customers = Services.ContentManager.Query<CustomerPart, CustomerPartRecord>().List()
                .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();

            List<SelectListItem> chargeItems =
                Services.ContentManager.Query<ChargeItemSettingPart, ChargeItemSettingPartRecord>().List()
                    .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()}).ToList();

            List<SelectListItem> meterItems = Services.ContentManager.Query<MeterTypePart, MeterTypePartRecord>().List()
                .Select(it => new SelectListItem
                {
                    Text = it.Name,
                    Value = it.Id.ToString()
                }).ToList();

            List<SelectListItem> officerItems = Services.ContentManager.Query<UserPart, UserPartRecord>().List()
                .Select(it => new SelectListItem
                {
                    Text = it.UserName,
                    Value = it.Id.ToString()
                }).ToList();

            var model = new HouseEditViewModel();
            model.ApartmentListItems = apartments; //楼盘列表
            model.BuildingListItems = buildings; //楼宇列表
            model.OwnerListItems = customers; //业主列表
            model.ExpenserListItems = customers;
            model.ChargeListItems = chargeItems; //项目下拉列表
            model.MeterListItems = meterItems; //仪表下拉列表
            model.OfficerListItems = officerItems;

            model.Id = id; //房间Id，给页面隐藏域
            model.ApartmentId = house.Apartment.Id;
            model.BuildingId = house.Building.Id;
            model.OwnerId = part.OwnerId ?? 0;
            model.OwnerName = part.Owner.Name;
            model.HouseNumber = house.HouseNumber;
            model.OfficerId = part.OfficerId ?? 0;
            model.HouseStatusId =part.HouseStatus.HasValue?(int)part.HouseStatus:(int)HouseStatusOption.空置;
            //model.HouseStatusId = (int) HouseStatusOption.空置; //默认空置状态
            //List<ContractPartRecord> contracts =
            //    _contractRepository.Table.Where(c => c.Houses.Any(m => m.House.Id == id)).ToList();
            //contracts.ForEach(m =>
            //{
            //    if (m.ContractStatus != ContractStatusOption.终止)
            //    {
            //        if (m.HouseStatus.HasValue)
            //        {
            //            model.HouseStatusId = (int) m.HouseStatus; //合同还没有终止，房间状态为合同中的房间状态
            //        }
            //    }
            //});
            model.BuildingArea = house.BuildingArea ?? 0;
            model.InsideArea = house.InsideArea ?? 0;
            model.PoolArea = house.PoolArea ?? 0;

            house.ChargeItems.ForEach(h =>
            {
                var chargeEntity = new HouseChargeItemEntity();
                chargeEntity.Id = h.Id;
                chargeEntity.ChargeItemSettingId = h.ChargeItemSetting.Id;
                chargeEntity.ChargeItemSettingName = h.Name;
                chargeEntity.ItemCategory = h.ItemCategory;
                chargeEntity.ItemCategoryDisplayName = h.ItemCategory.ToString();
                chargeEntity.CalculationMethod = h.CalculationMethod;
                chargeEntity.CalculationMethodDisplayName = h.CalculationMethod.ToString();
                chargeEntity.MeterType = h.MeterTypeId;
                chargeEntity.UnitPrice = h.UnitPrice;
                chargeEntity.MeteringMode = h.MeteringMode;
                chargeEntity.Money = h.Money;
                chargeEntity.CustomFormula = h.CustomFormula;
                chargeEntity.ChargingPeriod = h.ChargingPeriod;
                chargeEntity.ChargingPeriodDisplayName = h.ChargingPeriod.ToString();
                chargeEntity.BeginDate = h.BeginDate.ToString("MM/dd/yyyy");
                chargeEntity.EndDate = h.EndDate.HasValue ? h.EndDate.Value.ToString("MM/dd/yyyy") : null; //结束时间应该是可为空的
                chargeEntity.Description = h.Description;
                chargeEntity.HouseId = h.House.Id;
                chargeEntity.ExpenserOptionId = h.ExpenserOption;
                chargeEntity.ExpenserName = ((ExpenserOption)h.ExpenserOption).ToString(); //h.Expenser.Name;
                model.ChargeItemEntities.Add(chargeEntity);
            });

            house.MeterTypeItems.ForEach(m =>
            {
                var meterEntity = new HouseMeterEntity
                {
                    Id = m.Id,
                    HouseId = m.House.Id,
                    MeterTypeItemId = m.MeterType.Id,
                    MeterName = m.MeterType.Name,
                    MeterNumber = m.MeterNumber,
                    Ratio = m.Ratio
                };
                model.MeterTypeItemEntities.Add(meterEntity);
            });

            IQueryable<ContractPartRecord> contractHouse =
                _repositoryContractHouse.Table.Where(x => x.House.Id == id).Select(x => x.Contract);
            foreach (ContractPartRecord contracthouse in contractHouse)
            {
                contracthouse.ChargeItems.ForEach(m =>
                {
                    var item = new ContractChargeItemEntity();
                    item.ChargeItemName = m.Name;
                    item.ChargeItemSettingId = m.ChargeItemSetting.Id;
                    item.ChargeItemSettingName = m.ChargeItemSetting.Name;
                    item.ChargeBeginDate = m.BeginDate.ToString("MM/dd/yyyy");
                    item.ChargeEndDate = m.EndDate.HasValue ? m.EndDate.Value.ToString("MM/dd/yyyy") : null;
                    item.ChargeDescription = m.Description;
                    item.ExpenserName = ((ExpenserOption)m.ExpenserOption).ToString();
                    item.ContractId = m.Id;
                    item.Id = m.Id;
                    model.ContractChargeItemEntities.Add(item);
                });
            }
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int id, HouseEditViewModel model)
        {
            ContentItem contentItem = Services.ContentManager.Get(model.Id);
            if (!Services.Authorizer.Authorize(StandardPermissions.Edit, contentItem, T("没有房间编辑权限！")))
                return new HttpUnauthorizedResult();
            var part = contentItem.As<HousePart>();
            HousePartRecord record = part.Record;
            //编辑页面房间唯一性验证，跟别人比较，要先排除自己
            List<HousePart> houses = Services.ContentManager.Query<HousePart, HousePartRecord>()
                .Where(h => h.Id != id).List().ToList();
            if (
                houses.Any(
                    h =>
                        h.Apartment.Id == model.ApartmentId && h.Building.Id == model.BuildingId &&
                        h.HouseNumber.Trim() == model.HouseNumber.Trim()))
            {
                //Services.Notifier.Information(T("房间编号已存在，修改失败！"));
                return Json(new {redirectUrl = ""}); //返回一个空url，页面上根据这个值弹出提示信息
            }
            //1、基础数据修改
            part.ApartmentId = model.ApartmentId;
            part.BuildingId = model.BuildingId;
            part.OwnerId = model.OwnerId;
            record.HouseNumber = model.HouseNumber;
            //record.HouseStatus = (HouseStatusOption) model.HouseStatusId;//不能改变房间状态
            record.BuildingArea = model.BuildingArea;
            record.InsideArea = model.InsideArea;
            record.PoolArea = model.PoolArea;
            part.OfficerId = model.OfficerId;
            //2、两个items的 删除， 增加，修改，一定要先删除，再添加或修改
            //ChargeItems
            IEnumerable<HouseChargeItemRecord> deletingChargeItems =
                record.ChargeItems.Where(x => model.ChargeItemEntities.All(c => c.Id != x.Id));
            deletingChargeItems.ToList().ForEach(x => record.ChargeItems.Remove(x)); //先在数据库中删除 这些Id与原来不一样的记录

            IEnumerable<HouseChargeItemRecord> updatingChargeItems =
                record.ChargeItems.Where(x => model.ChargeItemEntities.Any(c => c.Id == x.Id));
            //相同的id，就是要更新的记录
            updatingChargeItems.ToList().ForEach(x =>
            {
                HouseChargeItemEntity entity = model.ChargeItemEntities.First(c => c.Id == x.Id);
                x.ChargeItemSetting =
                    Services.ContentManager.Get<ChargeItemSettingPart>(entity.ChargeItemSettingId).Record;
                x.ItemCategory = entity.ItemCategory;
                x.Name = entity.ChargeItemSettingName;
                x.CalculationMethod = entity.CalculationMethod;
                x.MeterTypeId = entity.MeterType;
                x.UnitPrice = entity.UnitPrice;
                x.MeteringMode = entity.MeteringMode;
                x.Money = entity.Money;
                x.CustomFormula = entity.CustomFormula;
                x.ChargingPeriod = entity.ChargingPeriod;
                x.ExpenserOption = entity.ExpenserOptionId;
                x.BeginDate = Convert.ToDateTime(entity.BeginDate);
                if (!string.IsNullOrEmpty(entity.EndDate))
                {
                    x.EndDate = Convert.ToDateTime(entity.EndDate);
                }
                x.Description = entity.Description;
                x.House = record;
                x.Id = entity.Id;
            });

            IEnumerable<HouseChargeItemEntity> addingChargeItems = model.ChargeItemEntities.Where(x => x.Id == 0);
                //Id为0的列就是要添加的记录,默认为0，最好在编辑页面上给它0
            addingChargeItems.ForEach(it =>
            {
                var entity = new HouseChargeItemRecord();
                entity.ChargeItemSetting =
                    Services.ContentManager.Get<ChargeItemSettingPart>(it.ChargeItemSettingId).Record;
                entity.ItemCategory = it.ItemCategory;
                entity.Name = it.ChargeItemSettingName;
                entity.CalculationMethod = it.CalculationMethod;
                entity.MeterTypeId = it.MeterType;
                entity.UnitPrice = it.UnitPrice;
                entity.MeteringMode = it.MeteringMode;
                entity.Money = it.Money;
                entity.CustomFormula = it.CustomFormula;
                entity.ChargingPeriod = it.ChargingPeriod;
                entity.ExpenserOption = it.ExpenserOptionId;
                entity.BeginDate = Convert.ToDateTime(it.BeginDate);
                if (!string.IsNullOrEmpty(it.EndDate))
                {
                    entity.EndDate = Convert.ToDateTime(it.EndDate);
                }
                entity.Description = it.Description;
                entity.House = record;
                record.ChargeItems.Add(entity);
            });


            //MeterItems
            IEnumerable<HouseMeterRecord> deletingMeterItems =
                record.MeterTypeItems.Where(m => model.MeterTypeItemEntities.All(c => c.Id != m.Id));
            deletingMeterItems.ToList().ForEach(m => record.MeterTypeItems.Remove(m)); //先在数据库中删除 这些Id与原来不符合的数据

            IEnumerable<HouseMeterRecord> updatingMeterItems =
                record.MeterTypeItems.Where(m => model.MeterTypeItemEntities.Any(c => c.Id == m.Id));
            //相同的id，就是要修改的记录
            updatingMeterItems.ToList().ForEach(m =>
            {
                HouseMeterEntity entity = model.MeterTypeItemEntities.First(c => c.Id == m.Id);
                m.House = Services.ContentManager.Get<HousePart>(entity.HouseId).Record;
                m.MeterType = Services.ContentManager.Get<MeterTypePart>(entity.MeterTypeItemId).Record;
                m.MeterNumber = entity.MeterNumber;
                m.Ratio = entity.Ratio;
            });

            IEnumerable<HouseMeterEntity> addingMeterItems = model.MeterTypeItemEntities.Where(m => m.Id == 0);
                //Id为0的列就是要添加的记录,默认为0，我在页面上给它0
            addingMeterItems.ForEach(it =>
            {
                var meterItem = new HouseMeterRecord();
                meterItem.MeterType = Services.ContentManager.Get<MeterTypePart>(it.MeterTypeItemId).Record;
                meterItem.MeterNumber = it.MeterNumber;
                meterItem.Ratio = it.Ratio;
                meterItem.House = record;
                record.MeterTypeItems.Add(meterItem);
            });

            Services.ContentManager.Publish(contentItem);
            Services.Notifier.Information(T("更新房间信息成功！"));
            string redirect = Url.Action("Index");
            return Json(new {redirectUrl = redirect});
        }

        [HttpPost]
        public ActionResult Delete(List<int> selectedIds)
        {
            if (
                !Services.Authorizer.Authorize(StandardPermissions.Delete, Services.ContentManager.New("House"),
                    T("没有房间删除权限！")))
                return new HttpUnauthorizedResult();
            try
            {
                IEnumerable<ContentItem> items = Services.ContentManager.Query().ForContentItems(selectedIds).List();
                foreach (ContentItem item in items)
                {
                    Services.ContentManager.Remove(item);
                }
                if (Services.Notifier.List().All(x => x.Type != NotifyType.Error))
                {
                    Services.Notifier.Information(T("删除成功！"));
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK, T("删除成功！").Text);
            }
            catch
            {
                Services.Notifier.Error(T("删除失败!"));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, T("删除失败！").Text);
            }
        }

        [HttpPost]
        public ActionResult RoomOfiicerDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc")
        {
            IContentQuery<UserPart, UserPartRecord> query = Services.ContentManager.Query<UserPart, UserPartRecord>();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.UserName.Contains(term));
            }
            int totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1)*pageSize, pageSize)
                .Select(item => new
                {
                    id = item.Record.ContentItemRecord.Id,
                    text = item.UserName
                }).ToList();
            return Json(new {records, total = totalRecords});
        }

        [HttpPost]
        public ActionResult GetChargeItemDetailById(int id)
        {
            IEnumerable<ChargeItemSettingPart> query =
                Services.ContentManager.Query<ChargeItemSettingPart, ChargeItemSettingPartRecord>()
                    .Where(x => x.Id == id && x.ItemCategory != ItemCategoryOption.临时性收费项目)
                    .List();
            var records = query.Select(item => new
            {
                itemCategory = item.ItemCategory.ToString(),
                chargingPeriod = item.ChargingPeriod.ToString(),
                calculationMethod = item.CalculationMethod.ToString(),
                unitPrice = item.UnitPrice,
                meteringMode = item.MeteringMode.ToString(),
                money = item.Money,
                customFormula = item.CustomFormula
            }).ToList();
            if (records.Count == 0)
            {
                return HttpNotFound();
            }
            return Json(records);
        }
    }
}