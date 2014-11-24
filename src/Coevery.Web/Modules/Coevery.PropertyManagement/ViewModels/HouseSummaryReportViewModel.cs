using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class HouseSummaryReportViewModel
    {
        public double? BuildingArea { get; set; }
        public double? InsideArea { get; set; }
        public double? PoolArea { get; set; }
        public string HouseStatus { get; set; }
    }
}