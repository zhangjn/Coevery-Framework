using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("Inventory")]
    public class InventoryRecord
    {
        public virtual int Id { get; set; }
        public virtual int MaterialId { get; set; }
        public virtual int Number { get; set; }
        public virtual decimal CostPrice { get; set; }
        public virtual decimal Amount { get; set; }
    }
}