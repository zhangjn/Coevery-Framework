using System;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("Bill")]
    public class BillRecord
    {
        public virtual int Id { get; set; }
        public virtual int CustomerId { get; set; }
        public virtual ContractPartRecord Contract { get; set; }
        public virtual HousePartRecord House { get; set; }
        public virtual ChargeItemSettingCommonRecord ChargeItem { get; set; }
        public virtual decimal? Amount { get; set; } //金额
        public virtual decimal? Exempt { get; set; }//优惠
        public virtual decimal? DelayCharge { get; set; }//滞纳金
        public virtual DateTime StartDate { get; set; } //开始时间
        public virtual DateTime EndDate { get; set; } //结束时间
        public virtual int? Year { get; set; }
        public virtual int? Month { get; set; }
        public virtual BillStatusOption Status { get; set; }
        public virtual string Notes { get; set; }
        public virtual double? Quantity { get; set; }//抄表类 用量

        public enum BillStatusOption
        {
            已出账单,
            已结账单
        }
    }
}