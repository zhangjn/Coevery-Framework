using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.Data.Conventions;
using Coevery.Users.Models;
using Coevery.ContentManagement.Records;

namespace Coevery.PropertyManagement.Models
{
    [Table("Voucher")]
    public class VoucherPartRecord : ContentPartRecord
    {
        public VoucherPartRecord()
        {
            InventoryItems = new List<InventoryChangeRecord>();
        }

        public virtual string VoucherNo { get; set; }
        public virtual InventoryChangeOperation Operation { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int OperatorId { get; set; }
        //public virtual SupplierPartRecord Supplier { get; set; }
        public virtual int SupplierId { get; set; }
        public virtual int DepartmentId { get; set; }
        public virtual string Remark { get; set; }


        [CascadeAllDeleteOrphan, Aggregate]
        public virtual IList<InventoryChangeRecord> InventoryItems { get; set; }
    }
}