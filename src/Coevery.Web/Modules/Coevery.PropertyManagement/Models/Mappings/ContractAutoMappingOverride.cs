
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class ContractAutoMappingOverride : IAutoMappingOverride<ContractPartRecord>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<ContractPartRecord> mapping) {
            mapping.References(x => x.Renter).Column("RenterId");
            mapping.HasMany(x => x.Houses).KeyColumn("ContractId").Inverse();
            mapping.HasMany(x => x.ChargeItems).KeyColumn("ContractId").Inverse();
        }
    }
}