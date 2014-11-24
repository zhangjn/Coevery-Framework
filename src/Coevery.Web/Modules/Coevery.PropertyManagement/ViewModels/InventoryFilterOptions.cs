using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class InventoryFilterOptions
    {
        public string VoucherNo { get; set; }
        public int? SupplierId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}