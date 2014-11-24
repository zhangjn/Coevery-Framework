using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("PaymentLineItem")]
    public class PaymentLineItemPartRecord
    {
        public virtual int Id { get; set; }
        public virtual BillRecord Bill { get; set; }
        public virtual PaymentPartRecord Payment { get; set; }
    }
}