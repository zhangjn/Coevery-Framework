using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.PropertyManagement.Extensions;

namespace Coevery.PropertyManagement.ViewModels
{
    public class ServiceVoucherCreateViewModel
    {
        public int id { get; set; }
        public string VoucherNo { get; set; }
        public int  ServicePersonId { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string Mobile { get; set; }
        public string FaultDescription { get; set; }
        public string ReceivedDate { get; set; }
        public ServiceVoucherCreateViewModel()
        {
            InventoryDetailEntities=new List<InventoryDetail>();
        }
         public List<InventoryDetail> InventoryDetailEntities { get; set; }

    }
    //显示订单
    public class ServiceCreateListViewModel
    {

        public string VoucherNo { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string Mobile { get; set; }
        public string FaultDescription { get; set; }
        public string ReceivedDate { get; set; }
        public string ServicePersonName { get; set; }

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
                return List.Sum(m => m.Total);
            }
        }

        public List<HelpListViewModel> List { get; set; }
    }
}