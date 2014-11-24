using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.ViewModels
{
    public class ContractEditViewModel
    {
        public ContractEditViewModel()
        {
            HouseEntities = new List<ContractHouseEntity>();
            ChargeItemEntities = new List<ContractChargeItemEntity>();
            ChargeListItems = new List<SelectListItem>();
            ApartmentListItems = new List<SelectListItem>();
            OwnerListItems = new List<SelectListItem>();
            RenterListItems = new List<SelectListItem>();
            MeterTypeListItems = new List<SelectListItem>();
        }

        public List<ContractHouseEntity> HouseEntities { get; set; }
        public List<ContractChargeItemEntity> ChargeItemEntities { get; set; } //创建编辑页grid显示以及edit保存的时候用

        public List<SelectListItem> ChargeListItems { get; set; } //加载收费项目条目
        
        public List<SelectListItem> ApartmentListItems { get; set; } //楼盘列表
        public List<SelectListItem> OwnerListItems { get; set; } //业主列表  
        public List<SelectListItem> RenterListItems { get; set; } //租户列表 与租户同一个列表
        public List<SelectListItem> MeterTypeListItems { get; set; }//收费项目有可能选择仪表类型

        public string Number { get; set; }
        public string Name { get; set; }
        public int RenterId { get; set; }
        public string RenterName { get; set; }
        public int ContractStatusId { get; set; } //枚举类型
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; } //备注

        public int Id { get; set; } //别忘了合同Id，编辑页面会用到
        public ContractStatusOption? ContractStatus { get; set; }
        public HouseStatusOption HouseStatus { get; set; }
        public int HouseStatusId { get; set; }
    }
}