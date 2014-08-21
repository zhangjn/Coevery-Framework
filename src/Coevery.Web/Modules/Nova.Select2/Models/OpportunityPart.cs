
using System;
using System.Collections.Generic;
using Coevery.ContentManagement;

namespace Nova.Select2.Models {
    public sealed class OpportunityPart : ContentPart<OpportunityPartRecord> {

		public string Name{
			get{ return Record.Name; }
			set{ Record.Name = value; }
		}

		public int? OriginatingLead{
			get{ return Record.OriginatingLead; }
			set{ Record.OriginatingLead = value; }
		}
    }
}
