

using System;
using System.Collections.Generic;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.ViewModels {
    public sealed class ContractListViewModel {
		public int Id { get; set; }
		public int VersionId { get; set; }
		public string Number{ get; set; }
		public string Name{ get; set; }
		public string OwnerName{ get; set; }
		public int? OwnerId{ get; set; }
		public string RenterName{ get; set; }
		public int? RenterId{ get; set; }
		public string ContractStatus{ get; set; }
		public DateTime? BeginDate{ get; set; }
		public DateTime? EndDate{ get; set; }
		public string Description{ get; set; }
        public HouseStatusOption HouseStatus { get; set; }
    }
}
