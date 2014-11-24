using System;
using System.Collections.Generic;
using System.Linq;
using Coevery.PropertyManagement.Extensions;


namespace Coevery.PropertyManagement.ViewModels
{
    public class HelpListViewModel {
        public int Id { get; set; }
        public string Material_SerialNo { get; set; }
        public string Material_Name { get; set; }
        public string Material_Brand { get; set; }
        public string Material_Model { get; set; }
        public string Material_Unit { get; set; }
        public int Number { get; set; }
        public decimal CostPrice { get; set; }
        public string SupplierName { get; set; }
        public string DepartmentName { get; set; }
        public int ReturnNumber { get; set; }
        public decimal Total {
            get { return CostPrice * Number; }
        }
    }

    public class VoucherCreateViewModel
    {
        public int SupplierId { get; set; }
        public int DepartmentId { get; set; }
        public string VoucherNo { get; set; }
        public string OperatorName { get; set; }
        public string Remark { get; set; }
        public VoucherCreateViewModel()
        {
            InventoryDetailEntities=new List<InventoryDetail>();
        }
         public List<InventoryDetail> InventoryDetailEntities { get; set; }

    }
    //显示订单
    public class InventoryCreateListViewModel
    {
        
        public string VoucherNo { get; set; }
        public string CurrentDate {
            get { return DateTime.Now.ToString("yyyy/MM/dd"); }
        }

        public string BigTotalMoney {
            get {
                return TotalMoney.ToChineseString();
            }
        }

        public decimal TotalMoney {
            get {
                return List.Sum(m => m.Total);
            }
        }
       
        public List<HelpListViewModel> List { get; set; }
    }
}