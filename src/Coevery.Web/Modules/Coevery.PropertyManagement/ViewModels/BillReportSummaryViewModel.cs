using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class BillReportSummaryViewModel
    {
        public string ChargeItemName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? ReceivedMoney { get; set; }
        public decimal? OwingMoney { get; set; }
    }
}