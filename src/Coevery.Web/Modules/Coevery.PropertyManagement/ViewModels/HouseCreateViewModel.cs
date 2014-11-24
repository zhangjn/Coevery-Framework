using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.ViewModels
{
    public class HouseCreateViewModel
    {
        public HouseCreateViewModel()
        {
            ChargeItemEntities = new List<HouseChargeItemEntity>();
            MeterTypeItemEntities = new List<HouseMeterEntity>();
            ChargeListItems=new List<SelectListItem>();
            MeterListItems=new List<SelectListItem>();
            ApartmentListItems=new List<SelectListItem>();
            BuildingListItems=new List<SelectListItem>();
            OwnerListItems=new List<SelectListItem>();
            ExpenserListItems=new List<SelectListItem>();
            OfficerListItems=new List<SelectListItem>();
        }

        public List<HouseChargeItemEntity> ChargeItemEntities { get; set; }//创建编辑页grid显示以及edit保存的时候用
        public List<HouseMeterEntity> MeterTypeItemEntities { get; set; }//创建编辑页grid显示以及edit保存的时候用

        public List<SelectListItem> ChargeListItems { get; set; }//加载收费项目条目
        public List<SelectListItem> MeterListItems { get; set; }//根据收费项目条目加载收费标准，先写下来，有可能保存的时候用

        public List<SelectListItem> ApartmentListItems { get; set; }
        public List<SelectListItem> BuildingListItems { get; set; }
        public List<SelectListItem> OwnerListItems { get; set; }
        public List<SelectListItem> ExpenserListItems { get; set; }
        public List<SelectListItem> OfficerListItems { get; set; }

        public int BuildingId { get; set; }
        public int ApartmentId { get; set; }
        public int OwnerId { get; set; }
        public string HouseNumber { get; set; }
        public int OfficerId { get; set; }
        public double BuildingArea { get; set; }
        public double InsideArea { get; set; }
        public double PoolArea { get; set; }
        public int HouseStatusId { get; set; }//这是一个枚举类型哦

        public int Id { get; set; }  //别忘了房间Id，编辑页面可能用到
        

    }
}