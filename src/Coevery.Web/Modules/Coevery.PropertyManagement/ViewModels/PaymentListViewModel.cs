using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coevery.PropertyManagement.Models;

namespace Coevery.PropertyManagement.ViewModels
{
    public class PaymentListViewModel
    {
        public PaymentListViewModel()
        {
            ApartmentsList=new List<SelectListItem>();
            CustomersList=new List<SelectListItem>();
            IncidentalContractList=new List<SelectListItem>();
            IncidentalChargeItemList = new List<SelectListItem>();
            LineItems = new List<PaymentLineItemsEntity>();
        }
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ContractId { get; set; }
        public int HouseId { get; set; }
        public int ChargeItemSettingId { get; set; }
        public string ChargeItemSettingDescription { get; set; }
        public string ContractNumber { get; set; } //合同号
        public string ApartmentName { get; set; }//楼盘名称
        public string BuildingName { get; set; }//楼宇名称
        public string HouseNumber { get; set; }
        public string ChargeItemName { get; set; } //收费项目名称
        public string ChargeItemSettingName { get; set; } //费用项-收费标准项目名称
        public string BeginDate { get; set; } //开始时间
        public string EndDate { get; set; } //结束时间
        public decimal? Amount { get; set; } //金额
        public  decimal? Exempt { get; set; }//优惠
        public decimal? DelayCharge { get; set; }//滞纳金
        
        public decimal? Total {
            get
            {
                decimal amount = Amount ?? 0;
                decimal extempt = Exempt ?? 0;
                decimal delayCharge = DelayCharge ?? 0;
                return amount - extempt + delayCharge;
            }
        }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public double? Quantity { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        public decimal Paid { get; set; } //收款金额
        public List<SelectListItem> ApartmentsList;

        public List<SelectListItem> CustomersList;
        public List<SelectListItem> IncidentalContractList;
        public List<SelectListItem> IncidentalChargeItemList;
        public List<PaymentLineItemsEntity> LineItems { get; set; }
    }
}