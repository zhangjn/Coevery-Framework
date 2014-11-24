using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.OpenXml4Net.OPC.Internal;

namespace Coevery.PropertyManagement.ViewModels
{
    public class AdvancePaymentListViewModel
    {
        public int Id { get; set; }
        public decimal Paid { get; set; }
        public string PaidDate {  get; set;}
        public string OperatorName {  get; set;}
        public string CustomerName { get; set; }
    }
}