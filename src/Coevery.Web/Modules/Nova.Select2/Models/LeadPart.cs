
using System;
using System.Collections.Generic;
using Coevery.ContentManagement;

namespace Nova.Select2.Models {
    public sealed class LeadPart : ContentPart<LeadPartRecord> {

		public string Subject{
			get{ return Record.Subject; }
			set{ Record.Subject = value; }
		}
    }
}
