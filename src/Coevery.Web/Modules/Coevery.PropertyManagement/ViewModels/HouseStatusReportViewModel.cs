using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class HouseStatusReportViewModel
    {
        public string ContractNumber { get; set; } //合同号
        public string HouseNumber { get; set; }
        public string HouseOwnerName { get; set; }
        public string HouseRenterName { get; set; }
        public string ExpenserName { get; set; }
        public string OfficerName { get; set; }
        public string ApartmentName { get; set; }
        public string BuildingName { get; set; }
        public double? BuildingArea { get; set; }
        public double? InsideArea { get; set; }
        public double? PoolArea { get; set; }
        public string HouseStatus { get; set; }
    }
}