
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.PropertyManagement.Models.Mappings
{
    public class ContractHouseAutoMappingOverride : IAutoMappingOverride<ContractHouseRecord>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<ContractHouseRecord> mapping) {
            mapping.References(x => x.Contract).Column("ContractId");
            mapping.References(x => x.House).Column("HouseId");
        }
    }
}