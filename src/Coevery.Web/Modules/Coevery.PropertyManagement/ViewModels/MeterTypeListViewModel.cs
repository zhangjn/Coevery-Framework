

using System;
using System.Collections.Generic;

namespace Coevery.PropertyManagement.ViewModels
{
    public sealed class MeterTypeListViewModel {
		public int Id { get; set; }
		public int VersionId { get; set; }
		public string Name{ get; set; }
        public string Unit { get; set; }
		public string Description{ get; set; }
    }
}
