

using System;
using System.Collections.Generic;

namespace Coevery.PropertyManagement.ViewModels {
    public sealed class CustomerListViewModel {
		public int Id { get; set; }
		public int VersionId { get; set; }
		public string Number{ get; set; }
		public string Name{ get; set; }
		public string CustomerType{ get; set; }
		public string Phone{ get; set; }
		public string Description{ get; set; }
    }
}
