using System.Web.Mvc;
using System.Web.Routing;
using Coevery.Localization;
using Coevery.PropertyManagement.Security;
using Coevery.Security;
using Coevery.UI.Navigation;

namespace Coevery.PropertyManagement {
    public class FrontMenu : INavigationProvider {
        public FrontMenu(ICoeveryServices services) {
            T = NullLocalizer.Instance;
            CoeveryServices = services;
        }

        public Localizer T { get; set; }
        public ICoeveryServices CoeveryServices { get; set; }

        public string MenuName {
            get { return "FrontMenu"; }
        }

        public void GetNavigation(NavigationBuilder builder) {
            var requestContext = new RequestContext(CoeveryServices.WorkContext.HttpContext, new RouteData());
            var urlhelper = new UrlHelper(requestContext);

            builder.Add(T("基础信息"), "1", menu => menu
                .IdHint("BasicInformation")
                .Add(T("仪表种类管理"), "1", item => item.Url(urlhelper.Action("Index", "MeterType", new {area = "Coevery.PropertyManagement"}))
                    .Permission(StandardPermissions.View).Content(CoeveryServices.ContentManager.New("MeterType")), new[] {"icon-MeterType"})
                .Add(T("客户管理"), "2", item => item.Url(urlhelper.Action("Index", "Customer", new {area = "Coevery.PropertyManagement"}))
                    .Permission(StandardPermissions.View).Content(CoeveryServices.ContentManager.New("Customer")), new[] {"icon-Customer"})
                .Add(T("楼盘管理"), "3", item => item.Url(urlhelper.Action("Index", "Apartment", new {area = "Coevery.PropertyManagement"}))
                    .Permission(StandardPermissions.View).Content(CoeveryServices.ContentManager.New("Apartment")), new[] {"icon-Apartment"})
                .Add(T("楼宇管理"), "4", item => item.Url(urlhelper.Action("Index", "Building", new {area = "Coevery.PropertyManagement"}))
                    .Permission(StandardPermissions.View).Content(CoeveryServices.ContentManager.New("Building")), new[] {"icon-Building"})
                .Add(T("房间管理"), "5", item => item.Url(urlhelper.Action("Index", "House", new {area = "Coevery.PropertyManagement"}))
                    .Permission(StandardPermissions.View).Content(CoeveryServices.ContentManager.New("House")), new[] {"icon-House"})
                .Add(T("部门管理"), "6", item => item.Url(urlhelper.Action("Index", "Department", new { area = "Coevery.PropertyManagement" }))
                    .Permission(StandardPermissions.View).Content(CoeveryServices.ContentManager.New("Department")), new[] { "icon-Department" })
                );
            builder.Add(T("收费管理"), "2", menu => menu
                .IdHint("ChargeInformation")
                .Add(T("收费标准管理"), "1", item => item.Url(urlhelper.Action("Index", "ChargeItemSetting", new {area = "Coevery.PropertyManagement"}))
                    .Permission(StandardPermissions.View).Content(CoeveryServices.ContentManager.New("ChargeItemSetting")), new[] {"icon-ChargeItemSetting"})
                .Add(T("抄表数据录入"), "2", item => item.Url(urlhelper.Action("List", "HouseMeterReading", new {area = "Coevery.PropertyManagement"}))
                    .Permission(StandardPermissions.View).Content(CoeveryServices.ContentManager.New("HouseMeterReading")), new[] {"icon-MeterType"})
                .Add(T("未出账单"), "4", item => item.Url(urlhelper.Action("Index", "Bill", new {area = "Coevery.PropertyManagement"}))
                    .Permission(Permissions.BillManage), new[] {"icon-Bill"})
                .Add(T("客户缴费"), "3", item => item.Url(urlhelper.Action("Index", "Payment", new {area = "Coevery.PropertyManagement"}))
                    .Permission(StandardPermissions.View).Content(CoeveryServices.ContentManager.New("Payment")), new[] {"icon-Payment"})
                .Add(T("客户预缴费"), "5", item => item.Url(urlhelper.Action("List", "AdvancePayment", new { area = "Coevery.PropertyManagement" }))
                    .Permission(StandardPermissions.View).Content(CoeveryServices.ContentManager.New("Payment")), new[] { "icon-Payment" })
                );
            builder.Add(T("合同管理"), "2", menu => menu
                .IdHint("Contractformation")
                .Add(T("合同列表"), "3", item => item.Url(urlhelper.Action("Index", "Contract", new {area = "Coevery.PropertyManagement"}))
                    .Permission(StandardPermissions.View).Content(CoeveryServices.ContentManager.New("Contract")), new[] {"icon-Contract"})
                );
            builder.Add(T("物料管理"), "3", menu => menu
               .IdHint("Materialformation")
               .Add(T("供应商"), "1", item => item.Url(urlhelper.Action("Index", "Supplier", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Material" })
               .Add(T("材料"), "2", item => item.Url(urlhelper.Action("Index", "Material", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Material" })
               .Add(T("入库"), "3", item => item.Url(urlhelper.Action("List", "Inventory", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Material" })
               .Add(T("出库"), "4", item => item.Url(urlhelper.Action("List", "StockOut", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Material" })
               );
            builder.Add(T("维修管理"), "4", menu => menu
             .IdHint("Serviceformation")
             .Add(T("维修"), "1", item => item.Url(urlhelper.Action("Index", "Service", new { area = "Coevery.PropertyManagement" }))
                 , new[] { "icon-Service" })
             );
            builder.Add(T("报表"), "5", menu => menu
               .IdHint("Reports")
               .Add(T("收款明细表"), "1", item => item.Url(urlhelper.Action("Index", "Receipt", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Report" })
               .Add(T("账单明细表"), "2", item => item.Url(urlhelper.Action("Index", "BillReport", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Report" })
               .Add(T("欠费统计明细表"), "3", item => item.Url(urlhelper.Action("Index", "OwingReport", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Report" })
               .Add(T("物业经营明细表"), "4", item => item.Url(urlhelper.Action("Index", "HouseStatusReport", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Report" })
               .Add(T("收费对账单"), "5", item => item.Url(urlhelper.Action("Index", "StatementReport", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Report" })
               .Add(T("采购入库明细表"), "6", item => item.Url(urlhelper.Action("StockInDetailList", "StockReport", new { area = "Coevery.PropertyManagement" }))
                  , new[] { "icon-Report" })
               .Add(T("领用出库明细表"), "7", item => item.Url(urlhelper.Action("StockOutDetailList", "StockReport", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Report" })
               .Add(T("库存汇总表"), "8", item => item.Url(urlhelper.Action("StockSumarryList", "StockReport", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Report" })
               .Add(T("以往应收分摊表"), "9", item => item.Url(urlhelper.Action("Index", "BillAllocationReport", new { area = "Coevery.PropertyManagement" }))
                   , new[] { "icon-Report" })
               );
        }
    }
}