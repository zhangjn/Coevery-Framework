using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class StockDetailListViewModel
    {
        public int Id { get; set; }
        public string VoucherNo { get; set; }
        public string Operation { get; set; }
        //public DateTime Date { get; set; }
        public string Date { get; set; }
        public string Remark { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Number { get; set; }
        public string MaterialName { get; set; }
        public string MaterialSerialNo { get; set; }
        public string MaterialUnit { get; set; }
        public string OperatorName { get; set; }
        public string SupplierName { get; set; }
        public string DepartmentName { get; set; }

        public decimal Amount
        {
            get { return CostPrice*Number; }
        }
    }
}