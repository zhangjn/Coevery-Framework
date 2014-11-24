using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class PaymentFilterOptions
    {
        public int? ApartmentId { get; set; }
        public int? BuildingId { get; set; }
        public string HouseNumber { get; set; }
        public string ContractNumber { get; set; }
        public int? ContractId { get; set; }
        public int? ExpenserId { get; set; }

        public int? ContractIdByContract { get; set; }
        public int? ExpenserIdByContract { get; set; }
    }
}