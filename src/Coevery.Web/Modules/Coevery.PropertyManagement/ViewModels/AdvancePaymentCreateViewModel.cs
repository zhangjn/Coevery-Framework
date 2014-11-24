using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class AdvancePaymentCreateViewModel
    {
        public AdvancePaymentCreateViewModel()
        {
            LineItems = new List<AdvancePaymentItemsEntity>();
        }

        public int CustomerId { get; set; }
        public decimal Paid { get; set; } //收款金额
        public List<AdvancePaymentItemsEntity> LineItems { get; set; }
    }
}