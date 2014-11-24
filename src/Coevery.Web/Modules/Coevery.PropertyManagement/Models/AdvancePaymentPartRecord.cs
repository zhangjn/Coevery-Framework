using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data.Conventions;
using Coevery.Users.Models;
using Coevery.ContentManagement.Records;

namespace Coevery.PropertyManagement.Models
{
     [Table("AdvancePayment")]
    public class AdvancePaymentPartRecord : ContentPartRecord
    {
         public AdvancePaymentPartRecord()
        {
            AdvancePaymentItems = new List<AdvancePaymentItemRecord>();
        }
  
        public virtual decimal Paid { get; set; }
        public virtual DateTime PaidOn { get; set; }
        public virtual CustomerPartRecord Customer { get; set; }
        public virtual UserPartRecord Operator { get; set; }

        [CascadeAllDeleteOrphan, Aggregate]
        public virtual IList<AdvancePaymentItemRecord> AdvancePaymentItems { get; set; }
    }
}