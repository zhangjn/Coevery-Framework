using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class BillAllocationReportListViewModel
    {
        public string ContractNumber { get; set; }
        public string HouseNumber { get; set; }
        public string CustomerName { get; set; }
        public string ChargeItemName { get; set; }
        public decimal? LeftBalance  { get; set; } //结转往期
        public decimal? PaidPreviousPeriod { get; set; } // 实收上期
        public decimal? PaidCurrentPeriod { get; set; } //实收本期
    }
}