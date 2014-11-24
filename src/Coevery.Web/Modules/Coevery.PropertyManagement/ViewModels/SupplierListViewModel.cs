using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public sealed class SupplierListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contactor { get; set; }
        public string Tel { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string QQ { get; set; }
        public string Fax { get; set; }
        public string Description { get; set; }
    }

    public class SupplierFilterOptions
    {
        public string Search { get; set; }
        public string Contactor { get; set; }
    }
}