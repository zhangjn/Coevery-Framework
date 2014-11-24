using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class ServiceListViewModel
    {
        public int Id { get; set; }
        public string  HouseNumber { get; set; }
        public string  OwnerName { get; set; }
        public string Mobile { get; set; }
        public string FaultDescription { get; set; }
        public string ReceivedDate { get; set; }
        public string  ServicePersonName { get; set; }
        public decimal ServiceCharge { get; set; }
        public string ServiceVoucherNo { get; set; }
        public string StockOutVoucher { get; set; }
        public string StockReturnVoucher { get; set; }
    }
}