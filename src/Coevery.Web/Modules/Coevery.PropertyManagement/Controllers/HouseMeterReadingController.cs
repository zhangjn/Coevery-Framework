using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Logging;
using Coevery.Security;
using Coevery.Themes;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;
using Coevery.UI.Notify;
using NHibernate.Hql.Ast.ANTLR;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class HouseMeterReadingController : Controller, IUpdateModel
    {
        private readonly IRepository<HouseMeterRecord> _houseMeterRepository;
        private readonly IRepository<HouseMeterReadingPartRecord> _houseMeterReadingRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<ApartmentPartRecord> _apartmentRepository;
        private readonly IRepository<BuildingPartRecord> _buildingRepository;

        public HouseMeterReadingController(ICoeveryServices services,
            ITransactionManager transactionManager,
            IRepository<HouseMeterRecord> houseMeterRepository,
            IRepository<HouseMeterReadingPartRecord> houseMeterReadingRepository,
            IRepository<ApartmentPartRecord> apartmentRepository,
            IRepository<BuildingPartRecord> buildingRepository)
        {
            Services = services;
            _transactionManager = transactionManager;
            _houseMeterRepository = houseMeterRepository;
            _houseMeterReadingRepository = houseMeterReadingRepository;
            _apartmentRepository = apartmentRepository;
            _buildingRepository = buildingRepository;
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
            var contentItem = Services.ContentManager.New("HouseMeterReading");
            if (!Services.Authorizer.Authorize(StandardPermissions.View, contentItem, T("没有房间仪表读数查看权限！")))
                return new HttpUnauthorizedResult();
            contentItem.Weld(new HouseMeterReadingPart());
            var model = Services.ContentManager.BuildDisplay(contentItem, "List");

            var apartments = _apartmentRepository.Table
                            .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
            apartments.Insert(0, new SelectListItem());
            ViewBag.apartments = apartments;
            var buildings = Services.ContentManager.Query<BuildingPart, BuildingPartRecord>().List()
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
            buildings.Insert(0, new SelectListItem());
            ViewBag.buildings = buildings;
            var houses = Services.ContentManager.Query<HousePart, HousePartRecord>().List()
                .Select(c => new SelectListItem { Text = c.HouseNumber, Value = c.Id.ToString() }).ToList();
            houses.Insert(0, new SelectListItem());
            ViewBag.houses = houses;
            return View("List", model);
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null,
            string sortOrder = "asc")
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.View, Services.ContentManager.New("HouseMeterReading"), T("没有房间仪表读数查看权限！")))
                return new HttpUnauthorizedResult();
            var options = new HouseMeterReadingFilterOptions();
            UpdateModel(options, "FilterOptions", input);
            var houseMeterReadingRecords = _houseMeterRepository.Table;
            if (options.ApartmentId != null) {
                houseMeterReadingRecords =
                    houseMeterReadingRecords.Where(x => x.House.Apartment.Id == options.ApartmentId);
            }
            else {
                options.BuildingId = null;
                options.HouseNumber = null;
            }

            if (options.BuildingId != null) {
                houseMeterReadingRecords =
                    houseMeterReadingRecords.Where(x => x.House.Building.Id == options.BuildingId);
            }
            else {
                options.HouseNumber = null;
            }

            if (!string.IsNullOrEmpty(options.HouseNumber))
            {
                houseMeterReadingRecords =
                    houseMeterReadingRecords.Where(x => x.House.HouseNumber == options.HouseNumber);
            }
            if (!options.DateFrom.HasValue) //处理最开始filter没数据 只有取当月
            {
                options.DateFrom = DateTime.Now;
            }
            var query = from r in houseMeterReadingRecords
                select new
                {
                    HouseMeterId = r.Id,
                    HouseNumber = r.House.HouseNumber,
                    HouseOwnerName = r.House.Owner.Name,
                    MeterTypeName = r.MeterType.Name,
                    Reading =
                        r.MeterReadings.FirstOrDefault(
                            x => x.Year == options.DateFrom.Value.Year && x.Month == options.DateFrom.Value.Month)
                };

            var totalRecords = query.Count();
            var records = query
                .Skip((page - 1)*pageSize)
                .Take(pageSize)
                .ToList()
                .Select(x => new HouseMeterReadingListViewModel()
                {
                    HouseMeterId = x.HouseMeterId,
                    HouseNumber = x.HouseNumber,
                    HouseOwnerName = x.HouseOwnerName,
                    MeterTypeName = x.MeterTypeName,
                    MeterData = x.Reading != null ? x.Reading.MeterData : null,
                    Remarks = x.Reading != null ? x.Reading.Remarks : null,
                    Status = x.Reading != null ? x.Reading.Status.ToString() : null,
                    HouseMeterTypeItemId = x.Reading != null ? x.Reading.HouseMeter.Id : (int?) null,
                    Amount = x.Reading != null ? x.Reading.Amount : null
                });


            var result = new
            {
                page,
                totalPages = Math.Ceiling((double)totalRecords/pageSize),
                totalRecords,
                rows = records
            };
            return Json(result);
        }

        public ActionResult GetBuildingItems(int apartmentItemId)
        {
            var query = Services.ContentManager.Query<BuildingPart, BuildingPartRecord>().List()
                .Where(x => x.Apartment == apartmentItemId)
                .Select(item => new
                {
                    Value = item.Id,
                    text = item.Name
                });
            return Json(query);
        }

        public ActionResult GetHouseItems(int apartmentItemId, int buildingItemId)
        {
            var query = Services.ContentManager.Query<HousePart, HousePartRecord>().List()
                .Where(x => x.Apartment.Id == apartmentItemId && x.Building.Id == buildingItemId)
                .Select(item => new
                {
                    Value = item.HouseNumber,
                    text = item.HouseNumber
                });
            return Json(query);
        }

        public double? GetMeterReadingAmount(int year, int month, double meterdata, int houseMeterId)
        {
            double? amount;
            var query = Services.ContentManager.Query<HouseMeterReadingPart, HouseMeterReadingPartRecord>().List()
                .Where(x => x.Year == year && x.HouseMeterId == houseMeterId)
                .OrderBy(x => x.Month).ToList();
            if (query.Count > 0)
            {
                var lastHouseMeterRecord = query.Last();
                amount = meterdata - lastHouseMeterRecord.MeterData;
            }
            else
            {
                amount = meterdata;
            }
            return amount;
        }


        [HttpPost]
        public ActionResult CreateHoseMeterReadingData(int year, int month, string meterdata, int houseMeterId)
        {
            var query = Services.ContentManager.Query<HouseMeterReadingPart, HouseMeterReadingPartRecord>().List()
                .Where(x => x.Year == year && x.Month == month && x.HouseMeterId == houseMeterId).ToList();
            if (query.Count > 0)
            {
                var houseMeterRecord = query.Last();
                var contentItem = Services.ContentManager.Get(houseMeterRecord.Id);
                var part = contentItem.As<HouseMeterReadingPart>();
                var record = part.Record;
                record.MeterData = double.Parse(meterdata);
                Services.ContentManager.Publish(contentItem);
                return Json(new {Value = 2});
            }
            else
            {
                var contentItem = Services.ContentManager.New("HouseMeterReading");
                var part = contentItem.As<HouseMeterReadingPart>();
                var record = part.Record;
                record.Year = year;
                record.Month = month;
                record.MeterData = double.Parse(meterdata);
                record.Status = StatusOption.已录入;
                part.HouseMeterId = houseMeterId;
                Services.ContentManager.Create(contentItem, VersionOptions.Draft);
                Services.ContentManager.Publish(contentItem);
                return Json(new {Value = 1});
            }
        }

        [HttpPost]
        public ActionResult Import()
        {
            var model = new ImportViewModel();
            var result = new List<ImportListModel>();
            var file = Request.Files["importFile"];
            var houseMeterReadingRecords = _houseMeterRepository.Table;
            var apartmentRecords = _apartmentRepository.Table;
            var buildingRecords = _buildingRepository.Table;
            if (file == null){
              
                Services.Notifier.Information(T("导入失败！"));
                return List();
            }

            var workbook = new HSSFWorkbook(file.InputStream);
            var sheet = workbook[0];
            bool isFirst = true;
            try
            {
                foreach (HSSFRow row in sheet)
                {
                    if (row.PhysicalNumberOfCells < 10)
                    {
                        break;
                    }
                    if (isFirst)
                    {
                        isFirst = false;
                        if (row.Cells[0].StringCellValue != "年份"
                            || row.Cells[1].StringCellValue != "月份"
                            || row.Cells[2].StringCellValue != "楼盘"
                            || row.Cells[3].StringCellValue != "楼宇"
                            || row.Cells[4].StringCellValue != "房间号"
                            || row.Cells[5].StringCellValue != "业主姓名"
                            || row.Cells[6].StringCellValue != "租户姓名"
                            || row.Cells[7].StringCellValue != "专管业务员"
                            || row.Cells[8].StringCellValue != "合同号"
                            || row.Cells[9].StringCellValue != "仪表种类"
                            || row.Cells[10].StringCellValue != "仪表读数"
                            )
                        {
                            result.Add(new ImportListModel()
                            {
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
                    var year = Convert.ToInt32(row.Cells[0].NumericCellValue);
                    var month = Convert.ToInt32(row.Cells[1].NumericCellValue);
                    var apartmentName = row.Cells[2].StringCellValue;
                    var buildingName = row.Cells[3].StringCellValue;
                    var houseNumber = row.Cells[4].StringCellValue;
                    var meterTypeName = row.Cells[9].StringCellValue;
                    var meterReading = row.Cells[10].NumericCellValue.ToString();
                    var apartmentIdArray = apartmentRecords.Where(x => x.Name == apartmentName)
                        .Select(x => x.Id)
                        .ToArray();
                    if (apartmentIdArray.Length > 0)
                        apartmentId = apartmentIdArray[0];
                    else
                    {
                        result.Add(new ImportListModel()
                        {
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
                    else
                    {
                        result.Add(new ImportListModel()
                        {
                            RowId = row.RowNum,
                            FieldName = "楼宇--" + buildingName,
                            ErrorMessage = "数据库中不存在此楼宇"
                        });
                        continue;
                    }

                    var houseMeterIdArray = houseMeterReadingRecords
                        .Where(x => x.House.Apartment.Id == apartmentId
                                    && x.House.Building.Id == buildingId
                                    && x.House.HouseNumber == houseNumber
                                    && x.MeterType.Name == meterTypeName)
                        .Select(x => x.Id)
                        .ToArray();

                    if (houseMeterIdArray.Length > 0)
                    {
                        var houseMeterId = houseMeterIdArray[0];
                        CreateHoseMeterReadingData(year, month, meterReading, houseMeterId);
                    }
                    else
                    {
                        result.Add(new ImportListModel()
                        {
                            RowId = row.RowNum,
                            FieldName = "房间号|房间仪表--" + houseNumber + "|" + meterTypeName,
                            ErrorMessage = "数据库中不存在此房间号或者房间仪表"
                        });
                       
                    }
                }
                model.List = result;
            }
            catch (Exception ex)
            {
                result.Add(new ImportListModel()
                {
                    RowId = 0,
                    ErrorMessage =ex.Message
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
            var path = Server.MapPath("~/App_Data/抄表数据录入模板.xls");
            var name = Path.GetFileName(path);
            return File(path, "application/zip-x-compressed", name);
        }

        private DateTime? SpecifyDateTimeKind(DateTime? utcDateTime)
        {
            if (utcDateTime != null)
                return DateTime.SpecifyKind(utcDateTime.Value, DateTimeKind.Utc);
            return null;
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