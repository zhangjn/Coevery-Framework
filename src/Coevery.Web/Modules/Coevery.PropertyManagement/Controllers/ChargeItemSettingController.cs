using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Coevery;
using Coevery.Security;
using Coevery.UI.Notify;
using Coevery.ContentManagement;
using Coevery.Data;
using Coevery.Themes;
using Coevery.Localization;
using Coevery.PropertyManagement.Models;
using Coevery.PropertyManagement.ViewModels;

namespace Coevery.PropertyManagement.Controllers
{
    [Themed]
    public class ChargeItemSettingController : Controller, IUpdateModel
    {
        private readonly ITransactionManager _transactionManager;

        public ChargeItemSettingController(ICoeveryServices services,
            ITransactionManager transactionManager)
        {
            Services = services;
            _transactionManager = transactionManager;
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
            var contentItem = Services.ContentManager.New("ChargeItemSetting");
            if (!Services.Authorizer.Authorize(StandardPermissions.View, contentItem, T("没有收费项目标准查看权限")))
                return new HttpUnauthorizedResult();
            contentItem.Weld(new ChargeItemSettingPart());
            var model = Services.ContentManager.BuildDisplay(contentItem, "List");
            return View("List", model);
        }

        [HttpPost]
        public ActionResult List(int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc")
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.View, Services.ContentManager.New("ChargeItemSetting"), T("没有收费项目标准查看权限")))
                return new HttpUnauthorizedResult();
            var query = Services.ContentManager.Query<ChargeItemSettingPart, ChargeItemSettingPartRecord>();
            var totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1)*pageSize, pageSize)
                .Select(item => new ChargeItemSettingListViewModel
                {
                    Id = item.Record.ContentItemRecord.Id,
                    VersionId = item.Record.Id,
                    Name = item.Record.Name,
                    ItemCategory = item.Record.ItemCategory.ToString(),
                    CalculationMethod = item.Record.CalculationMethod.ToString(),
                    UnitPrice = item.Record.UnitPrice,
                    MeteringMode = item.Record.MeteringMode.ToString(),
                    Money = item.Record.Money,
                    CustomFormula = item.Record.CustomFormula,
                    Remark = item.Record.Remark,
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
        public ActionResult MeterTypeDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc")
        {
            var query = Services.ContentManager.Query<MeterTypePart, MeterTypePartRecord>();
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
            var contentItem = Services.ContentManager.New("ChargeItemSetting");
            if (!Services.Authorizer.Authorize(StandardPermissions.Create, contentItem, T("没有收费项目标准创建权限")))
                return new HttpUnauthorizedResult();
            var model = Services.ContentManager.BuildEditor(contentItem, "Create");
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(string returnUrl)
        {
            var contentItem = Services.ContentManager.New<ChargeItemSettingPart>("ChargeItemSetting");
            if (!Services.Authorizer.Authorize(StandardPermissions.Create, contentItem, T("没有收费项目标准创建权限")))
                return new HttpUnauthorizedResult();
            dynamic model = Services.ContentManager.UpdateEditor(contentItem, this, "Create");
            var name = contentItem.Name;
            var count = Services.ContentManager.Query<ChargeItemSettingPart, ChargeItemSettingPartRecord>().Where(x => x.Name == name).Count();
            if (count > 0)
            {
                Services.Notifier.Error(T("已经存在相同的收费项目标准名称！"));
                return View("Create", (object)model);
            }
            if (!ModelState.IsValid)
            {
                return View("Create", (object) model);
            }
            Services.ContentManager.Create(contentItem, VersionOptions.Published);
            Services.Notifier.Information(T("创建收费标准成功！"));
            return RedirectToAction("List");
        }

        public ActionResult Edit(int id)
        {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null)
            {
                return HttpNotFound();
            }
            if (!Services.Authorizer.Authorize(StandardPermissions.Edit, contentItem, T("没有收费项目标准编辑权限")))
                return new HttpUnauthorizedResult();
            dynamic model = Services.ContentManager.BuildEditor(contentItem, "Edit");
            return View((object) model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int id, string returnUrl)
        {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            if (contentItem == null)
            {
                return HttpNotFound();
            }
            if (!Services.Authorizer.Authorize(StandardPermissions.Edit, contentItem, T("没有收费项目标准编辑权限")))
                return new HttpUnauthorizedResult();
            dynamic model = Services.ContentManager.UpdateEditor(contentItem, this, "Edit");
            if (!ModelState.IsValid)
            {
                _transactionManager.Cancel();
                return View("Edit", (object) model);
            }
            Services.Notifier.Information(T("更新收费标准信息成功"));
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Delete(List<int> selectedIds)
        {
            if (!Services.Authorizer.Authorize(StandardPermissions.Delete, Services.ContentManager.New("ChargeItemSetting"), T("没有收费项目编辑权限")))
                return new HttpUnauthorizedResult();
            try
            {
                var items = Services.ContentManager.Query().ForContentItems(selectedIds).List();
                foreach (var item in items)
                {
                    Services.ContentManager.Remove(item);
                }
                if (Services.Notifier.List().All(x => x.Type != NotifyType.Error))
                {
                    Services.Notifier.Information(T("删除成功！"));
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                Services.Notifier.Error(T("删除失败!"));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult EditDelayCharge(int id)
        {
            var part = Services.ContentManager.Get<ChargeItemSettingPart>(id);
            ChargeItemSettingPartRecord chargeItemSetting = part.Record;
            var model = new ChargeItemDelayChargeEditViewModel()
            {
                Id = chargeItemSetting.Id,
                ChargeItemName = chargeItemSetting.Name,
                DelayChargeDays = chargeItemSetting.DelayChargeDays,
                DelayChargeRatio = chargeItemSetting.DelayChargeRatio,
                DelayChargeCalculationMethod = chargeItemSetting.DelayChargeCalculationMethod ?? DelayChargeCalculationMethodOption.手动计算滞纳金,
                StartCalculationDatetime = chargeItemSetting.StartCalculationDatetime ?? StartCalculationDatetimeOption.欠费开始时间,
                ChargingPeriodPrecision = chargeItemSetting.ChargingPeriodPrecision ?? ChargingPeriodPrecisionOption.按周期计算,
                DefaultChargingPeriod = chargeItemSetting.DefaultChargingPeriod ?? DefaultChargingPeriodOption.当期收当期
            };
            return View(model);
        }

        [HttpPost, ActionName("EditDelayCharge")]
        public ActionResult EditDelayChargePost(ChargeItemDelayChargeEditViewModel model)
        {
            var contentItem = Services.ContentManager.Get(model.Id);
            var part = contentItem.As<ChargeItemSettingPart>();
            var record = part.Record;
            record.DelayChargeDays = model.DelayChargeDays;
            record.DelayChargeRatio = model.DelayChargeRatio;
            record.DelayChargeCalculationMethod = model.DelayChargeCalculationMethod;
            record.StartCalculationDatetime = model.StartCalculationDatetime;
            record.ChargingPeriodPrecision = model.ChargingPeriodPrecision;
            record.DefaultChargingPeriod = model.DefaultChargingPeriod;
            Services.ContentManager.Publish(contentItem);
            Services.Notifier.Information(T("更新滞纳金信息成功！"));
            return RedirectToAction("List");
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