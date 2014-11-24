using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.OpenXml4Net.OPC.Internal;

namespace Coevery.PropertyManagement.ViewModels
{
    public class ChargeHistoryListViewModel
    {
        public int PaymentId;
        public DateTime? PaidTime;
        public decimal Paid;
        public string Operator;
        public string CustomerName;
    }
}