using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement;
using Coevery.Users.Models;

namespace Coevery.PropertyManagement.Models
{
    public class AdvancePaymentPart : ContentPart<AdvancePaymentPartRecord> 
    {
        public decimal Paid{
            get { return Record.Paid; }
            set { Record.Paid = value; }
		}

        public DateTime PaidOn
        {
            get { return Record.PaidOn; }
            set { Record.PaidOn = value; }
        }

        public CustomerPartRecord Customer{
            get { return Record.Customer; }
            set { Record.Customer = value; }
        }
        public UserPartRecord Operator
        {
            get { return Record.Operator; }
            set { Record.Operator = value; }
        }
    }
}