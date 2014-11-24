namespace Coevery.PropertyManagement.ViewModels
{
    public class ContractHouseEntity
    {
        public int Id { get; set; } //别忘了Id
        public int ContractId { get; set; } //合同Id

        public int HouseId { get; set; } //房间id

        public string HouseNumber { get; set; } //房间号
        public string ApartmentName { get; set; } //楼盘名称
        public string BuildingName { get; set; } //楼宇名称
        public string OwnerName { get; set; } //业主名称

        public double? BuildingArea { get; set; }
        public double? InsideArea { get; set; }
        public double? PoolArea { get; set; }
    
    }
}