using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("PaymentMethodItem")]
    public class PaymentMethodItemRecord
    {
        public virtual int Id { get; set; }
        //public virtual int PaymentId { get; set; }
        public virtual PaymentPartRecord Payment { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual PaymentMethodOption PaymentMethod { get; set; }//付款方式
        public virtual string Description { get; set; }//备注
    }
    public enum PaymentMethodOption
    {
        现金 = 1,
        刷卡 = 2,
        转账 = 3,
        其它 = 4,
        预付款=5
    }
}