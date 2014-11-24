using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.ContentManagement;
using Coevery.Users.Models;

namespace Coevery.PropertyManagement.Models
{
    public class VoucherPart : ContentPart<VoucherPartRecord> 
    {
        public string VoucherNo
        {
            get { return Record.VoucherNo; }
            set { Record.VoucherNo = value; }
		}

        public DateTime Date
        {
            get { return Record.Date; }
            set { Record.Date = value; }
        }

        public int OperatorId
        {
            get { return Record.OperatorId; }
            set { Record.OperatorId = value; }
        }
        public int SupplierId
        {
            get { return Record.SupplierId; }
            set { Record.SupplierId = value; }
        }
        //public SupplierPartRecord Supplier
        //{
        //    get { return Record.Supplier; }
        //    set { Record.Supplier = value; }
        //}

        public int DepartmentId
        {
            get { return Record.DepartmentId; }
            set { Record.DepartmentId = value; }
        }
        public InventoryChangeOperation Operation
        {
            get { return Record.Operation; }
            set { Record.Operation = value; }
        }
        
    }
}