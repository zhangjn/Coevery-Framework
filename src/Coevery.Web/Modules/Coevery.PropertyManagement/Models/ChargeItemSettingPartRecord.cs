using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("ChargeItemSetting")]
     public class ChargeItemSettingPartRecord : ContentPartRecord {
        public virtual string Name { get; set; }
		public virtual CalculationMethodOption? CalculationMethod { get; set; }
		public virtual decimal? UnitPrice { get; set; }
        public virtual MeteringModeOption? MeteringMode { get; set; }
		public virtual ChargingPeriodOption? ChargingPeriod { get; set; }
		public virtual string CustomFormula { get; set; }
		public virtual string Remark { get; set; }
		public virtual decimal? Money { get; set; }

        public virtual ItemCategoryOption? ItemCategory { get; set; }
        public virtual int? MeterType { get; set; }
        public virtual string DelayChargeDays { get; set; }
        public virtual double? DelayChargeRatio { get; set; }
        public virtual DelayChargeCalculationMethodOption? DelayChargeCalculationMethod { get; set; }
        public virtual StartCalculationDatetimeOption? StartCalculationDatetime { get; set; }
        public virtual ChargingPeriodPrecisionOption? ChargingPeriodPrecision { get; set; }
        public virtual DefaultChargingPeriodOption? DefaultChargingPeriod { get; set; }
    }

    public enum CalculationMethodOption {
		单价数量,
		指定金额,
		自定义公式,
	}

    public enum ChargingPeriodOption:int
    {
        每1个月收一次=1,
        每2个月收一次=2,
        每3个月收一次=3,
        每4个月收一次=4,
        每5个月收一次=5,
        每6个月收一次=6,
        每7个月收一次=7,
        每8个月收一次=8,
        每9个月收一次=9,
        每10个月收一次=10,
        每11个月收一次=11,
        每12个月收一次=12,
    }

    public enum MeteringModeOption
    {
        建筑面积,
        套内面积,
        用量,
    }

    public enum ItemCategoryOption
    {
        周期性收费项目,
        抄表类收费项目,
        临时性收费项目,
    }
    public enum DelayChargeCalculationMethodOption
    {
        手动计算滞纳金,
        自动计算滞纳金,
    }

    public enum StartCalculationDatetimeOption
    {
        欠费开始时间,
        欠费结束时间,
    }

    public enum ChargingPeriodPrecisionOption
    {
        按周期计算,
        按实际天数计算,
    }


    public enum DefaultChargingPeriodOption
    {
        当期收当期,
        当期收上期,
        当期收下期,
    }

}
