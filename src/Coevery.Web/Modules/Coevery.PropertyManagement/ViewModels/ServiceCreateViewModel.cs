using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class ServiceCreateViewModel
    {
        public int ApartmentId { get; set; }
        public int BuildingId { get; set; }
        public int HouseId { get; set; }
        public string HouseNumber { get; set; }
        public int OwnerId { get; set; }
        public string Mobile { get; set; }
        public string FaultDescription { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int ServicePersonId { get; set; }
        public decimal ServiceCharge { get; set; }
        public string ServiceVoucherNo { get; set; }
        public string StockOutVoucher { get; set; }
        public string StockReturnVoucher { get; set; }
    }
}