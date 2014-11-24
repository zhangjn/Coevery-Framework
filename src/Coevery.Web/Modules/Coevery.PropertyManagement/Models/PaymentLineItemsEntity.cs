using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.Models
{
    public class PaymentLineItemsEntity
    {
        public int Id { get; set; } //bill id
        public int PaymentId { get; set; }
    }
}