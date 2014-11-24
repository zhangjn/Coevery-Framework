using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class ContractFilterOptions
    {
        public string ContractNumber { get; set; }
        public string ContractName { get; set; }
        public int? RenterId { get; set; }
    }
}