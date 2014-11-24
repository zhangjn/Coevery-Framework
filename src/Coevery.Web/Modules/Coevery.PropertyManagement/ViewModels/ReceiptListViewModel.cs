using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class ReceiptListViewModel
    {
        public int CustomerId { get; set; }
        public int ContractId { get; set; }
        public int HouseId { get; set; }
        public int PaymentId { get; set; }

        public string ContractNumber { get; set; } //合同号
        public string HouseNumber { get; set; }
        public string HouseOwnerName { get; set; }
        public string HouseRenterName { get; set; }
        public string PaymentType { get; set; }
        public string ChargeItemSettingName { get; set; } //费用项-收费标准项目名称
        public decimal? ChargeItemAmount { get; set; }
    }
}