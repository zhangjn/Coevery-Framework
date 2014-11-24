using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.ViewModels
{
    public class PaymentMethodViewModel
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        //public int BillId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodOption PaymentMethod { get; set; }//付款方式
        public int PaymentMethodId { get; set; }//枚举类型Id
        public string Description { get; set; }//备注
    }
}