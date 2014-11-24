using System;
using System.Collections.Generic;

namespace Coevery.PropertyManagement.ViewModels
{
    public sealed class ChargeItemSettingListViewModel
    {
        public int Id { get; set; }
        public int VersionId { get; set; }
        public string Name { get; set; }
        public string ItemCategory { get; set; }
        public int? ChargeItemId { get; set; }
        public string CalculationMethod { get; set; }
        public decimal? UnitPrice { get; set; }
        public string MeteringMode { get; set; }
        public decimal? Money { get; set; }
        public string CustomFormula { get; set; }
        public string ChargingPeriod { get; set; }
        public string Remark { get; set; }
    }
}