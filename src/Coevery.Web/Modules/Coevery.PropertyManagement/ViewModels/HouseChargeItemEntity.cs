using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.ViewModels
{
    public class HouseChargeItemEntity
    {
        public int Id { get; set; }
        public int ChargeItemSettingId { get; set; } //
        public string ChargeItemSettingName { get; set; } //收费标准名称
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; } //备注
        public int HouseId { get; set; } //有可能用到房间Id
        public int ExpenserOptionId { get; set; }//费用承担人业主或租户 枚举id
        public int ExpenserId { get; set; } //费用承担人Id
        public string ExpenserName { get; set; } //费用承担者姓名

        public virtual CalculationMethodOption? CalculationMethod { get; set; }
        public virtual decimal? UnitPrice { get; set; }
        public virtual MeteringModeOption? MeteringMode { get; set; }
        public virtual ChargingPeriodOption? ChargingPeriod { get; set; }
        public virtual string CustomFormula { get; set; }
        public virtual string Remark { get; set; }
        public virtual decimal? Money { get; set; }
        public virtual ItemCategoryOption? ItemCategory { get; set; }
        public virtual int? MeterType { get; set; }
        public virtual StartCalculationDatetimeOption? StartCalculationDatetime { get; set; }
        public virtual ChargingPeriodPrecisionOption? ChargingPeriodPrecision { get; set; }

        public string ItemCategoryDisplayName { get; set; }
        public string CalculationMethodDisplayName { get; set; }
        public string ChargingPeriodDisplayName { get; set; }
    }
}