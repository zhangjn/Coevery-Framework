using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement.Records;

namespace Sample.Lead.Models
{
    public class LeadPartRecord: ContentPartRecord
    {
        public virtual string Subject { get; set; }
        public virtual string Description { get; set; }
        public virtual string CompanyName { get; set; }
    }
}