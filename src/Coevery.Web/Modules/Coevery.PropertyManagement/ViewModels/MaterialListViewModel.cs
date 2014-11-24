using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public sealed class MaterialListViewModel
    {
        public int Id { get; set; }
        public string SerialNo { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
        public decimal StockPrice { get; set; }
    }

    public class MaterialFilterOptions
    {
        public string Search { get; set; }
        public string SerialNo { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Unit { get; set; }
    }
}