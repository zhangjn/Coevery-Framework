using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.PropertyManagement.Extensions;

namespace Coevery.PropertyManagement.ViewModels
{
    public class AdvancePaymentDetailViewModel
    {
        public int Id { get; set; }
        public string  AdvancePaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
     
    }

    public class AdvancePaymentVoucherViewModel
    {
        public string VoucherNo { get; set; }
        public string CustomerName { get; set; }
        public string CurrentDate
        {
            get { return DateTime.Now.ToString("yyyy/MM/dd"); }
        }

        public string BigTotalMoney
        {
            get
            {
                return TotalMoney.ToChineseString();
            }
        }

        public decimal TotalMoney
        {
            get
            {
                return List.Sum(m => m.Amount);
            }
        }

        public List<AdvancePaymentDetailViewModel> List { get; set; }
    }
}