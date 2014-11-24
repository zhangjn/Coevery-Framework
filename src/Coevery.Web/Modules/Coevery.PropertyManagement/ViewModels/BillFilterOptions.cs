using System;

namespace Coevery.PropertyManagement.ViewModels
{
    public class BillFilterOptions
    {
        public string  ApartmentId { get; set; }
        public string BuildingId { get; set; }
        public string HouseNumber { get; set; }
        public string ContractNumber { get; set; }
        public string OwnerIds { get; set; } //多选情况
        public string RenterIds { get; set; } //多选情况
        public int? OwnerId { get; set; }
        public int? RenterId { get; set; }
        public int? OfficerId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string HouseStatus { get; set; }
        public int? ExpenserId { get; set; }
        public int? ChargeItemId { get; set; }
    }
}