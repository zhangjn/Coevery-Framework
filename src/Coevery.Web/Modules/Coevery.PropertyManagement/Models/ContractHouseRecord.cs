
using Coevery.Data.Conventions;

namespace Coevery.PropertyManagement.Models
{
    [Table("ContractHouse")]
    public class ContractHouseRecord
    {
        public virtual int Id { get; set; }
        public virtual HousePartRecord House { get; set; }
        public virtual ContractPartRecord Contract { get; set; }
    }
}