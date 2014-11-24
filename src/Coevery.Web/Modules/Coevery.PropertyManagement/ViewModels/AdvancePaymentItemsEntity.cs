using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class AdvancePaymentItemsEntity
    {
      //  public int AdvancePaymentId { get; set; }
        public decimal Amount { get; set; }
        public string AdvancePaymentMethod { get; set; }
        public int AdvancePaymentMethodId { get; set; }
        public string Description { get; set; }
    }
}