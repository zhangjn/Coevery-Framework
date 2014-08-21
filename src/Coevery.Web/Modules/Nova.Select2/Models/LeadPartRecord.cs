
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Coevery.ContentManagement.Records;

namespace Nova.Select2.Models {
	[Table("Lead")]
    public class LeadPartRecord : ContentPartRecord {
		public virtual string Subject { get; set; }
    }
}

