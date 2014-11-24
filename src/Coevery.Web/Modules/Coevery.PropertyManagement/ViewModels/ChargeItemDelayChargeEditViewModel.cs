using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.ViewModels
{
    public class ChargeItemDelayChargeEditViewModel
    {
        public string ChargeItemName { get; set; }
        public string DelayChargeDays { get; set; }
        public double? DelayChargeRatio { get; set; }
        public DelayChargeCalculationMethodOption? DelayChargeCalculationMethod { get; set; }
        public StartCalculationDatetimeOption? StartCalculationDatetime { get; set; }
        public ChargingPeriodPrecisionOption? ChargingPeriodPrecision { get; set; }
        public DefaultChargingPeriodOption? DefaultChargingPeriod { get; set; }
        public int Id { get; set; }
    }
}