using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery.PropertyManagement.Extensions;

namespace Coevery.PropertyManagement.ViewModels
{
    public class MaterialReturnViewModel
    {
        public int Id { get; set; }
        public decimal ServiceCharge { get; set; }
        public MaterialReturnViewModel()
        {
            InventoryReturnDetailEntities = new List<InventoryReturnDetail>();
        }
        public List<InventoryReturnDetail> InventoryReturnDetailEntities { get; set; }

    }
   // 显示账单
    public class ServiceChargeCreateViewModel
    {
        public int id { get; set; }
        public string VoucherNo { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string Mobile { get; set; }
        public string FaultDescription { get; set; }
        public string ReceivedDate { get; set; }
        public string ServicePersonName { get; set; }
        public decimal ServiceCharge { get; set; }
        public ServiceChargeCreateViewModel()
        {
            InventoryDetailEntities = new List<InventoryDetail>();
        }
        
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
                return List.Sum(m => m.Total) + ServiceCharge;
            }
        }
        public List<HelpListViewModel> List { get; set; }
        public List<InventoryDetail> InventoryDetailEntities { get; set; }
        
    }
}