using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("AdvancePaymentItem")]
    public class AdvancePaymentItemRecord
    {
        public virtual int Id { get; set; }
        public virtual AdvancePaymentPartRecord Payment { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual AdvancePaymentMethodOption AdvancePaymentMethod { get; set; }//付款方式
        public virtual string Description { get; set; }//备注
    }
    public enum AdvancePaymentMethodOption
    {
        现金 = 1,
        刷卡 = 2,
        转账 = 3,
        其它 = 4
    }
}