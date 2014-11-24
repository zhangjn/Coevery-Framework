using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class StockSummaryListViewModel
    {
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public string MaterialSerialNo { get; set; }
        public string MaterialUnit { get; set; }
        //期初
        public decimal BeginningCostPrice { get; set; }
        public decimal BeginningNumber { get; set; }
        public decimal BeginningAmount { get; set; }
        //{
        //    get { return BeginningCostPrice * BeginningNumber; }
        //}
        //本期采购
        public decimal StockInPrice { get; set; }
        public decimal StockInNumber { get; set; }
        public decimal StockInAmount { get; set; }
        //{
        //    get { return StockInPrice * StockInNumber; }
        //}
        //本期领用
        public decimal StockOutPrice { get; set; }
        public decimal StockOutNumber { get; set; }
        public decimal StockOutAmount { get; set; }
        //{
        //    get { return StockOutPrice * StockOutNumber; }
        //}
        //期末
        public decimal FinalCostPrice { get; set; }
        public decimal FinalNumber
        {
            get { return BeginningNumber + StockInNumber - StockOutNumber; }
        }
        public decimal FinalAmount 
        {
            get { return BeginningAmount + StockInAmount - StockOutAmount; }
        }

       
    }
}