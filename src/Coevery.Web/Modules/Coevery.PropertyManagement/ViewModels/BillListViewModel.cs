using System;
using System.Collections.Generic;
namespace Coevery.PropertyManagement.ViewModels
{
    public sealed class BillListViewModel
    {
        public BillListViewModel()
        {
            BillEntities=new List<BillListViewModel>();
        }
        public int CustomerId { get; set; }
        public int ContractId { get; set; }
        public int HouseId { get; set; }
        public int ChargeItemSettingId { get; set; }

        public string ContractNumber { get; set; } //合同号
        public string ApartmentName { get; set; }//楼盘名称
        public string BuildingName { get; set; }//楼宇名称
        public string HouseNumber { get; set; }
        public string ChargeItemName { get; set; } //收费项目名称
        public string ChargeItemSettingName { get; set; } //费用项-收费标准项目名称
        public DateTime BeginDate { get; set; } //开始时间
        public DateTime EndDate { get; set; } //结束时间
        public decimal? UnitPrice { get; set; } //单价
        public double? LastReading { get; set; } //上次抄表读数
        public double? CurrentReading { get; set; } //本次抄表读数

        public double? Quantity
        {
            get
            {
                if (CurrentReading.HasValue)
                {
                    if (!LastReading.HasValue)
                    {
                        return CurrentReading;
                    }
                    return CurrentReading - LastReading;
                }
                return null;
            } //数量
        }

        public decimal? Amount { get; set; } //金额

        public string OfficerName { get; set; }

        public List<BillListViewModel> BillEntities { get; set; }
    }
}