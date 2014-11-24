

using System;
using System.Collections.Generic;

namespace Coevery.PropertyManagement.ViewModels {
    public sealed class HouseListViewModel {
		public int Id { get; set; }
		public int VersionId { get; set; }
		public string Apartment_Name{ get; set; }
		public int? Apartment{ get; set; }
		public string Building_Name{ get; set; }
		public int? Building{ get; set; }
		public string HouseNumber{ get; set; }
		public double? InsideArea{ get; set; }
		public double? BuildingArea{ get; set; }
		public double? PoolArea{ get; set; }
		public string HouseStatus{ get; set; }
		public string OwnerName{ get; set; }
		public string OfficerName{ get; set; }
        public string Contact { get; set; }
        public List<string> ChargeItemNameList { get; set; }
        public string ChargeItemNames { get; set; }
        public string MeterItemNames { get; set; }
    }
}
