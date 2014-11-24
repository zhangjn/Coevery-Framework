using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class StatementReportListViewModel
    {
        public DateTime? Date { get; set; } 
        public string ContractNumber { get; set; }
        public string OfficerName { get; set; }

        public string ChargeItemName { get; set; } //费用项-收费标准项目名称
        public string ChargeTerm { get; set; } // 计费期间
        public decimal? ChargeItemAmount { get; set; }

        public decimal? ReceivedAmount { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? OwingAmount { get; set; }
    }
}