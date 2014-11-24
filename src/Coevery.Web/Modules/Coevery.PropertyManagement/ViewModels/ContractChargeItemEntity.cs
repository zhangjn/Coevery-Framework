namespace Coevery.PropertyManagement.ViewModels
{
    public class ContractChargeItemEntity
    {
        public int Id { get; set; } //别忘了Id
        public int ContractId { get; set; } //合同Id
        public int ChargeItemSettingId { get; set; } //有可能用到收费标准Id
        public int ExpenserOptionId { get; set; }//费用承担人枚举id
        public string ChargeItemName { get; set; } //收费项目名称
        public string ChargeItemSettingName { get; set; } //收费标准名称
        public string ExpenserName { get; set; } //费用承担者姓名
        public string ChargeBeginDate { get; set; }
        public string ChargeEndDate { get; set; }
        public string ChargeDescription { get; set; } //备注


        //收费标准中需要显示在合同收费项目中的字段

        public string CalculationMethod { get; set; }
        public decimal? UnitPrice { get; set; }
        public string MeteringMode { get; set; }
        public string ChargingPeriod { get; set; }
        public string CustomFormula { get; set; }
        public decimal? Money { get; set; }
        public string ItemCategory { get; set; }
        public string MeterTypeName { get; set; }
        public string DelayChargeDays { get; set; }
        public double? DelayChargeRatio { get; set; }
        public string DelayChargeCalculationMethod { get; set; }
        public string StartCalculationDatetime { get; set; }
        public string ChargingPeriodPrecision { get; set; }
        public string DefaultChargingPeriod { get; set; }

        //要用到的Id
        public int? MeterTypeId { get; set; }
        public int? CalculationMethodId { get; set; }
        public int? MeteringModeId { get; set; }
        public int? ChargingPeriodId { get; set; }
        public int? ItemCategoryId { get; set; }
        public int? DelayChargeCalculationMethodId { get; set; }
        public int? StartCalculationDatetimeId { get; set; }
        public int? ChargingPeriodPrecisionId { get; set; }
        public int? DefaultChargingPeriodId { get; set; }
    }

    public enum ExpenserOption:int
    {
        业主=1,
        租户=2
    }
}