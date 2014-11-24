using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class VoucherListViewModel
    {
        public int Id { get; set; }
        public string VoucherNo { get; set; }
        public string Operation { get; set; }
        //public DateTime Date { get; set; }
        public string  Date { get; set; }
        public string Remark { get; set; }
        public string SupplierName { get; set; }
        public string DepartmentName { get; set; }
    }
}