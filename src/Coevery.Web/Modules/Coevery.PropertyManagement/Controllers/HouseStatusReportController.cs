using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
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
using Coevery.UI.Notify;
using Coevery.Users.Models;
using Coevery.PropertyManagement.Extensions;
using NPOI.HSSF.UserModel;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class HouseStatusReportController : Controller, IUpdateModel
    {
        private readonly IRepository<HousePartRecord> _houseRepository;
        private readonly IRepository<ContractHouseRecord> _contractHouseRepository;
        private readonly IRepository<ApartmentPartRecord> _apartmentRepository;

        private readonly IReportServices _reportServices;

        public HouseStatusReportController(ICoeveryServices services,
            IRepository<HousePartRecord> houseRepository,
            IRepository<ContractHouseRecord> contractHouseRepository,
            IReportServices reportServices, 
            IRepository<ApartmentPartRecord> apartmentRepository)
        {
            Services = services;
            _houseRepository = houseRepository;
            _reportServices = reportServices;
            _apartmentRepository = apartmentRepository;
            _contractHouseRepository = contractHouseRepository;
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

            #region init dropdown

            List<SelectListItem> apartments = _apartmentRepository.Table
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            apartments.Insert(0, new SelectListItem());
            ViewBag.Apartments = apartments;

            List<SelectListItem> customers = Services.ContentManager.Query<CustomerPart, CustomerPartRecord>().List()
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
            customers.Insert(0, new SelectListItem());
            ViewBag.Customers = customers;

            List<SelectListItem> officers = Services.ContentManager.Query<UserPart, UserPartRecord>().List()
                .Select(c => new SelectListItem { Text = c.UserName, Value = c.Id.ToString() }).ToList();
            officers.Insert(0, new SelectListItem());
            ViewBag.Officers = officers;

            List<SelectListItem> houseStatus = Enum.GetNames(typeof (HouseStatusOption)).Select(name => new SelectListItem() {
                Text = name, Value = name
            }).ToList();
            houseStatus.Insert(0, new SelectListItem());
            ViewBag.HouseStatus = houseStatus;

            #endregion

            return View("List");
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc") {
            if (!Services.Authorizer.Authorize(Permissions.BillManage, T("没有账单查看权限")))
                return new HttpUnauthorizedResult();

            List<HousePartRecord> records = QueryHouseRecord(input);
            var list = _reportServices.GenerateHouseStatusReportViewModel(records).ToList();

            int totalRecords = list.Count;
            list = list.OrderBy(sortBy, sortOrder).Skip((page - 1)*pageSize)
                       .Take(pageSize).ToList();
            var result = new {
                page,
                totalPages = Math.Ceiling((double) totalRecords/pageSize),
                totalRecords,
                rows = list
            };
            return Json(result);
        }

        [HttpPost]
        public ActionResult HouseReportSummary(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc") {
            if (!Services.Authorizer.Authorize(Permissions.BillManage, T("没有账单查看权限")))
                return new HttpUnauthorizedResult();

            List<HousePartRecord> records = QueryHouseRecord(input);
            var list = _reportServices.GenerateHouseSummaryReportViewModel(records).ToList();

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

        public ActionResult ExportExcel(FormCollection input, string gridColumns, string gridSummaryColumns) {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("物业经营状态明细表");

            #region main grid

            var query = QueryHouseRecord(input).ToList();
            var mainList = _reportServices.GenerateHouseStatusReportViewModel(query).ToList();

            _reportServices.FillExcel(gridColumns, mainList, sheet);

            #endregion

            #region Summary Grid

            var summaryList = _reportServices.GenerateHouseSummaryReportViewModel(query.ToList()).ToList();

            _reportServices.FillExcel(gridSummaryColumns, summaryList, sheet);

            #endregion

            //Save the Excel spreadsheet to a MemoryStream and return it to the client
            using (var exportData = new MemoryStream()) {
                workbook.Write(exportData);
                string saveAsFileName = string.Format("{0}-{1:d}.xls", "物业经营状态明细表", DateTime.Now).Replace("/", "-");
                return File(exportData.ToArray(), "application/vnd.ms-excel", saveAsFileName);
            }
        }

        private List<HousePartRecord> QueryHouseRecord(FormCollection input) {
            IQueryable<HousePartRecord> query = _houseRepository.Table;

            //这里加过滤器的代码
            var options = new BillFilterOptions();
            UpdateModel(options, "Filter", input);

            #region 楼盘，楼宇，房号查询

            if (!string.IsNullOrEmpty(options.ApartmentId))
            {
                int[] apartmentIdArray = Array.ConvertAll(options.ApartmentId.Split(','), s => Int32.Parse(s.Trim()));
                query = query.Where(x => apartmentIdArray.Contains(x.Apartment.Id));
            }
            else
            {
                options.BuildingId = string.Empty;
                options.HouseNumber = string.Empty;
            }
            if (!string.IsNullOrEmpty(options.BuildingId))
            {
                int[] buildingIdArray = Array.ConvertAll(options.BuildingId.Split(','), s => Int32.Parse(s.Trim()));
                query = query.Where(x => buildingIdArray.Contains(x.Building.Id));
            }
            else
            {
                options.HouseNumber = string.Empty;
            }
            if (!string.IsNullOrEmpty(options.HouseNumber))
            {
                int[] houseNumberArray = Array.ConvertAll(options.HouseNumber.Split(','), s => Int32.Parse(s.Trim()));
                query = query.Where(x => houseNumberArray.Contains(x.Id));
            }

            #endregion

            #region 业主，租户，专管员查询

            if (options.OwnerId.HasValue) {
                query = query.Where(b => b.Owner.Id == options.OwnerId);
            }
            if (options.RenterId.HasValue)
            {
                query = from q in query
                        join c in _contractHouseRepository.Table on q.Id equals c.House.Id
                        where c.Contract.Renter.Id == options.RenterId
                        select q;
            }
            if (options.OfficerId.HasValue) {
                query = query.Where(b => b.Officer.Id == options.OfficerId);
            }

            #endregion

            #region 合同号，日期查询
            if (!string.IsNullOrEmpty(options.ContractNumber)) {
                query = from h in query
                        join c in _contractHouseRepository.Table on h.Id equals c.House.Id
                        where c.Contract.Number == options.ContractNumber
                        select h;
            }

            if (!string.IsNullOrEmpty(options.HouseStatus)) {
                HouseStatusOption status;
                Enum.TryParse(options.HouseStatus, out status);
                query = query.Where(x => x.HouseStatus == status);
            }
            #endregion 

            return query.ToList();
        }
    }
}