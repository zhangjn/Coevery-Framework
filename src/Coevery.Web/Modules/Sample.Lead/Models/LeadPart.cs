using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement;

namespace Sample.Lead.Models {
    public class LeadPart : ContentPart<LeadPartRecord> {
        public string Subject {
            get { return Record.Subject; }
            set { Record.Subject = value; }
        }

        public string Description {
            get { return Record.Description; }
            set { Record.Description = value; }
        }

        public string CompanyName {
            get { return Record.CompanyName; }
            set { Record.CompanyName = value; }
        }
    }
}