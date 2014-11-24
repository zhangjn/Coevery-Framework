using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement;
using Coevery.Users.Models;

namespace Coevery.PropertyManagement.Models
{
    public class PaymentPart : ContentPart<PaymentPartRecord> 
    {
        public decimal Paid{
            get { return Record.Paid; }
            set { Record.Paid = value; }
		}

        public DateTime? PaidOn
        {
            get { return Record.PaidOn; }
            set { Record.PaidOn = value; }
        }

        public CustomerPartRecord CustomerId
        {
            get { return Record.CustomerId; }
            set { Record.CustomerId = value; }
        }
        public UserPartRecord Operator
        {
            get { return Record.Operator; }
            set { Record.Operator = value; }
        }
    }
}