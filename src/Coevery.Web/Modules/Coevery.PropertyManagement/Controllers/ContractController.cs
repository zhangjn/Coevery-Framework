using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Coevery.Security;
using Coevery.UI.Notify;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Themes;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;
using NHibernate.Util;


namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class ContractController : Controller, IUpdateModel
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<ContractHouseRecord> _contractHouseRepository;
        private readonly IRepository<ApartmentPartRecord> _apartmentRepository;

        public ContractController(ICoeveryServices services,
            ITransactionManager transactionManager,
            IRepository<ContractHouseRecord> contractHouseRepository, 
            IRepository<ApartmentPartRecord> apartmentRepository)
        {
            Services = services;
            _transactionManager = transactionManager;
            _contractHouseRepository = contractHouseRepository;
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
            var contentItem = Services.ContentManager.New("Contract");
            if (!Services.Authorizer.Authorize(StandardPermissions.View, contentItem, T("没有合同查看权限")))
                return new HttpUnauthorizedResult();
            contentItem.Weld(new ContractPart());
            var model = Services.ContentManager.BuildDisplay(contentItem, "List");
            return View("List", model);
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc")
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.View, Services.ContentManager.New("Contract"), T("没有合同查看权限")))
                return new HttpUnauthorizedResult();
            var query = Services.ContentManager.Query<ContractPart, ContractPartRecord>();
            var options = new ContractFilterOptions();
            UpdateModel(options, "Filter", input);
            if (options.RenterId.HasValue)
            {
                query = query.Where(c => c.Renter.Id == options.RenterId);
            }
            if (!string.IsNullOrEmpty(options.ContractName))
            {
                query = query.Where(c => c.Name.Contains(options.ContractName));
            }
            if (!string.IsNullOrEmpty(options.ContractNumber))
            {
                query = query.Where(c => c.Number.Contains(options.ContractNumber));
            }
            var totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1)*pageSize, pageSize)
                .Select(item => new ContractListViewModel
                {
                    Id = item.Record.ContentItemRecord.Id,
                    VersionId = item.Record.Id,
                    Number = item.Record.Number,
                    Name = item.Record.Name,
                    RenterId = item.Record.Renter.Id,
                    RenterName = item.Record.Renter.Name,
                    ContractStatus = item.Record.ContractStatus.ToString(),
                    BeginDate = SpecifyDateTimeKind(item.Record.BeginDate),
                    EndDate = SpecifyDateTimeKind(item.Record.EndDate),
                    Description = item.Record.Description,
                    HouseStatus = item.Record.HouseStatus??HouseStatusOption.空置
                }).ToList();

            var result = new
            {
                page,
                totalPages = Math.Ceiling((double)totalRecords/pageSize),
                totalRecords,
                rows = records
            };
            return Json(result);
        }

        private DateTime? SpecifyDateTimeKind(DateTime? utcDateTime)
        {
            if (utcDateTime != null)
                return DateTime.SpecifyKind(utcDateTime.Value, DateTimeKind.Utc);
            return null;
        }

        [HttpPost]
        public ActionResult OwnerIdDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc")
        {
            var query = Services.ContentManager.Query<CustomerPart, CustomerPartRecord>();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.Name.Contains(term));
            }
            var totalRecords = query.Count();
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
        public ActionResult RenterIdDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc")
        {
            var query = Services.ContentManager.Query<CustomerPart, CustomerPartRecord>();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.Name.Contains(term));
            }
            var totalRecords = query.Count();
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
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null)
            {
                return HttpNotFound();
            }

            dynamic model = Services.ContentManager.BuildDisplay(contentItem, "Detail");
            return View((object) model);
        }

        public ActionResult Create()
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.Create, Services.ContentManager.New("Contract"), T("没有合同创建权限")))
                return new HttpUnauthorizedResult();
            var model = new ContractCreateViewModel();
            var customers = Services.ContentManager.Query<CustomerPart, CustomerPartRecord>().List()
                            .Select(c => new SelectListItem 
                            {
                                Text = c.Name, 
                                Value = c.Id.ToString()
                            }).ToList();

            var apartments = _apartmentRepository.Table
                            .Select(it => new SelectListItem
                            {
                                Text = it.Name,
                                Value = it.Id.ToString()
                            }).ToList();

            var chargeItems = Services.ContentManager.Query<ChargeItemSettingPart, ChargeItemSettingPartRecord>().List()
                              .Where(x => x.ItemCategory != ItemCategoryOption.临时性收费项目)
                              .Select(c => new SelectListItem 
                              { 
                                  Text = c.Name, 
                                  Value = c.Id.ToString()
                              }).ToList();

            var meterTypes = Services.ContentManager.Query<MeterTypePart, MeterTypePartRecord>().List()
                             .Select(c => new SelectListItem
                             {
                                 Text = c.Name, 
                                 Value = c.Id.ToString()
                             }).ToList();

            customers.Insert(0,new SelectListItem{Text="",Value=""});
            apartments.Insert(0, new SelectListItem { Text = "", Value = "" });
            chargeItems.Insert(0, new SelectListItem { Text = "", Value = "" });
            meterTypes.Insert(0, new SelectListItem { Text = "", Value = "" });
            model.RenterListItems = customers;
            model.ApartmentListItems = apartments;
            model.ChargeListItems = chargeItems;
            model.MeterTypeListItems = meterTypes;

            return View(model);
        }

        //获取收费标准后台方法
        [HttpPost]
        public ActionResult GetChargeSetting(int chargeItemSettingId) {
            var item = Services.ContentManager.Get<ChargeItemSettingPart>(chargeItemSettingId);
            if (item == null||item.ItemCategory==ItemCategoryOption.临时性收费项目)
            {
                return HttpNotFound();
            }
            var result = new ContractChargeItemEntity();
            if (item.CalculationMethod.HasValue)
            {
                result.CalculationMethodId = (int)item.CalculationMethod;
                result.CalculationMethod = item.CalculationMethod.ToString();
            }
            if (item.MeteringMode.HasValue)
            {
                result.MeteringModeId = (int)item.MeteringMode;
                result.MeteringMode = item.MeteringMode.ToString();
            }
            if (item.ChargingPeriod.HasValue)
            {
                result.ChargingPeriodId = (int)item.ChargingPeriod;
                result.ChargingPeriod = item.ChargingPeriod.ToString();
            }
            if (item.ItemCategory.HasValue)
            {
                result.ItemCategoryId = (int)item.ItemCategory;
                result.ItemCategory = item.ItemCategory.ToString();
            }
            if (item.DelayChargeCalculationMethod.HasValue)
            {
                result.DelayChargeCalculationMethodId = (int)item.DelayChargeCalculationMethod;
                result.DelayChargeCalculationMethod = item.DelayChargeCalculationMethod.ToString();
            }
            if (item.StartCalculationDatetime.HasValue)
            {
                result.StartCalculationDatetimeId = (int)item.StartCalculationDatetime;
                result.StartCalculationDatetime = item.StartCalculationDatetime.ToString();
            }
            if (item.ChargingPeriodPrecision.HasValue)
            {
                result.ChargingPeriodPrecisionId = (int)item.ChargingPeriodPrecision;
                result.ChargingPeriodPrecision = item.ChargingPeriodPrecision.ToString();
            }
            if (item.DefaultChargingPeriod.HasValue)
            {
                result.DefaultChargingPeriodId = (int)item.DefaultChargingPeriod;
                result.DefaultChargingPeriod = item.DefaultChargingPeriod.ToString();
            }
            result.ChargeItemSettingId = item.Id;
            result.MeterTypeId = item.MeterType;
            if (item.MeterType.HasValue)
            {
                result.MeterTypeName = Services.ContentManager.Get<MeterTypePart>(item.MeterType.Value).Name;
            }
            result.ChargeItemSettingName = item.Name;
            result.UnitPrice = item.UnitPrice;
            result.CustomFormula = item.CustomFormula;
            result.Money = item.Money;
            result.DelayChargeDays = item.DelayChargeDays;
            result.DelayChargeRatio = item.DelayChargeRatio;

            return Json(result);
        }


       

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(ContractCreateViewModel model)
        {
            var contentItem = Services.ContentManager.New("Contract");
            if (!Services.Authorizer.Authorize(StandardPermissions.Create, contentItem, T("没有合同创建权限")))
                return new HttpUnauthorizedResult();
            var contracts = Services.ContentManager.Query<ContractPart, ContractPartRecord>().List().ToList();
            if (contracts.Any(c=> c.Name.Trim() == model.Name.Trim()|| c.Number.Trim() == model.Number.Trim()))
            {
                Services.Notifier.Information(T("合同名称或编号不能重复！"));
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            var part = contentItem.As<ContractPart>();
            var record = part.Record;
            part.Name = model.Name;
            part.Number = model.Number;
            part.ContractStatus = ContractStatusOption.签订;
            part.Renter = Services.ContentManager.Get<CustomerPart>(model.RenterId).Record;
            part.RenterId = model.RenterId;
            part.BeginDate = model.BeginDate.Value;
            part.EndDate = model.EndDate.Value;
            part.Description = model.Description;
            part.HouseStatus = (HouseStatusOption)model.HouseStatusId;

            model.ChargeItemEntities.ForEach(it =>
            {
                var item = new ContractChargeItemRecord
                {
                    ExpenserOption = it.ExpenserOptionId,
                    BeginDate = DateTime.Parse(it.ChargeBeginDate),
                    UnitPrice = it.UnitPrice,
                    Money = it.Money,
                    CustomFormula = it.CustomFormula,
                    MeterTypeId = it.MeterTypeId
                };

                var chargeItemSetting = Services.ContentManager.Get<ChargeItemSettingPart>(it.ChargeItemSettingId).Record;
                chargeItemSetting.CopyPropertiesTo(item, "Id");
                item.ChargeItemSetting = chargeItemSetting;
               
                if (it.ItemCategoryId.HasValue)
                {
                    item.ItemCategory = (ItemCategoryOption)it.ItemCategoryId.Value;
                }
                if (it.ChargingPeriodId.HasValue)
                {
                    item.ChargingPeriod = (ChargingPeriodOption)it.ChargingPeriodId.Value;
                }
                if (it.CalculationMethodId.HasValue)
                {
                    item.CalculationMethod = (CalculationMethodOption)it.CalculationMethodId;
                }
                if (it.MeteringModeId.HasValue)
                {
                    item.MeteringMode = (MeteringModeOption)it.MeteringModeId;
                }
                if (!string.IsNullOrEmpty(it.ChargeEndDate)) {
                    item.EndDate = DateTime.Parse(it.ChargeEndDate);
                }
                item.Description = it.ChargeDescription;
                item.Contract = record;
                record.ChargeItems.Add(item);
            });

            model.HouseEntities.ForEach(it => {
                var item = new ContractHouseRecord {
                    House = Services.ContentManager.Get<HousePart>(it.HouseId).Record, 
                    Contract = record
                };
                record.Houses.Add(item);
            });

            Services.ContentManager.Create(contentItem, VersionOptions.Draft);

            Services.ContentManager.Publish(contentItem);
            Services.Notifier.Information(T("创建合同成功！"));
            var redirect = Url.Action("Index");
            return Json(new {redirectUrl = redirect});
        }

        public ActionResult See(int id)
        {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null)
            {
                return HttpNotFound();
            }
            if (!Services.Authorizer.Authorize(StandardPermissions.View, contentItem, T("没有合同查看权限")))
                return new HttpUnauthorizedResult();
            var record = contentItem.As<ContractPart>().Record;
            var model = new ContractEditViewModel();

            model.Id = id;//这么重要的Id
            model.Name = record.Name;
            model.Number = record.Number;
            model.RenterId = record.Renter.Id;
            model.RenterName = record.Renter.Name;
            model.BeginDate = record.BeginDate.ToString("yyyy/MM/dd");
            model.EndDate = record.EndDate.ToString("yyyy/MM/dd");
            model.ContractStatusId = record.ContractStatus.HasValue ? (int)record.ContractStatus.Value : 0;
            model.Description = record.Description;
            model.HouseStatus = record.HouseStatus.Value;

            record.ChargeItems.ForEach(it =>
            {
                var item=new ContractChargeItemEntity();
                //item.ChargeItemName = it.Name;
                item.ChargeItemSettingId = it.ChargeItemSetting.Id;
                item.ChargeItemSettingName = it.ChargeItemSetting.Name;
                item.ChargeBeginDate = it.BeginDate.ToString("MM/dd/yyyy");
                item.ChargeEndDate = it.EndDate.HasValue?it.EndDate.Value.ToString("MM/dd/yyyy"):null;
                item.ChargeDescription = it.Description;
                item.ExpenserOptionId = it.ExpenserOption;
                item.ExpenserName = ((ExpenserOption)item.ExpenserOptionId).ToString();


                if (it.CalculationMethod.HasValue)
                {
                    item.CalculationMethodId = Convert.ToInt32(it.CalculationMethod);
                    item.CalculationMethod = it.CalculationMethod.ToString();
                }
                if (it.ItemCategory.HasValue)
                {
                    item.ItemCategoryId = Convert.ToInt32(it.ItemCategory);
                    item.ItemCategory = it.ItemCategory.ToString();
                }
                if (it.ChargingPeriod.HasValue)
                {
                    item.ChargingPeriodId = Convert.ToInt32(it.ChargingPeriod);
                    item.ChargingPeriod = it.ChargingPeriod.ToString();
                }
                if (it.MeteringMode.HasValue)
                {
                    item.MeteringModeId = Convert.ToInt32(it.MeteringMode);
                    item.MeteringMode = it.MeteringMode.ToString();
                }
                if (it.MeterTypeId.HasValue)
                {
                    item.MeterTypeId = it.MeterTypeId;
                    item.MeterTypeName = Services.ContentManager.Get<MeterTypePart>(it.MeterTypeId.Value).Name;
                }
                item.UnitPrice = it.UnitPrice;
                item.Money = it.Money;
                item.CustomFormula = it.CustomFormula;

                item.ContractId = record.Id;
                item.Id = it.Id;
                model.ChargeItemEntities.Add(item);
            });

            record.Houses.ForEach(it =>
            {
                var item = new ContractHouseEntity
                {
                    Id = it.Id,
                    ContractId = it.Contract.Id,
                    HouseId = it.House.Id,
                    HouseNumber = it.House.HouseNumber,
                    ApartmentName = it.House.Apartment.Name,
                    BuildingName = it.House.Building.Name,
                    BuildingArea = it.House.BuildingArea ?? 0,
                    InsideArea = it.House.InsideArea ?? 0,
                    PoolArea = it.House.PoolArea ?? 0,
                    OwnerName = it.House.Owner.Name
                };
                model.HouseEntities.Add(item);
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult StopContract(int id) {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null)
            {
                return HttpNotFound();
            }
            if (!Services.Authorizer.Authorize(StandardPermissions.Edit, contentItem, T("没有合同终止权限")))
                return new HttpUnauthorizedResult();
            var record = contentItem.As<ContractPart>().Record;
            record.EndDate = DateTime.Now;
            record.ContractStatus=ContractStatusOption.终止;
            Services.ContentManager.Publish(contentItem);
            Services.Notifier.Information(T("合同已终止"));
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        public ActionResult Edit(int id)
        {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null)
            {
                return HttpNotFound();
            }
            if (!Services.Authorizer.Authorize(StandardPermissions.Edit, contentItem, T("没有合同编辑权限")))
                return new HttpUnauthorizedResult();
            var record = contentItem.As<ContractPart>().Record;
            var model = new ContractEditViewModel();

            model.Id = id;//这么重要的Id
            model.Name = record.Name;
            model.Number = record.Number;
            model.RenterId = record.Renter.Id;
            model.RenterName = record.Renter.Name;
            model.BeginDate = record.BeginDate.ToString("yyyy/MM/dd");
            model.EndDate = record.EndDate.ToString("yyyy/MM/dd");
            model.Description = record.Description;
            model.ContractStatus = record.ContractStatus;
            model.HouseStatus = record.HouseStatus ?? HouseStatusOption.空置;
            model.HouseStatusId = (int)model.HouseStatus;

            record.ChargeItems.ForEach(it =>
            {
                var item=new ContractChargeItemEntity();
                item.ChargeItemName = it.Name;
                item.ChargeItemSettingId = it.ChargeItemSetting.Id;
                item.ChargeItemSettingName = it.ChargeItemSetting.Name;
                item.ChargeBeginDate = it.BeginDate.ToString("MM/dd/yyyy");
                item.ChargeEndDate = it.EndDate.HasValue?it.EndDate.Value.ToString("MM/dd/yyyy"):null;
                item.ChargeDescription = it.Description;
                item.ExpenserOptionId = it.ExpenserOption;
                item.ExpenserName = ((ExpenserOption)item.ExpenserOptionId).ToString();


                if (it.CalculationMethod.HasValue)
                {
                    item.CalculationMethodId = Convert.ToInt32(it.CalculationMethod);
                    item.CalculationMethod = it.CalculationMethod.ToString();
                }
                if (it.ItemCategory.HasValue)
                {
                    item.ItemCategoryId = Convert.ToInt32(it.ItemCategory);
                    item.ItemCategory = it.ItemCategory.ToString();
                }
                if (it.ChargingPeriod.HasValue)
                {
                    item.ChargingPeriodId = Convert.ToInt32(it.ChargingPeriod);
                    item.ChargingPeriod = it.ChargingPeriod.ToString();
                }
                if (it.MeteringMode.HasValue)
                {
                    item.MeteringModeId = Convert.ToInt32(it.MeteringMode);
                    item.MeteringMode = it.MeteringMode.ToString();
                }
                if (it.MeterTypeId.HasValue)
                {
                    item.MeterTypeId = it.MeterTypeId;
                    item.MeterTypeName = Services.ContentManager.Get<MeterTypePart>(it.MeterTypeId.Value).Name;
                }

                item.UnitPrice = it.UnitPrice;
                item.Money = it.Money;
                item.CustomFormula = it.CustomFormula;

                item.ContractId = record.Id;
                item.Id = it.Id;
                model.ChargeItemEntities.Add(item);
            });

            record.Houses.ForEach(it => {
                var item = new ContractHouseEntity {
                    Id = it.Id,
                    ContractId = it.Contract.Id,
                    HouseId = it.House.Id,
                    HouseNumber = it.House.HouseNumber,
                    ApartmentName = it.House.Apartment.Name,
                    BuildingName = it.House.Building.Name,
                    BuildingArea = it.House.BuildingArea ?? 0,
                    InsideArea = it.House.InsideArea ?? 0,
                    PoolArea = it.House.PoolArea ?? 0,
                    OwnerName = it.House.Owner.Name
                };
                model.HouseEntities.Add(item);
            });
            
            var customers = Services.ContentManager.Query<CustomerPart, CustomerPartRecord>().List()
                            .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                            .ToList();

            var chargeItems = Services.ContentManager.Query<ChargeItemSettingPart, ChargeItemSettingPartRecord>().List()
                              .Where(x => x.ItemCategory != ItemCategoryOption.临时性收费项目)
                              .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                              .ToList();

            var apartments = _apartmentRepository.Table
                             .Select(it => new SelectListItem
                             {
                                 Text = it.Name,
                                 Value = it.Id.ToString()
                             }).ToList();
            var meterTypes = Services.ContentManager.Query<MeterTypePart, MeterTypePartRecord>().List()
                             .Select(c => new SelectListItem
                             {
                                 Text = c.Name,
                                 Value = c.Id.ToString()
                             }).ToList();
            apartments.Insert(0, new SelectListItem { Text = "", Value = "" });
            chargeItems.Insert(0,new SelectListItem{Text = "",Value = ""});
            meterTypes.Insert(0, new SelectListItem { Text = "", Value = "" });
            model.RenterListItems = customers;
            model.ApartmentListItems = apartments;
            model.ChargeListItems = chargeItems;
            model.MeterTypeListItems = meterTypes;

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int id,ContractEditViewModel model) 
        {
			var contentItem = Services.ContentManager.Get(id,VersionOptions.Latest);
            if (contentItem == null) {
                return HttpNotFound();
            }
            if (!Services.Authorizer.Authorize(StandardPermissions.Edit, contentItem, T("没有合同编辑权限")))
                return new HttpUnauthorizedResult();
            var contracts = Services.ContentManager.Query<ContractPart, ContractPartRecord>().Where(c=>c.Id!=id).List().ToList();
            if (contracts.Any(c => c.Name.Trim() == model.Name.Trim() || c.Number.Trim() == model.Number.Trim()))
            {
                Services.Notifier.Information(T("合同名称或编号不能重复！"));
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            var part = contentItem.As<ContractPart>();
            var record = part.Record;

            //更新合同基础数据
            part.Name = model.Name;
            part.Number = model.Number;
            part.Renter = Services.ContentManager.Get<CustomerPart>(model.RenterId).Record;
            part.RenterId = model.RenterId;
            part.BeginDate = Convert.ToDateTime(model.BeginDate);
            part.EndDate = Convert.ToDateTime(model.EndDate);
            part.Description = model.Description;
            part.HouseStatus = (HouseStatusOption)model.HouseStatusId;

            //更新合同houses，删，改，增
            var deletingHouseItems = record.Houses.Where(x => model.HouseEntities.All(c => c.Id != x.Id));
            deletingHouseItems.ToList().ForEach(x => record.Houses.Remove(x));

            var updatingHouseItems = record.Houses.Where(x => model.HouseEntities.Any(c => c.Id == x.Id));
            updatingHouseItems.ToList().ForEach(x => {
                var entity = model.HouseEntities.First(c => c.Id == x.Id);
                x.House = Services.ContentManager.Get<HousePart>(entity.HouseId).Record;
                x.Contract = record;
                x.Id = entity.Id;
            });

            var addingHouseItems = model.HouseEntities.Where(x => x.Id == 0);
            addingHouseItems.ForEach(it => {
                var item = new ContractHouseRecord {
                    House = Services.ContentManager.Get<HousePart>(it.HouseId).Record,
                    Contract = record
                };
                record.Houses.Add(item);
            });


            //更新合同chargeItems 增删改
            var deletingChargeItems = record.ChargeItems.Where(x => model.ChargeItemEntities.All(c => c.Id != x.Id));
            deletingChargeItems.ToList().ForEach(x => record.ChargeItems.Remove(x));

            var updatingChargeItems = record.ChargeItems.Where(x => model.ChargeItemEntities.Any(c => c.Id == x.Id));
            updatingChargeItems.ToList().ForEach(x =>
            {
                var entity = model.ChargeItemEntities.First(c => c.Id == x.Id);
                x.ChargeItemSetting = Services.ContentManager.Get<ChargeItemSettingPart>(entity.ChargeItemSettingId).Record;
                x.ChargeItemSetting.CopyPropertiesTo(x, "Id");
                x.ExpenserOption = entity.ExpenserOptionId;
                x.BeginDate = Convert.ToDateTime(entity.ChargeBeginDate);
                if (!string.IsNullOrEmpty(entity.ChargeEndDate))
                {
                    x.EndDate = Convert.ToDateTime(entity.ChargeEndDate);
                }
                x.Description = entity.ChargeDescription;

                x.UnitPrice = entity.UnitPrice;
                x.Money = entity.Money;
                x.CustomFormula = entity.CustomFormula;
                if (entity.ItemCategoryId.HasValue)
                {
                    x.ItemCategory = (ItemCategoryOption)entity.ItemCategoryId.Value;
                }
                if (entity.ChargingPeriodId.HasValue)
                {
                    x.ChargingPeriod = (ChargingPeriodOption)entity.ChargingPeriodId.Value;
                }
                if (entity.CalculationMethodId.HasValue)
                {
                    x.CalculationMethod = (CalculationMethodOption)entity.CalculationMethodId;
                }
                if (entity.MeteringModeId.HasValue)
                {
                    x.MeteringMode = (MeteringModeOption)entity.MeteringModeId;
                }
                x.MeterTypeId = entity.MeterTypeId;

                x.Contract = record;
                x.Id = entity.Id;
            });

            var addingChargeItems = model.ChargeItemEntities.Where(x => x.Id == 0);
            addingChargeItems.ForEach(it =>
            {
                var item = new ContractChargeItemRecord();
                item.ChargeItemSetting = Services.ContentManager.Get<ChargeItemSettingPart>(it.ChargeItemSettingId).Record;
                item.ChargeItemSetting.CopyPropertiesTo(item, "Id");
                item.ExpenserOption = it.ExpenserOptionId;
                item.BeginDate = DateTime.Parse(it.ChargeBeginDate);

                item.UnitPrice = it.UnitPrice;
                item.Money = it.Money;
                item.CustomFormula = it.CustomFormula;
                if (it.ItemCategoryId.HasValue)
                {
                    item.ItemCategory = (ItemCategoryOption) it.ItemCategoryId.Value;
                }
                if (it.ChargingPeriodId.HasValue)
                {
                    item.ChargingPeriod = (ChargingPeriodOption)it.ChargingPeriodId.Value;
                }
                if (it.CalculationMethodId.HasValue)
                {
                    item.CalculationMethod = (CalculationMethodOption)it.CalculationMethodId;
                }
                if (it.MeteringModeId.HasValue)
                {
                    item.MeteringMode = (MeteringModeOption) it.MeteringModeId;
                }
                item.MeterTypeId = it.MeterTypeId;
                if (!string.IsNullOrEmpty(it.ChargeEndDate))
                {
                    item.EndDate = DateTime.Parse(it.ChargeEndDate);
                }
                item.Description = it.ChargeDescription;
                item.Contract = record;
                record.ChargeItems.Add(item);
            });

            Services.ContentManager.Publish(contentItem);
            Services.Notifier.Information(T("更新合同成功！"));
            var redirect = Url.Action("Index");
            return Json(new { redirectUrl = redirect });
        }


        public ActionResult Change(int id) {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null)
            {
                return HttpNotFound();
            }
            if (!Services.Authorizer.Authorize(StandardPermissions.Edit, contentItem, T("没有合同变更权限")))
                return new HttpUnauthorizedResult();
            var record = contentItem.As<ContractPart>().Record;
            var model = new ContractEditViewModel();

            model.Id = id;//这么重要的Id
            model.Name = record.Name;
            model.Number = record.Number;
            model.RenterId = record.Renter.Id;
            model.RenterName = record.Renter.Name;
            model.BeginDate = DateTime.Now.Date.ToString("yyyy/MM/dd");
            model.HouseStatus = record.HouseStatus.Value;
            model.HouseStatusId = (int)model.HouseStatus;

            if (DateTime.Now<record.EndDate)
            {
                model.EndDate = record.EndDate.ToString("yyyy/MM/dd");
            }
            
            model.Description = record.Description;
            model.ContractStatus = ContractStatusOption.合同变更;

            record.ChargeItems.ForEach(it =>
            {
                var item = new ContractChargeItemEntity();
                item.ChargeItemName = it.Name;
                item.ChargeItemSettingId = it.ChargeItemSetting.Id;
                item.ChargeItemSettingName = it.ChargeItemSetting.Name;
                item.ChargeBeginDate = it.BeginDate.ToString("MM/dd/yyyy");
                item.ChargeEndDate = it.EndDate.HasValue ? it.EndDate.Value.ToString("MM/dd/yyyy") : null;
                item.ChargeDescription = it.Description;
                item.ExpenserOptionId = it.ExpenserOption;
                item.ExpenserName = ((ExpenserOption)item.ExpenserOptionId).ToString();


                if (it.CalculationMethod.HasValue)
                {
                    item.CalculationMethodId = Convert.ToInt32(it.CalculationMethod);
                    item.CalculationMethod = it.CalculationMethod.ToString();
                }
                if (it.ItemCategory.HasValue)
                {
                    item.ItemCategoryId = Convert.ToInt32(it.ItemCategory);
                    item.ItemCategory = it.ItemCategory.ToString();
                }
                if (it.ChargingPeriod.HasValue)
                {
                    item.ChargingPeriodId = Convert.ToInt32(it.ChargingPeriod);
                    item.ChargingPeriod = it.ChargingPeriod.ToString();
                }
                if (it.MeteringMode.HasValue)
                {
                    item.MeteringModeId = Convert.ToInt32(it.MeteringMode);
                    item.MeteringMode = it.MeteringMode.ToString();
                }
                if (it.MeterTypeId.HasValue)
                {
                    item.MeterTypeId = it.MeterTypeId;
                    item.MeterTypeName = Services.ContentManager.Get<MeterTypePart>(it.MeterTypeId.Value).Name;
                }

                item.UnitPrice = it.UnitPrice;
                item.Money = it.Money;
                item.CustomFormula = it.CustomFormula;

                //还有很多枚举没有赋值,还没用到这些
                item.ContractId = record.Id;
                item.Id = it.Id;
                model.ChargeItemEntities.Add(item);
            });

            record.Houses.ForEach(it =>
            {
                var item = new ContractHouseEntity
                {
                    Id = it.Id,
                    ContractId = it.Contract.Id,
                    HouseId = it.House.Id,
                    HouseNumber = it.House.HouseNumber,
                    ApartmentName = it.House.Apartment.Name,
                    BuildingName = it.House.Building.Name,
                    BuildingArea = it.House.BuildingArea ?? 0,
                    InsideArea = it.House.InsideArea ?? 0,
                    PoolArea = it.House.PoolArea ?? 0,
                    OwnerName = it.House.Owner.Name
                };
                model.HouseEntities.Add(item);
            });

            var customers = Services.ContentManager.Query<CustomerPart, CustomerPartRecord>().List()
                            .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                            .ToList();

            var chargeItems = Services.ContentManager.Query<ChargeItemSettingPart, ChargeItemSettingPartRecord>().List()
                              .Where(x => x.ItemCategory != ItemCategoryOption.临时性收费项目)
                              .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                              .ToList();

            var apartments = _apartmentRepository.Table
                             .Select(it => new SelectListItem
                             {
                                 Text = it.Name,
                                 Value = it.Id.ToString()
                             }).ToList();
            var meterTypes = Services.ContentManager.Query<MeterTypePart, MeterTypePartRecord>().List()
                             .Select(c => new SelectListItem
                             {
                                 Text = c.Name,
                                 Value = c.Id.ToString()
                             }).ToList();
            apartments.Insert(0, new SelectListItem { Text = "", Value = "" });
            chargeItems.Insert(0, new SelectListItem { Text = "", Value = "" });
            meterTypes.Insert(0, new SelectListItem { Text = "", Value = "" });
            model.RenterListItems = customers;
            model.ApartmentListItems = apartments;
            model.ChargeListItems = chargeItems;
            model.MeterTypeListItems = meterTypes;

            return View(model);
        }
        [HttpPost,ActionName("Change")]
        public ActionResult ChangePost(ContractEditViewModel model) {
            var contentItem = Services.ContentManager.New("Contract");
            if (!Services.Authorizer.Authorize(StandardPermissions.Create, contentItem, T("没有合同变更权限")))
                return new HttpUnauthorizedResult();
            var contracts = Services.ContentManager.Query<ContractPart, ContractPartRecord>().List().ToList();
            if (contracts.Any(c => c.Name.Trim() == model.Name.Trim() || c.Number.Trim() == model.Number.Trim()))
            {
                Services.Notifier.Information(T("合同名称或编号不能重复！"));
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            //先终止之前的合同
            var endPart = Services.ContentManager.Get<ContractPart>(model.Id);
            endPart.EndDate = DateTime.Now.Date;//时间变了也会自动终止合同
            endPart.ContractStatus = ContractStatusOption.终止;
            Services.ContentManager.Publish(endPart.ContentItem);

            //再新建变更的合同
            var part = contentItem.As<ContractPart>();
            var record = part.Record;
            part.Name = model.Name;
            part.Number = model.Number;
            part.ContractStatus = ContractStatusOption.签订;
            part.Renter = Services.ContentManager.Get<CustomerPart>(model.RenterId).Record;
            part.RenterId = model.RenterId;
            part.BeginDate = Convert.ToDateTime(model.BeginDate);
            part.EndDate = Convert.ToDateTime(model.EndDate);
            part.Description = model.Description;
            part.HouseStatus = (HouseStatusOption)model.HouseStatusId;

            model.ChargeItemEntities.ForEach(it =>
            {
                var item = new ContractChargeItemRecord();
                item.ChargeItemSetting = Services.ContentManager.Get<ChargeItemSettingPart>(it.ChargeItemSettingId).Record;
                item.ChargeItemSetting.CopyPropertiesTo(item, "Id");
                item.ExpenserOption = it.ExpenserOptionId;
                item.BeginDate = DateTime.Parse(it.ChargeBeginDate);
                item.UnitPrice = it.UnitPrice;
                item.Money = it.Money;
                item.CustomFormula = it.CustomFormula;
                item.MeterTypeId = it.MeterTypeId;
                
                if (it.ItemCategoryId.HasValue)
                {
                    item.ItemCategory = (ItemCategoryOption)it.ItemCategoryId.Value;
                }
                if (it.ChargingPeriodId.HasValue)
                {
                    item.ChargingPeriod = (ChargingPeriodOption)it.ChargingPeriodId.Value;
                }
                if (it.CalculationMethodId.HasValue)
                {
                    item.CalculationMethod = (CalculationMethodOption)it.CalculationMethodId;
                }
                if (it.MeteringModeId.HasValue)
                {
                    item.MeteringMode = (MeteringModeOption)it.MeteringModeId;
                }
                if (!string.IsNullOrEmpty(it.ChargeEndDate))
                {
                    item.EndDate = DateTime.Parse(it.ChargeEndDate);
                }
                item.Description = it.ChargeDescription;
                item.Contract = record;
                record.ChargeItems.Add(item);
            });

            model.HouseEntities.ForEach(it =>
            {
                var item = new ContractHouseRecord
                {
                    House = Services.ContentManager.Get<HousePart>(it.HouseId).Record,
                    Contract = record
                };
                record.Houses.Add(item);
            });

            Services.ContentManager.Create(contentItem, VersionOptions.Draft);

            Services.ContentManager.Publish(contentItem);
            Services.Notifier.Information(T("变更合同成功！"));
            var redirect = Url.Action("Index");
            return Json(new { redirectUrl = redirect });
        }

        [HttpPost]
        public ActionResult GetHouseDetail(int? contractId, int? apartmentId, int? buildingId, string houseNumber,int page = 1, int pageSize = 10)
        {
            var query = Services.ContentManager.Query<HousePart, HousePartRecord>();
            //过滤器
            if (apartmentId.HasValue)
            {
                query = query.Where(h => h.Apartment.Id == apartmentId);
            }
            if (buildingId.HasValue)
            {
                query = query.Where(h => h.Building.Id == buildingId);
            }
            if (!string.IsNullOrEmpty(houseNumber))
            {
                query = query.Where(h => h.HouseNumber.Contains(houseNumber));
            }
            //找符合条件所有房间ids
            var houseIds = query.List().Select(m => m.Id).ToList();
            //去除 所有房间里面 签了合同且合同没有终止的房间ids  
            var deleteHouseIds = _contractHouseRepository.Table
                                 .Where(c => houseIds.Contains(c.House.Id) && c.Contract.ContractStatus!=ContractStatusOption.终止)
                                 .Select(x => x.House.Id).ToList();
            houseIds=houseIds.Except(deleteHouseIds).ToList();
            //编辑页面，如果有合同Id,那么还要加上当前合同已经签的房间ids
            if (contractId.HasValue)
            {
                var addHouseIds = _contractHouseRepository.Table
                                 .Where(c => c.Contract.Id==contractId)
                                 .Select(x => x.House.Id).ToList();
                houseIds=houseIds.Union(addHouseIds).Distinct().ToList();
            }
            var list = query.List()
                .Where(h => houseIds.Contains(h.Id)).Select(x => new ContractHouseEntity
            {
                HouseId = x.Id,
                HouseNumber = x.HouseNumber,
                InsideArea = x.InsideArea,
                BuildingArea = x.BuildingArea,
                PoolArea = x.PoolArea,
                OwnerName = x.Owner != null? x.Owner.Name : null,
                ApartmentName = x.Apartment.Name,
                BuildingName = x.Building.Name
            }).ToList();
            
            var totalRecords = list.Count;
            list = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var result = new
            {
                page,
                totalPages = Math.Ceiling((double)totalRecords/pageSize),
                totalRecords,
                rows = list
            };
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(List<int> selectedIds)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.Delete, Services.ContentManager.New("Contract"), T("没有合同删除权限！")))
                return new HttpUnauthorizedResult();
            try
            {
                var items = Services.ContentManager.Query().ForContentItems(selectedIds).List();
                foreach (var item in items) {
                    Services.ContentManager.Remove(item);
                }
                if (Services.Notifier.List().All(x => x.Type != NotifyType.Error))
                {
                    Services.Notifier.Information(T("删除成功！"));
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch {
			    Services.Notifier.Error(T("删除失败！"));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, T("删除失败！").Text);
            }
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}

