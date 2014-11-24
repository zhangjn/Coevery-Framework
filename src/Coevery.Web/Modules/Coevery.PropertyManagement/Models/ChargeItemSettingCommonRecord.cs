using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models {

    [Table("ChargeItemSettingCommon")]
    public class ChargeItemSettingCommonRecord {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual CalculationMethodOption? CalculationMethod { get; set; }
        public virtual decimal? UnitPrice { get; set; }
        public virtual MeteringModeOption? MeteringMode { get; set; }
        public virtual ChargingPeriodOption? ChargingPeriod { get; set; }
        public virtual string CustomFormula { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal? Money { get; set; }
        public int ExpenserOption { get; set; }

        public virtual ItemCategoryOption? ItemCategory { get; set; }
        public virtual int? MeterTypeId { get; set; }
        public virtual string DelayChargeDays { get; set; }
        public virtual double? DelayChargeRatio { get; set; }
        public virtual DelayChargeCalculationMethodOption? DelayChargeCalculationMethod { get; set; }
        public virtual StartCalculationDatetimeOption? StartCalculationDatetime { get; set; }
        public virtual ChargingPeriodPrecisionOption? ChargingPeriodPrecision { get; set; }
        public virtual DefaultChargingPeriodOption? DefaultChargingPeriod { get; set; }

        public virtual DateTime BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

        public virtual ChargeItemSettingPartRecord ChargeItemSetting { get; set; }
    }
}