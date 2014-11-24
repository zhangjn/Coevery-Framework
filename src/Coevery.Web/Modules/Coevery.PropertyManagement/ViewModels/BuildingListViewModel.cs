

using System;
using System.Collections.Generic;

namespace Coevery.PropertyManagement.ViewModels {
    public sealed class BuildingListViewModel {
		public int Id { get; set; }
		public int VersionId { get; set; }
		public string Apartment_Name{ get; set; }
		public int? Apartment{ get; set; }
		public string Name{ get; set; }
		public string Description{ get; set; }
    }
}
