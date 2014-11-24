using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data.Conventions;
using Coevery.Users.Models;
using Coevery.ContentManagement.Records;

namespace Coevery.PropertyManagement.Models
{
     [Table("Payment")]
    public class PaymentPartRecord : ContentPartRecord
    {
        public PaymentPartRecord()
        {
            LineItems = new List<PaymentLineItemPartRecord>();
            PaymentMethodItems = new List<PaymentMethodItemRecord>();
        }
       // public int Id { get; set; }
        public virtual decimal Paid { get; set; }
        public virtual DateTime? PaidOn { get; set; }
        public virtual CustomerPartRecord CustomerId { get; set; }
        public virtual UserPartRecord Operator { get; set; }

        [CascadeAllDeleteOrphan, Aggregate]
        public virtual IList<PaymentLineItemPartRecord> LineItems { get; set; }

        [CascadeAllDeleteOrphan, Aggregate]
        public virtual IList<PaymentMethodItemRecord> PaymentMethodItems { get; set; }
    }
}