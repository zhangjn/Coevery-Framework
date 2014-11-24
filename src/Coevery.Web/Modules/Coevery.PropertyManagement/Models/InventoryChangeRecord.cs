using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("InventoryChange")]
    public class InventoryChangeRecord
    {
        public virtual int Id { get; set; }
        public virtual string VoucherNo { get; set; }
        public virtual decimal CostPrice { get; set; }
        public virtual int MaterialId { get; set; }
        public virtual InventoryChangeOperation Operation { get; set; }
        public virtual int Number { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int OperatorId { get; set; }
    }

    public enum InventoryChangeOperation
    {
        所有操作,
        入库,
        出库
     }
}