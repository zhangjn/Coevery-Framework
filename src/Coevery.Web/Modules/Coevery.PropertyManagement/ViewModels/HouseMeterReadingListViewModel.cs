

using System;
using System.Collections.Generic;

namespace Coevery.PropertyManagement.ViewModels {
    public sealed class HouseMeterReadingListViewModel {
        public int HouseMeterId { get; set; }
        public string HouseNumber { get; set; }
        public string HouseOwnerName { get; set; }
		public string MeterTypeName{ get; set; }
        public double? PreviousMeterData { get; set; }
		public double? MeterData{ get; set; }
		public double? Amount{ get; set; }
		public string Status{ get; set; }
		public string Remarks{ get; set; }
        public int? OwnerId { get; set; }
        public int? HouseMeterTypeItemId { get; set; }
    }
}
