
using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.PropertyManagement.Extensions;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.ViewModels
{
    public class CheckOutViewModel
    {
        public CheckOutViewModel()
        {
            List = new List<PaymentListViewModel>();
        }

        public List<PaymentListViewModel> List { get; set; }
        public string ChargeUser { get; set; }
        public string ChargeDate { get; set; }

        public decimal TotalMoney
        {
            get { return List.Sum(x => x.Total ?? 0); }
        }

        public string ToTalBigMoney
        {
            get { return TotalMoney.ToChineseString(); } //中文大写
        }

        public CustomerPartRecord Expenser { get; set; } //显示费用承担者，传递参数

        public ContractPartRecord Contract { get; set; } //保存合同，传递参数
        //缴费方式
    }
}