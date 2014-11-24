using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Utilities;

namespace Coevery.PropertyManagement.Models
{
    public sealed class ChargeItemSettingPart : ContentPart<ChargeItemSettingPartRecord>
    {
       // internal LazyField<int?> _chargeItemIdField = new LazyField<int?>();
        public string Name {
            get { return Record.Name; }
            set { Record.Name = value; }
        }
        public CalculationMethodOption? CalculationMethod
        {
            get { return Record.CalculationMethod; }
            set { Record.CalculationMethod = value; }
        }

        //public int? ChargeItemId
        //{
        //    get { return _chargeItemIdField.Value; }
        //    set { _chargeItemIdField.Value = value; }
        //}

        //public ChargeItemPartRecord ChargeItem
        //{
        //    get { return Record.ChargeItem; }
        //    set { Record.ChargeItem = value; }
        //}

        public decimal? UnitPrice
        {
            get { return Record.UnitPrice; }
            set { Record.UnitPrice = value; }
        }

        public MeteringModeOption? MeteringMode
        {
            get { return Record.MeteringMode; }
            set { Record.MeteringMode = value; }
        }

        public ChargingPeriodOption? ChargingPeriod
        {
            get { return Record.ChargingPeriod; }
            set { Record.ChargingPeriod = value; }
        }

        public string CustomFormula
        {
            get { return Record.CustomFormula; }
            set { Record.CustomFormula = value; }
        }

        public string Remark
        {
            get { return Record.Remark; }
            set { Record.Remark = value; }
        }

        public decimal? Money
        {
            get { return Record.Money; }
            set { Record.Money = value; }
        }

        public ItemCategoryOption? ItemCategory
        {
            get { return Record.ItemCategory; }
            set { Record.ItemCategory = value; }
        }

        public int? MeterType
        {
            get { return Record.MeterType; }
            set { Record.MeterType = value; }
        }
        public string DelayChargeDays
        {
            get { return Record.DelayChargeDays; }
            set { Record.DelayChargeDays = value; }
        }

        public double? DelayChargeRatio
        {
            get { return Record.DelayChargeRatio; }
            set { Record.DelayChargeRatio = value; }
        }

        public DelayChargeCalculationMethodOption? DelayChargeCalculationMethod
        {
            get { return Record.DelayChargeCalculationMethod; }
            set { Record.DelayChargeCalculationMethod = value; }
        }

        public StartCalculationDatetimeOption? StartCalculationDatetime
        {
            get { return Record.StartCalculationDatetime; }
            set { Record.StartCalculationDatetime = value; }
        }

        public ChargingPeriodPrecisionOption? ChargingPeriodPrecision
        {
            get { return Record.ChargingPeriodPrecision; }
            set { Record.ChargingPeriodPrecision = value; }
        }

        public DefaultChargingPeriodOption? DefaultChargingPeriod
        {
            get { return Record.DefaultChargingPeriod; }
            set { Record.DefaultChargingPeriod = value; }
        }
    }
}