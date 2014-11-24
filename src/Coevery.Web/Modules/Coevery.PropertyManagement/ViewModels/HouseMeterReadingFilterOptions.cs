using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class HouseMeterReadingFilterOptions
    {
        public int? ApartmentId { get; set; }
        public int? BuildingId { get; set; }
        public string HouseNumber { get; set; }
        public DateTime? DateFrom { get; set; }
    }
}