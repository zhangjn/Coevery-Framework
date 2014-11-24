using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class InventoryDetail
    {
       
        public int MaterialId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Unit { get; set; }
        public decimal CostPrice { get; set; }
        public int Number { get; set; }
    }
}