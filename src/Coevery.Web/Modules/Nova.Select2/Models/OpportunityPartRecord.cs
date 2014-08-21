
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Coevery.ContentManagement.Records;

namespace Nova.Select2.Models {
	[Table("Opportunity")]
    public class OpportunityPartRecord : ContentPartRecord {
		public virtual string Name { get; set; }
		public virtual int? OriginatingLead { get; set; }
    }
}

