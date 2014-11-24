using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class BillReportListViewModel
    {
        public string ContractNumber { get; set; } //合同号
        public string HouseNumber { get; set; }
        public string HouseOwnerName { get; set; }
        public string HouseRenterName { get; set; }
        public string ExpenserName { get; set; }
        public string OfficerName { get; set; }

        public string ChargeItemSettingName { get; set; } //费用项-收费标准项目名称
        public string ChargeTerm { get; set; } // 计费期间
        public string ChargeUnit { get; set; } // 计量单位
        public double? ChargeNumber { get; set; } // 计费数量
        public decimal? ChargeUnitPrice { get; set; } // 计费单价
        public decimal? ChargeItemAmount { get; set; }
        public string BillStatus { get; set; }
    }

    public class GridColumn
    {
        public string Name { get; set; }
        public string Label { get; set; }
    }
}