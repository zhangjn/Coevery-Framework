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
    public class ServiceController : Controller, IUpdateModel
    {
         private readonly ITransactionManager _transactionManager;
         private readonly IVoucherNumberService _voucherNumberService;
         private readonly IRepository<ApartmentPartRecord> _apartmentRepository;
         private readonly IRepository<InventoryChangeRecord> _inventoryChangeRepository;
         private readonly IRepository<InventoryRecord> _inventoryRepository;
         private readonly IInventoryService _inventoryService;
         public ServiceController(ICoeveryServices services,
            ITransactionManager transactionManager, 
             IVoucherNumberService voucherNumberService, 
             IRepository<InventoryChangeRecord> inventoryChangeRepository, 
             IRepository<InventoryRecord> inventoryRepository, 
             IInventoryService inventoryService, 
             IRepository<ApartmentPartRecord> apartmentRepository)
        {
            Services = services;
            _transactionManager = transactionManager;
             _voucherNumberService = voucherNumberService;
             _inventoryChangeRepository = inventoryChangeRepository;
             _inventoryRepository = inventoryRepository;
             _inventoryService = inventoryService;
             _apartmentRepository = apartmentRepository;
             T = NullLocalizer.Instance;
        }
        public ICoeveryServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult Create()
        {
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
            return View();
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(ServiceCreateViewModel model)
        {
            var houses = Services.ContentManager.Query<HousePart, HousePartRecord>()
                .Where(x =>x.Apartment.Id == model.ApartmentId
                        && x.Building.Id == model.BuildingId 
                        &&x.Id == model.HouseId)
                .List().Select(h => h.Id).ToArray();
            ContentItem contentItem = Services.ContentManager.New("Service");
            var part = contentItem.As<ServicePart>();
            ServicePartRecord record = part.Record;
            record.House = Services.ContentManager.Get<HousePart>(houses[0]).Record;
            record.Owner = Services.ContentManager.Get<CustomerPart>(model.OwnerId).Record;
            record.Mobile = model.Mobile;
            record.FaultDescription = model.FaultDescription;
            record.ReceivedDate = model.ReceivedDate;
            Services.ContentManager.Create(contentItem, VersionOptions.Draft); 
            Services.ContentManager.Publish(contentItem);
            var redirect = Url.Action("Index");
            return Json(new { redirectUrl = redirect });
        }

        public ActionResult PickUpMaterial(int id)
        {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            var part = contentItem.As<ServicePart>();
            ServicePartRecord record = part.Record;
            var voucherNo = _voucherNumberService.DispayGenerateNumber();
            var createView = new ServiceVoucherCreateViewModel
            {
                id = id,
                Address = record.House.Apartment.Name + "/" + record.House.Building.Name + "/" +
                          record.House.HouseNumber,
                FaultDescription = record.FaultDescription,
                OwnerName = record.Owner.Name,
                ReceivedDate = record.ReceivedDate.ToString("d"),
                Mobile = record.Mobile,
                VoucherNo = voucherNo
            };
            return View(createView);
        }
        [HttpPost]
        public ActionResult ServiceVoucherCreate(ServiceVoucherCreateViewModel model)
        {
            var voucherNo = _voucherNumberService.GenerateNumber();
            var details = model.InventoryDetailEntities;
            if(details.Count>0)
            { 
            int[] ids = details.Select(x => x.MaterialId).ToArray();
            Dictionary<int, InventoryRecord> inventoryRecords = _inventoryRepository.Table
                .Where(x => ids.Contains(x.MaterialId))
                .Distinct()
                .ToDictionary(x => x.MaterialId);
            #region 检查库存数量
            var isValid = !(from detail in details let inventory = inventoryRecords[detail.MaterialId] 
                             where inventory.Number < detail.Number select detail).Any();
            if (!isValid)
            {
                Services.Notifier.Information(T("库存数量不够！"));
                return Json(new { Result = false });
            }
            #endregion
            #region 维修领料出库
            var user = Services.WorkContext.CurrentUser;
            ContentItem contentItem = Services.ContentManager.New("Voucher");
            var part = contentItem.As<VoucherPart>();
            VoucherPartRecord record = part.Record;
            record.VoucherNo = voucherNo;
            record.Operation = InventoryChangeOperation.出库;
            record.Date = DateTime.Now;
            record.OperatorId = user.Id;
            record.Remark = "维修领料出库";
            Services.ContentManager.Create(contentItem, VersionOptions.Draft);
            Services.ContentManager.Publish(contentItem);
          
            foreach (var detail in details)
            {
                InventoryRecord inventory = inventoryRecords[detail.MaterialId];
                detail.CostPrice = inventory.Amount / inventory.Number;
                _inventoryService.DecreaseInventory(detail.MaterialId, detail.Number, detail.CostPrice);
                _inventoryChangeRepository.Create(new InventoryChangeRecord
                {
                    VoucherNo = voucherNo,
                    MaterialId = detail.MaterialId,
                    Operation = InventoryChangeOperation.出库,
                    CostPrice = detail.CostPrice,
                    Number = detail.Number,
                    Date = DateTime.Today,
                    OperatorId = user.Id
                });
            }
            #endregion
            }
            #region 更新Service表
            var servicecontentItem = Services.ContentManager.Get(model.id, VersionOptions.Latest);
            var servicePart = servicecontentItem.As<ServicePart>();
            ServicePartRecord serviceRecord = servicePart.Record;
            serviceRecord.ServiceVoucherNo = voucherNo;
            serviceRecord.StockOutVoucher =details.Count>0?voucherNo:"";
            serviceRecord.ServicePerson = Services.ContentManager.Get<UserPart>(model.ServicePersonId).Record;
            Services.ContentManager.Publish(servicecontentItem);
            #endregion
            return Json(new { Result = true, Url = Url.Action("ServiceVoucherPreview", new { voucherNo, model.id }) });
        }

        public ActionResult ServiceVoucherPreview(string voucherNo,int id)
        {
            List<InventoryChangeRecord> queryInventoryChange =
                _inventoryChangeRepository.Table.Where(u => u.VoucherNo == voucherNo).OrderBy(u => u.Id).ToList();
            var servicecontentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            var servicePart = servicecontentItem.As<ServicePart>();
            ServicePartRecord serviceRecord = servicePart.Record;
            var model = new ServiceCreateListViewModel
            {
                VoucherNo = voucherNo,
                Address = serviceRecord.House.Apartment.Name + "/" + serviceRecord.House.Building.Name + "/" +
                                 serviceRecord.House.HouseNumber,
                FaultDescription = serviceRecord.FaultDescription,
                OwnerName = serviceRecord.Owner.Name,
                ReceivedDate = serviceRecord.ReceivedDate.ToString("d"),
                Mobile = serviceRecord.Mobile,
                ServicePersonName = serviceRecord.ServicePerson.UserName,
                List = new List<HelpListViewModel>()
            };
            Dictionary<int, MaterialPart> materials =
                Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            for (int i = 0; i < queryInventoryChange.Count; i++)
            {
                if (materials.ContainsKey(queryInventoryChange[i].MaterialId))
                {
                    MaterialPart material = materials[queryInventoryChange[i].MaterialId];
                    model.List.Add(new HelpListViewModel
                    {
                        Id = i + 1,
                        Material_Name = material.Name,
                        Material_Brand = material.Brand,
                        Material_Model = material.Model,
                        Material_SerialNo = material.SerialNo,
                        Material_Unit = material.Unit,
                        CostPrice = queryInventoryChange[i].CostPrice,
                        Number = queryInventoryChange[i].Number
                    });
                }
            }
            return View(model);
        }

        public ActionResult ServiceVoucherHistoryPreview(int id)
        {
            var servicecontentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            var servicePart = servicecontentItem.As<ServicePart>();
            ServicePartRecord serviceRecord = servicePart.Record;
            List<InventoryChangeRecord> queryInventoryChange =
                _inventoryChangeRepository.Table.Where(u => u.VoucherNo == serviceRecord.StockOutVoucher).OrderBy(u => u.Id).ToList();
       
            var model = new ServiceCreateListViewModel
            {
                VoucherNo = serviceRecord.StockOutVoucher,
                Address = serviceRecord.House.Apartment.Name + "/" + serviceRecord.House.Building.Name + "/" +
                                 serviceRecord.House.HouseNumber,
                FaultDescription = serviceRecord.FaultDescription,
                OwnerName = serviceRecord.Owner.Name,
                ReceivedDate = serviceRecord.ReceivedDate.ToString("d"),
                Mobile = serviceRecord.Mobile,
                ServicePersonName = serviceRecord.ServicePerson.UserName,
                List = new List<HelpListViewModel>()
            };
            Dictionary<int, MaterialPart> materials =
                Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            for (int i = 0; i < queryInventoryChange.Count; i++)
            {
                if (materials.ContainsKey(queryInventoryChange[i].MaterialId))
                {
                    MaterialPart material = materials[queryInventoryChange[i].MaterialId];
                    model.List.Add(new HelpListViewModel
                    {
                        Id = i + 1,
                        Material_Name = material.Name,
                        Material_Brand = material.Brand,
                        Material_Model = material.Model,
                        Material_SerialNo = material.SerialNo,
                        Material_Unit = material.Unit,
                        CostPrice = queryInventoryChange[i].CostPrice,
                        Number = queryInventoryChange[i].Number
                    });
                }
            }
            return View(model);
        }

        public ActionResult ServiceCharge(int id)
        {
            var contentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
            var part = contentItem.As<ServicePart>();
            ServicePartRecord record = part.Record;
            var createView = new ServiceChargeCreateViewModel();
            createView.id = id;
            createView.Address = record.House.Apartment.Name + "/" + record.House.Building.Name + "/" +
                                 record.House.HouseNumber;
            createView.FaultDescription = record.FaultDescription;
            createView.OwnerName = record.Owner.Name;
            createView.ReceivedDate = record.ReceivedDate.ToString("d");
            createView.Mobile = record.Mobile;
            createView.VoucherNo = record.ServiceVoucherNo;

            #region 绑定materials grid
            List<InventoryChangeRecord> queryInventoryChange =_inventoryChangeRepository.Table.Where(u => u.VoucherNo == record.ServiceVoucherNo).OrderBy(u => u.Id).ToList();
            Dictionary<int, MaterialPart> materials =Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
            for (int i = 0; i < queryInventoryChange.Count; i++)
            {
                if (materials.ContainsKey(queryInventoryChange[i].MaterialId))
                {
                    MaterialPart material = materials[queryInventoryChange[i].MaterialId];
                    createView.InventoryDetailEntities.Add(new InventoryDetail
                    {
                        MaterialId=material.Id,
                        Name = material.Name,
                        Brand = material.Brand,
                        Model = material.Model,
                        //SerialNo = material.SerialNo,
                        Unit = material.Unit,
                        CostPrice = queryInventoryChange[i].CostPrice,
                        Number = queryInventoryChange[i].Number
                       
                    });
                }
            }
            #endregion

            return View(createView);
        }
         [HttpPost]
        public ActionResult ServiceChargeCreate(MaterialReturnViewModel model)
         {
             var stockReturnVoucher = "";
            #region 维修退货入库
            var details = model.InventoryReturnDetailEntities;
             var checkReturnNum = details.Where(x => x.ReturnNumber > 0).ToList();
             if (checkReturnNum.Count > 0)
             {
                 var user = Services.WorkContext.CurrentUser;
                 var voucherNo = _voucherNumberService.GenerateNumber();
                 stockReturnVoucher = voucherNo;
                 ContentItem contentItem = Services.ContentManager.New("Voucher");
                 var part = contentItem.As<VoucherPart>();
                 VoucherPartRecord record = part.Record;
                 record.VoucherNo = voucherNo;
                 record.Operation = InventoryChangeOperation.入库;
                 record.Date = DateTime.Now;
                 record.OperatorId = user.Id;
                 record.Remark = "维修退货入库";
                 Services.ContentManager.Create(contentItem, VersionOptions.Draft);
                 Services.ContentManager.Publish(contentItem);
                 foreach (var detail in details)
                 {
                     if (detail.ReturnNumber > 0)
                     {
                         _inventoryService.IncreaseInventory(new InventoryInfo
                         {
                             MaterialId = detail.MaterialId,
                             CostPrice = detail.CostPrice
                         }, detail.ReturnNumber);

                         _inventoryChangeRepository.Create(new InventoryChangeRecord
                         {
                             VoucherNo = voucherNo,
                             MaterialId = detail.MaterialId,
                             Operation = InventoryChangeOperation.入库,
                             CostPrice = detail.CostPrice,
                             Number = detail.ReturnNumber,
                             Date = DateTime.Today

                         });
                     }
                 }
             }

             #endregion

             var servicecontentItem = Services.ContentManager.Get(model.Id, VersionOptions.Latest);
             var servicePart = servicecontentItem.As<ServicePart>();
             ServicePartRecord serviceRecord = servicePart.Record;
             serviceRecord.ServiceCharge = model.ServiceCharge;
             serviceRecord.StockReturnVoucher = stockReturnVoucher;
             Services.ContentManager.Publish(servicecontentItem);
            return Json(new { Result = true, Url = Url.Action("ServiceChargePreview", new { model.Id }) });
        }

         public ActionResult ServiceChargePreview(int id)
         {
             var servicecontentItem = Services.ContentManager.Get(id, VersionOptions.Latest);
             var servicePart = servicecontentItem.As<ServicePart>();
             ServicePartRecord serviceRecord = servicePart.Record;
             List<InventoryChangeRecord> queryInventoryChange =_inventoryChangeRepository.Table
                 .Where(u => u.VoucherNo == serviceRecord.StockOutVoucher)
                 .OrderBy(u => u.Id).ToList();
            
             var model = new ServiceChargeCreateViewModel
             {
                 VoucherNo = serviceRecord.StockOutVoucher,
                 Address = serviceRecord.House.Apartment.Name + "/" + serviceRecord.House.Building.Name + "/" +
                                  serviceRecord.House.HouseNumber,
                 FaultDescription = serviceRecord.FaultDescription,
                 OwnerName = serviceRecord.Owner.Name,
                 ReceivedDate = serviceRecord.ReceivedDate.ToString("d"),
                 Mobile = serviceRecord.Mobile,
                 ServicePersonName = serviceRecord.ServicePerson.UserName,
                 ServiceCharge = serviceRecord.ServiceCharge,
                 List = new List<HelpListViewModel>()
             };
             Dictionary<int, MaterialPart> materials =Services.ContentManager.Query<MaterialPart, MaterialPartRecord>().List().ToDictionary(x => x.Id);
             for (int i = 0; i < queryInventoryChange.Count; i++)
             {
                 if (materials.ContainsKey(queryInventoryChange[i].MaterialId))
                 {
                     var returnNumber = 0;
                     List<InventoryChangeRecord> queryInventoryReturn = _inventoryChangeRepository.Table
                   .Where(u => u.VoucherNo == serviceRecord.StockReturnVoucher && u.MaterialId == queryInventoryChange[i].MaterialId)
                   .OrderBy(u => u.Id).ToList();
                     if (queryInventoryReturn.Count > 0) { 
                      returnNumber = queryInventoryReturn[0].Number;
                     }
                     MaterialPart material = materials[queryInventoryChange[i].MaterialId];
                     model.List.Add(new HelpListViewModel
                     {
                         Id = i + 1,
                         Material_Name = material.Name,
                         Material_Brand = material.Brand,
                         Material_Model = material.Model,
                         Material_SerialNo = material.SerialNo,
                         Material_Unit = material.Unit,
                         CostPrice = queryInventoryChange[i].CostPrice,
                         Number = queryInventoryChange[i].Number - returnNumber,
                         ReturnNumber = returnNumber
                     });
                 }
             }
             return View(model);
         }

        public ActionResult Index()
        {

            return List();
        }

        public ActionResult List()
        {
            var contentItem = Services.ContentManager.New("Service");
            contentItem.Weld(new ServicePart());
            var model = Services.ContentManager.BuildDisplay(contentItem, "List");
            return View("List", model);
        }

        [HttpPost]
        public ActionResult List(FormCollection input, int page = 1, int pageSize = 10, string sortBy = null, string sortOrder = "asc")
        {
            var query = Services.ContentManager.Query<ServicePart, ServicePartRecord>();

            //Filter
           /* var options = new ServiceFilterOptions();
            UpdateModel(options, "FilterOptions", input);
            if (!String.IsNullOrWhiteSpace(options.Search))
            {
                query = query.Where(u => u.Name.Contains(options.Search));
            }

            if (!String.IsNullOrWhiteSpace(options.Contactor))
            {
                query = query.Where(u => u.Contactor.Contains(options.Contactor));
            }
            */
            var totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1) * pageSize, pageSize)
                .Select(item => new ServiceListViewModel
                {
                    Id = item.Record.Id,
                    HouseNumber = item.Record.House.HouseNumber,
                    OwnerName = item.Record.Owner.Name,
                    Mobile = item.Record.Mobile,
                    FaultDescription = item.Record.FaultDescription,
                    ReceivedDate = item.Record.ReceivedDate.ToString("d"),
                    ServicePersonName = item.Record.ServicePerson == null ? "" : item.Record.ServicePerson.UserName,
                    ServiceCharge = item.Record.ServiceCharge,
                    ServiceVoucherNo = item.Record.ServiceVoucherNo,
                    StockOutVoucher = item.Record.StockOutVoucher,
                    StockReturnVoucher = item.Record.StockReturnVoucher
                }).ToList();
            var result = new
            {
                page,
                totalPages = Math.Ceiling((double)totalRecords / pageSize),
                totalRecords,
                rows = records
            };
            return Json(result);
        }
        [HttpPost]
        public ActionResult Delete(List<int> selectedIds)
        {

            try
            {
                var items = Services.ContentManager.Query().ForContentItems(selectedIds).List();
                foreach (var item in items)
                {
                    Services.ContentManager.Remove(item);
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                Services.Notifier.Error(T("删除失败！"));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult ServicePersonDropdown(string term, int page = 1, int pageSize = 5, string sortBy = null,
            string sortOrder = "asc")
        {
            IContentQuery<UserPart, UserPartRecord> query =
                Services.ContentManager.Query<UserPart, UserPartRecord>();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(x => x.UserName.Contains(term));
            }
            int totalRecords = query.Count();
            var records = query
                .OrderBy(sortBy, sortOrder)
                .Slice((page - 1) * pageSize, pageSize)
                .Select(item => new
                {
                    id = item.Record.ContentItemRecord.Id,
                    text = item.UserName
                }).ToList();
            return Json(new { records, total = totalRecords });
        }
        public ActionResult GetOwnerItems(int apartmentItemId, int buildingItemId,int houseId)
        {
            var query = Services.ContentManager.Query<HousePart, HousePartRecord>().List()
                .Where(x => x.Apartment.Id == apartmentItemId && x.Building.Id == buildingItemId && x.Id == houseId)
                .Select(item => new
                {
                    OwnerName = item.Owner.Name,
                    OwnerId = item.Owner.Id,
                    Mobile = item.Owner.Phone
                });
            return Json(query);
        }
        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}